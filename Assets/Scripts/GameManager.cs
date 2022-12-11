using System;
using Unity.XR.CoreUtils;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public XROrigin XROrigin { get; private set; }
    public Camera Camera { get; private set; }
    public CameraCanvas CameraCanvas { get; private set; }
    public ActionBasedController LeftController { get; private set; }
    public ActionBasedController RightController { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public BasketManager BasketManager { get; private set; }
    public Scene Scene { get; set; }

    [SerializeField] private int m_targetFrameRate = 60;
    [SerializeField] private UnityEvent m_onSceneLoad;

    private void Awake()
    {
        Application.targetFrameRate = m_targetFrameRate;
        SceneManager.sceneLoaded += OnSceneLoaded;

        Scene = SceneManager.GetActiveScene();
        XROrigin = FindObjectOfType<XROrigin>();
        Camera = XROrigin.Camera;
        CameraCanvas = Camera.AddComponentAsChild<CameraCanvas>("Camera Canvas");
        LeftController = XROrigin.GetComponentOfNamedChild<ActionBasedController>("LeftHand Controller");
        RightController = XROrigin.GetComponentOfNamedChild<ActionBasedController>("RightHand Controller");
        SoundManager = gameObject.AddComponent<SoundManager>();
        SoundManager.XROrigin = XROrigin;
        BasketManager = gameObject.AddComponent<BasketManager>();
        BasketManager.BasketAnchor = LeftController;
        BasketManager.ToggleBasket = LeftController.uiPressAction.reference;
        BasketManager.Scene = Scene;       
    }

    private void Start()
    {
        XROrigin.GetComponent<TeleportationProvider>().enabled = Persistence.Instance.hasTeleportation;
        XROrigin.GetComponent<ActionBasedContinuousMoveProvider>().enabled = !Persistence.Instance.hasTeleportation;
        XROrigin.GetComponent<ActionBasedSnapTurnProvider>().enabled = Persistence.Instance.hasSnapTurn;
        XROrigin.GetComponent<ActionBasedContinuousTurnProvider>().enabled = !Persistence.Instance.hasSnapTurn;
    }

    public void LoadScene(string sceneName, Vector3 returnPosition, Quaternion returnRotation)
    {
        Persistence.Instance.LocationInScenes[SceneManager.GetActiveScene().buildIndex] = (returnPosition, returnRotation);
        Persistence.Instance.hasTeleportation = XROrigin.GetComponent<TeleportationProvider>().enabled;
        Persistence.Instance.hasSnapTurn = XROrigin.GetComponent<ActionBasedSnapTurnProvider>().enabled;
        CameraCanvas.FadeOut(LoadSceneEvent(sceneName), TimeSpan.FromSeconds(1));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (Persistence.Instance.LocationInScenes.TryGetValue(scene.buildIndex, out var location))
        {
            XROrigin.transform.SetPositionAndRotation(location.position, location.rotation);
        }
        CameraCanvas.FadeIn(m_onSceneLoad, TimeSpan.FromSeconds(1));
    }

    private UnityEvent LoadSceneEvent(string scene) => new UnityEvent().AddAction(() => SceneManager.LoadScene(scene, LoadSceneMode.Single));

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}