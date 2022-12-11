using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Persistence : MonoBehaviour
{
    public static Persistence Instance;

    [HideInInspector]
    public Dictionary<int, (Vector3 position, Quaternion rotation)> LocationInScenes = new();
    public Dictionary<Product, int> Products = new();
    public bool hasTeleportation;
    public bool hasSnapTurn;

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

    public void MergeProducts(Dictionary<Product, int> addition)
    {
        if (Products.Count == 0)
        {
            Products = addition;
            return;
        }
        foreach (var product in addition)
        {
            Products.TryGetValue(product.Key, out var quantity);
            Products[product.Key] = quantity + product.Value;
        }
    }
}