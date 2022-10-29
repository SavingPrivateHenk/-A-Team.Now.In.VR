using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<string, int> BasketItems { get; private set; } = new Dictionary<string, int>();

    public void AddToBasket(string productName) => BasketItems[productName] += 1;

    public void AddQuantity(string productName, int quantity) => BasketItems[productName] += quantity;

    public void RemoveFromBasket(string productName) => BasketItems.Remove(productName);
}