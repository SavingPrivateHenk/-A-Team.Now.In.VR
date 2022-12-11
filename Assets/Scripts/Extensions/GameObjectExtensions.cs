using Unity.XR.CoreUtils;
using UnityEngine;

public static class GameObjectExtensions
{
    public static GameObject AddEmptyGameObject(this GameObject @this, string name)
    {
        var obj = new GameObject(name);
        obj.transform.parent = @this.transform;
        return obj;
    }

    public static T AddComponentAsChild<T>(this GameObject @this, string name) where T : Component
    {
        return @this.AddEmptyGameObject(name).AddComponent<T>();
    }

    public static T GetComponentOfNamedChild<T>(this GameObject @this, string name) where T : Component
    {
        return @this.GetNamedChild(name).GetComponent<T>();
    }
}