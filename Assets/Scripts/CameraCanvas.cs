using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Image))]
public class CameraCanvas : MonoBehaviour
{
    private Canvas m_canvas;
    private CanvasGroup m_canvasGroup;
    private Transition m_transition;
    private Image m_image;

    private void Awake()
    {
        m_canvas = GetComponent<Canvas>();
        m_canvas.renderMode = RenderMode.ScreenSpaceCamera;
        m_canvas.worldCamera = Camera.main;
        m_canvas.planeDistance = 0.1f;
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_transition = new Transition(TransitionType.Smoother);
        m_image = GetComponent<Image>();
        m_image.color = Color.black;
    }

    public void FadeIn(UnityEvent unityEvent, TimeSpan duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeAnimation(1f, 0f, duration, unityEvent));
    }

    public void FadeOut(UnityEvent unityEvent, TimeSpan duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeAnimation(0f, 1f, duration, unityEvent));
    }

    private IEnumerator FadeAnimation(float alphaOrigin, float alphaTarget, TimeSpan duration, UnityEvent unityEvent)
    {
        var elapsedTime = 0f;
        while (elapsedTime < duration.Seconds)
        {
            elapsedTime += Time.deltaTime;
            m_canvasGroup.alpha = Mathf.Lerp(alphaOrigin, alphaTarget, m_transition.Interpolate(elapsedTime / duration.Seconds));
            yield return null;
        }
        unityEvent?.Invoke();
    }
}