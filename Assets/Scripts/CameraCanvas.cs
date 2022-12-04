using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class CameraCanvas : MonoBehaviour
{
    private CanvasGroup m_canvasGroup;
    private TransitionCurve m_transitionCurve;

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_transitionCurve = new TransitionCurve(TransitionType.Smoother);
    }

    public void FadeIn(UnityEvent unityEvent, float duration = 1000f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeAnimation(1f, 0f, duration, unityEvent));
    }

    public void FadeOut(UnityEvent unityEvent, float duration = 1000f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeAnimation(0f, 1f, duration, unityEvent));
    }

    private IEnumerator FadeAnimation(float alphaOrigin, float alphaTarget, float duration, UnityEvent unityEvent)
    {
        duration /= 1000f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime = elapsedTime + Time.deltaTime;
            m_canvasGroup.alpha = Mathf.Lerp(alphaOrigin, alphaTarget, m_transitionCurve.Interpolate(elapsedTime / duration));
            yield return null;
        }
        unityEvent?.Invoke();
    }
}