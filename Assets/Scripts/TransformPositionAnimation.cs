using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class TransformPositionAnimation : MonoBehaviour
{
    private Vector3 _origin;
    [SerializeField]
    private Vector3 _target;

    [SerializeField, Tooltip("Duration in milliseconds.")]
    private float _duration;

    [SerializeField]
    private TransitionCurve _transitionCurve;
    private Func<float, float> _interpolate;

    private void Awake()
    {
        _origin = transform.localPosition;
        _interpolate = _transitionCurve switch
        {
            TransitionCurve.Linear => Linear,
            TransitionCurve.EaseIn => EaseIn,
            TransitionCurve.EaseOut => EaseOut,
            TransitionCurve.Smooth => Smooth,
            TransitionCurve.Smoother => Smoother,
            _ => Linear
        };
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
            transform.localPosition = Vector3.Lerp(current, target, _interpolate(elapsedTime / duration));
            yield return null;
        }
    }

    public enum TransitionCurve
    {
        Linear,
        EaseIn,
        EaseOut,
        Smooth,
        Smoother
    }

    // Interpolation functions: sin, cos, https://en.wikipedia.org/wiki/Smoothstep
    private readonly Func<float, float> Linear = (value) => Mathf.Clamp01(value);
    private readonly Func<float, float> EaseIn = (value) => Mathf.Clamp01(1f - Mathf.Cos(value * Mathf.PI * 0.5f));
    private readonly Func<float, float> EaseOut = (value) => Mathf.Clamp01(Mathf.Sin(value * Mathf.PI * 0.5f));
    private readonly Func<float, float> Smooth = (value) => Mathf.Clamp01(Mathf.Pow(value, 2) * (3f - 2f * value));
    private readonly Func<float, float> Smoother = (value) => Mathf.Clamp01(Mathf.Pow(value, 3) * (value * (value * 6f - 15f) + 10));
}