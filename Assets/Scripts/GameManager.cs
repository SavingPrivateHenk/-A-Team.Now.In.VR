using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _targetFrameRate = 60;

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
        var loadScene = new UnityEvent();
        loadScene.AddListener(() => SceneManager.LoadScene(scene, LoadSceneMode.Single));
        _fadeCameraEffect.FadeOut(_fadeOutDuration, loadScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {

        _fadeCameraEffect.FadeIn(_fadeInDuration, _onSceneLoad);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}