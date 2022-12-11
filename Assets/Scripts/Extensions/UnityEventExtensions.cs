using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class UnityEventExtensions
{
    public static UnityEvent AddAction(this UnityEvent @this, UnityAction action)
    {
        @this.AddListener(action);
        return @this;
    }

    public static IEnumerator Invoke(this UnityEvent @this, TimeSpan delay)
    {
        yield return new WaitForSeconds(delay.Seconds);
        @this?.Invoke();
    }

    public static IEnumerator Invoke<T0>(this UnityEvent<T0> @this, T0 t0, TimeSpan delay)
    {
        yield return new WaitForSeconds(delay.Seconds);
        @this?.Invoke(t0);
    }

    public static IEnumerator Invoke<T0, T1>(this UnityEvent<T0, T1> @this, T0 t0, T1 t1, TimeSpan delay)
    {
        yield return new WaitForSeconds(delay.Seconds);
        @this?.Invoke(t0, t1);
    }

    public static IEnumerator Invoke<T0, T1, T2>(this UnityEvent<T0, T1, T2> @this, T0 t0, T1 t1, T2 t2, TimeSpan delay)
    {
        yield return new WaitForSeconds(delay.Seconds);
        @this?.Invoke(t0, t1, t2);
    }

    public static IEnumerator Invoke<T0, T1, T2, T3>(this UnityEvent<T0, T1, T2, T3> @this, T0 t0, T1 t1, T2 t2, T3 t3, TimeSpan delay)
    {
        yield return new WaitForSeconds(delay.Seconds);
        @this?.Invoke(t0, t1, t2, t3);
    }
}