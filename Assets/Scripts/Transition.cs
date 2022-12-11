using System;
using UnityEngine;

public class Transition
{
    private readonly Func<float, float> m_function;

    public Transition(TransitionType transitionType)
    {
        m_function = transitionType switch
        {
            TransitionType.Linear => Linear,
            TransitionType.EaseIn => EaseIn,
            TransitionType.EaseOut => EaseOut,
            TransitionType.Smooth => Smooth,
            TransitionType.Smoother => Smoother,
            _ => Linear
        };
    }

    public float Interpolate(float value)
    {
        return m_function(value);
    }

    // Interpolation functions: linear, sin, cos, https://en.wikipedia.org/wiki/Smoothstep
    private readonly Func<float, float> Linear = (value) => Mathf.Clamp01(value);
    private readonly Func<float, float> EaseIn = (value) => Mathf.Clamp01(1f - Mathf.Cos(value * Mathf.PI * 0.5f));
    private readonly Func<float, float> EaseOut = (value) => Mathf.Clamp01(Mathf.Sin(value * Mathf.PI * 0.5f));
    private readonly Func<float, float> Smooth = (value) => Mathf.Clamp01(Mathf.Pow(value, 2) * (3f - 2f * value));
    private readonly Func<float, float> Smoother = (value) => Mathf.Clamp01(Mathf.Pow(value, 3) * (value * (value * 6f - 15f) + 10));
}