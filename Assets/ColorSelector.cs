using System;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
    [SerializeField] private Material material;

    [ContextMenu("FindMaterials")]
    private void Awake()
    {
        FindMaterials(gameObject.transform, 0);
    }
    
    private void FindMaterials(Transform entity, int iteration)
    {
        for (var i = 0; i < entity.childCount; i++)
        {
            var child = entity.GetChild(i);
            if (child.GetComponent<MeshRenderer>() == null)
            {
                FindMaterials(child, iteration + 1);
                continue;
            }
            
            child.GetComponent<MeshRenderer>().material = material;
        }
    }
}
