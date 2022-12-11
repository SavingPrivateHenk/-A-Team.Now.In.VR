using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(XRSimpleInteractable))]
public class ProductBehaviour : MonoBehaviour
{
    public string ProductName;
    public float ProductPrice;
    public string PrefabName;
    public string MaterialName;

    public Product ToValueType() => new(ProductName, ProductPrice, PrefabName, MaterialName);
}