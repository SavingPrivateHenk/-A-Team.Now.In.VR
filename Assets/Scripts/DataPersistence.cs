using System.Collections.Generic;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    public static DataPersistence Instance;

    [HideInInspector]
    public Dictionary<int, (Vector3 position, Quaternion rotation)> XROriginTransform = new();
    public Dictionary<string, (float price, int quantity, string prefab, string material)> BasketItems = new();

    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        } 
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
