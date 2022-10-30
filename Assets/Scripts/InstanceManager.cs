using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    [SerializeField] public string reference;
    [SerializeField] public Material prefabColor;

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.C))
        {
            var prefab = Resources.Load<GameObject>("Prefaps/Interactable/" + reference);
            GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            
            instance.transform.position = Vector3.one;
            
            ColorChanger.FindMaterials(instance.transform, prefabColor);
        }
    }
}
