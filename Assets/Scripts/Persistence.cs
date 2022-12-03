using System.Collections.Generic;
using UnityEngine;

public class Persistence : MonoBehaviour
{
    public static Persistence Instance;

    [HideInInspector]
    public Dictionary<int, (Vector3 position, Quaternion rotation)> LocationInScenes = new();
    public Dictionary<Product, int> Products = new();

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
