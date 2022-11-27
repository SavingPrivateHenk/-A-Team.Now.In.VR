using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class InstanceManager : MonoBehaviour
{ 
    [SerializeField]
    private InputActionReference inputActionReference; 
    [SerializeField]
    private ShoppingBasketManager basket;

    private void Awake()
    {
        inputActionReference.action.performed += AddObject;
    }

    void AddObject(InputAction.CallbackContext callbackContext)
    {
        Item product = basket.GetFromBasket();
        if (product == null)
        {
            return;
        }
        var material = Resources.Load<Material>("Materials/" + product.MaterialName);
        var prefab = Resources.Load<GameObject>("Prefaps/Interactable/" + product.PrefabName);
        GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        
        instance.transform.position = new Vector3(Random.Range(0, 10), 1, Random.Range(0, 10));
        
        ColorChanger.FindMaterials(instance.transform, material);
        
        basket.RemoveOneFromBasket(product);
    }
}
