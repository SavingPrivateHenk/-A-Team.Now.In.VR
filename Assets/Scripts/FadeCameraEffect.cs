using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class FadeCameraEffect : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private TransitionCurve _transitionCurve;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 1f;
        _transitionCurve = new TransitionCurve(TransitionType.Smoother);
    }

    /// <summary>
    /// Fade in on canvas with given duration (ms).
    /// </summary>
    public void FadeIn(float duration, UnityEvent unityEvent)
    {
        StopAllCoroutines();
        StartCoroutine(FadeAnimation(1f, 0f, duration, unityEvent));
    }

    /// <summary>
    /// Fade out on canvas with given duration (ms).
    /// </summary>
    public void FadeOut(float duration, UnityEvent unityEvent)
    {
        StopAllCoroutines();
        StartCoroutine(FadeAnimation(0f, 1f, duration, unityEvent));
    }

    private IEnumerator FadeAnimation(float alphaOrigin, float alphaTarget, float duration, UnityEvent unityEvent)
    {
        duration = duration / 1000f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime = elapsedTime + Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(alphaOrigin, alphaTarget, _transitionCurve.Interpolate(elapsedTime / duration));
            yield return null;
        }
        unityEvent?.Invoke();
    }
}