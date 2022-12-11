using UnityEngine;

public static class ComponentExtensions
{
    public static T AddComponentAsChild<T>(this Component @this, string name) where T : Component
    {
        return @this.gameObject.AddComponentAsChild<T>(name);
    }

    public static T GetComponentOfNamedChild<T>(this Component @this, string name) where T : Component
    {
        return @this.gameObject.GetComponentOfNamedChild<T>(name);
    }
}