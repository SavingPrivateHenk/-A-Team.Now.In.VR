using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class InstanceManager : MonoBehaviour
{ 
    [SerializeField]
    private InputActionReference inputActionReference; 
    [SerializeField]
    private ShoppingBasketManager basket;

    [HideInInspector]
    public Dictionary<Product, int> Products { get; private set; } = new();


    private void Awake()
    {
        inputActionReference.action.performed += AddObject;
    }

    void AddObject(InputAction.CallbackContext callbackContext)
    {
        var product = basket.Products.Keys.FirstOrDefault();
        if (product.Equals(default)) return;

        if(Products.ContainsKey(product))
        {
            Products[product] += 1;
        }
        else
        {
            Products.Add(product, 1);
        }

        var material = Resources.Load<Material>("Materials/" + product.Material);
        var prefab = Resources.Load<GameObject>("Prefaps/Interactable/" + product.Prefab);
        GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        
        instance.transform.position = new Vector3(Random.Range(0, 10), 1, Random.Range(0, 10));
        
        ColorChanger.FindMaterials(instance.transform, material);

        basket.UpsertProduct(product, -1);
    }

    private void OnDestroy()
    {
        foreach (var product in Products)
        {
            Persistence.Instance.Products.Add(product.Key, product.Value);
        }
        inputActionReference.action.performed -= AddObject;
    }
}
