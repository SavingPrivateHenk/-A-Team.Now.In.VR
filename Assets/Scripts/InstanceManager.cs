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
        (float, int, string, string) item = basket.Items.First().Value;

        var material = Resources.Load<Material>("Materials/" + item.Item4);
        var prefab = Resources.Load<GameObject>("Prefaps/Interactable/" + item.Item3);
        GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        
        instance.transform.position = Vector3.one;
        
        ColorChanger.FindMaterials(instance.transform, material);
    }
}
