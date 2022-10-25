using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class UnityEventExtensions
{
    /// <summary>
    /// Coroutine to invoke all registered callbacks (runtime and persistent) after a delay (ms).
    /// </summary>
    public static IEnumerator Invoke(this UnityEvent unityEvent, float delay)
    {
        yield return new WaitForSeconds(delay / 1000f);
        unityEvent?.Invoke();
    }

    /// <summary>
    /// Coroutine to invoke all registered callbacks (runtime and persistent) after a delay (ms).
    /// </summary>
    public static IEnumerator Invoke<T0>(this UnityEvent<T0> unityEvent, T0 t0, float delay)
    {
        yield return new WaitForSeconds(delay / 1000f);
        unityEvent?.Invoke(t0);
    }

    /// <summary>
    /// Coroutine to invoke all registered callbacks (runtime and persistent) after a delay (ms).
    /// </summary>
    public static IEnumerator Invoke<T0, T1>(this UnityEvent<T0, T1> unityEvent, T0 t0, T1 t1, float delay)
    {
        yield return new WaitForSeconds(delay / 1000f);
        unityEvent?.Invoke(t0, t1);
    }

    /// <summary>
    /// Coroutine to invoke all registered callbacks (runtime and persistent) after a delay (ms).
    /// </summary>
    public static IEnumerator Invoke<T0, T1, T2>(this UnityEvent<T0, T1, T2> unityEvent, T0 t0, T1 t1, T2 t2, float delay)
    {
        yield return new WaitForSeconds(delay / 1000f);
        unityEvent?.Invoke(t0, t1, t2);
    }

    /// <summary>
    /// Coroutine to invoke all registered callbacks (runtime and persistent) after a delay (ms).
    /// </summary>
    public static IEnumerator Invoke<T0, T1, T2, T3>(this UnityEvent<T0, T1, T2, T3> unityEvent, T0 t0, T1 t1, T2 t2, T3 t3, float delay)
    {
        yield return new WaitForSeconds(delay / 1000f);
        unityEvent?.Invoke(t0, t1, t2, t3);
    }
}