using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InstanceManager : MonoBehaviour
{ 
    [SerializeField]
    private InputActionReference inputActionReference; 
    
    [SerializeField] 
    private GameManager gameManager;
    private BasketManager basket;
    private XROrigin xrOrigin;

    public Dictionary<Product, int> Products { get; private set; } = new();

    private void Start()
    {
        basket = gameManager.BasketManager;
        xrOrigin = gameManager.XROrigin;
        xrOrigin.GetComponent<TeleportationProvider>().enabled = Persistence.Instance.hasTeleportation;
        xrOrigin.GetComponent<ActionBasedContinuousMoveProvider>().enabled = !Persistence.Instance.hasTeleportation;
        xrOrigin.GetComponent<ActionBasedSnapTurnProvider>().enabled = Persistence.Instance.hasSnapTurn;
        xrOrigin.GetComponent<ActionBasedContinuousTurnProvider>().enabled = !Persistence.Instance.hasSnapTurn;

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
        Persistence.Instance.MergeProducts(Products);
        inputActionReference.action.performed -= AddObject;
    }
}
