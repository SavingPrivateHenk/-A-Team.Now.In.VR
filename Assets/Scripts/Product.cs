using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(XRSimpleInteractable))]
public class Product : MonoBehaviour
{
    public string ProductName;
    public float  ProductPrice;
    public string PrefabName;
    public string MaterialName;
}
