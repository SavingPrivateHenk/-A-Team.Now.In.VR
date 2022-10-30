using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class InstanceManager : MonoBehaviour
{
    [SerializeField] public string reference;
    [SerializeField] public Material prefabColor;

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.C))
        {
            var prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefaps/Interactable/" + reference + ".prefab", typeof(GameObject));
            GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            
            instance.transform.position = Vector3.one;
            
            ColorChanger.FindMaterials(instance.transform, prefabColor);
        }
    }
}
