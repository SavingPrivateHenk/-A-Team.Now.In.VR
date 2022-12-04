using System;
using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
    public static bool TryFindComponentInChildren<T>(this GameObject @this, bool includeInactive, Func<T, bool> predicate, out T result)
    {
        result = @this.GetComponentsInChildren<T>(includeInactive).FirstOrDefault(predicate);
        return result is not null;
    }
}