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
        ColorChanger.FindMaterials(gameObject.transform, material);
    }
}
