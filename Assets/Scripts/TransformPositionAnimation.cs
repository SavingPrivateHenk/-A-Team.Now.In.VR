using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPositionAnimation : MonoBehaviour
{
    private Vector3 m_origin;
    [SerializeField]
    private Vector3 m_target;

    [SerializeField, Tooltip("Duration (ms).")]
    private float m_duration;

    [SerializeField]
    private TransitionType m_transitionType;
    private TransitionCurve m_transitionCurve;

    private void Awake()
    {
        m_origin = transform.localPosition;
        m_transitionCurve = new TransitionCurve(m_transitionType);
    }

    [ContextMenu(nameof(MoveToTarget))]
    public void MoveToTarget() => TransformPosition(m_target, m_duration);

    [ContextMenu(nameof(MoveToOrigin))]
    public void MoveToOrigin() => TransformPosition(m_origin, m_duration);

    private void TransformPosition(Vector3 target, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateTransformation(transform.localPosition, target, duration / 1000f));
    }

    private IEnumerator AnimateTransformation(Vector3 current, Vector3 target, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime = elapsedTime + Time.deltaTime;
            transform.localPosition = Vector3.Lerp(current, target, m_transitionCurve.Interpolate(elapsedTime / duration));
            yield return null;
        }
    }
}