using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class InstanceManager : MonoBehaviour
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public Material prefabColor;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.C))
        {
            GameObject instance = Instantiate(prefab);
            
            ColorChanger.FindMaterials(instance.transform, prefabColor);
        }
    }
}
