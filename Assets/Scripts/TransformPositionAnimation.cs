using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPositionAnimation : MonoBehaviour
{
    private Vector3 _origin;
    [SerializeField]
    private Vector3 _target;

    [SerializeField, Tooltip("Duration (ms).")]
    private float _duration;

    [SerializeField]
    private TransitionType _transitionType;
    private TransitionCurve _transitionCurve;

    private void Awake()
    {
        _origin = transform.localPosition;
        _transitionCurve = new TransitionCurve(_transitionType);
    }

    [ContextMenu(nameof(MoveToTarget))]
    public void MoveToTarget() => TransformPosition(_target, _duration);

    [ContextMenu(nameof(MoveToOrigin))]
    public void MoveToOrigin() => TransformPosition(_origin, _duration);

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
            transform.localPosition = Vector3.Lerp(current, target, _transitionCurve.Interpolate(elapsedTime / duration));
            yield return null;
        }
    }
}