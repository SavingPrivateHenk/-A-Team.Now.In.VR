using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _targetFrameRate = 60;

    [SerializeField]
    private GameObject _xrOrigin;
    [SerializeField, Space]
    private Canvas _cameraCanvas;

    [SerializeField, Tooltip("Fade in duration (ms)."), Space]
    private float _fadeInDuration = 1000f;
    [SerializeField, Tooltip("Fade out duration (ms).")]
    private float _fadeOutDuration = 1000f;

    [SerializeField, Space]
    private UnityEvent _onSceneLoad;

    private FadeCameraEffect _fadeCameraEffect;

    private void Awake()
    {
        Application.targetFrameRate = _targetFrameRate;
        SceneManager.sceneLoaded += OnSceneLoaded;

        _fadeCameraEffect = _cameraCanvas.GetComponent<FadeCameraEffect>();
    }

    public void LoadScene(string scene)
    {
        DataPersistence.Instance.XROriginTransform[SceneManager.GetActiveScene().buildIndex] = (_xrOrigin.transform.position, _xrOrigin.transform.rotation);

        var loadScene = new UnityEvent();
        loadScene.AddListener(() => SceneManager.LoadScene(scene, LoadSceneMode.Single));
        _fadeCameraEffect.FadeOut(_fadeOutDuration, loadScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        // Transforming the Origin to the position the character originally left the scene at
        // will cause the Scene Change to Trigger again, causing an endless loop.
        //TransformXROrigin(scene);
        _fadeCameraEffect.FadeIn(_fadeInDuration, _onSceneLoad);
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