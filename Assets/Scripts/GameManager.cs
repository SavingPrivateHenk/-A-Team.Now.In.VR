using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private int m_targetFrameRate = 60;
    [Space]
    [SerializeField] 
    private UnityEvent m_onSceneLoad;
    private CameraCanvas m_fadeCanvas;
    [SerializeField]
    private XROrigin xrOrigin;

    private void Awake()
    {
        Application.targetFrameRate = m_targetFrameRate;
        SceneManager.sceneLoaded += OnSceneLoaded;
        m_fadeCanvas = Instantiate(
            Resources.Load<GameObject>("Prefaps/Fade Canvas"),
            Camera.main.transform).GetComponent<CameraCanvas>();
        xrOrigin.GetComponent<TeleportationProvider>().enabled = Persistence.Instance.hasTeleportation;
        xrOrigin.GetComponent<ActionBasedContinuousMoveProvider>().enabled = !Persistence.Instance.hasTeleportation;
        xrOrigin.GetComponent<ActionBasedSnapTurnProvider>().enabled = Persistence.Instance.hasSnapTurn;
        xrOrigin.GetComponent<ActionBasedContinuousTurnProvider>().enabled = !Persistence.Instance.hasSnapTurn;
    }

    public void LoadScene(string sceneName, Vector3 returnPosition, Quaternion returnRotation)
    {
        Persistence.Instance.LocationInScenes[SceneManager.GetActiveScene().buildIndex] = (returnPosition, returnRotation);
        m_fadeCanvas.FadeOut(LoadSceneEvent(sceneName));
    }

    private UnityEvent LoadSceneEvent(string scene)
    {
        var loadEvent = new UnityEvent();

        loadEvent.AddListener(
            () => SceneManager.LoadScene(scene, LoadSceneMode.Single));

        return loadEvent;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        SetPlayerLocation(scene);
        m_fadeCanvas.FadeIn(m_onSceneLoad);
    }

    private void SetPlayerLocation(Scene scene)
    {
        if (Persistence.Instance.LocationInScenes.TryGetValue(scene.buildIndex, out var location))
        {
            FindObjectOfType<XROrigin>().transform.SetPositionAndRotation(location.position, location.rotation);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Persistence.Instance.hasTeleportation = xrOrigin.GetComponent<TeleportationProvider>().enabled;
        Persistence.Instance.hasSnapTurn = xrOrigin.GetComponent<ActionBasedSnapTurnProvider>().enabled;
    }
}