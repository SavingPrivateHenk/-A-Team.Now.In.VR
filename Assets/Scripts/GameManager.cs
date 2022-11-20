using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _targetFrameRate = 60;

    [SerializeField]
    private GameObject _xrOrigin;

    [SerializeField]
    private ShoppingBasketManager _basket;

    [SerializeField]
    private FadeEffect _fadeEffect;

    [Space]
    [SerializeField]
    private UnityEvent _onSceneLoad;

    private void Awake()
    {
        Application.targetFrameRate = _targetFrameRate;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string scene)
    {
        DataPersistence.Instance.XROriginTransform[SceneManager.GetActiveScene().buildIndex] = (
            _xrOrigin.transform.position, _xrOrigin.transform.rotation);
        DataPersistence.Instance.BasketItems = _basket.Items;

        _fadeEffect.FadeOut(LoadSceneEvent(scene));
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
        // TODO: Transform Origin without trigger in Scene Load
        _fadeEffect.FadeIn(_onSceneLoad);
    }

    private void TransformXROrigin(Scene scene)
    {
        int buildIndex = scene.buildIndex;
        var xrOriginTransform = DataPersistence.Instance.XROriginTransform;

        if (xrOriginTransform.ContainsKey(buildIndex))
        {
            var transform = xrOriginTransform[buildIndex];
            _xrOrigin.transform.position = transform.position;
            _xrOrigin.transform.rotation = transform.rotation;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}