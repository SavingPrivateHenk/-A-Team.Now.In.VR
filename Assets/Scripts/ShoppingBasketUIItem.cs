using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingBasketUIItem : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _productLabel;
    [SerializeField]
    private TextMeshProUGUI _productQuantity;
    [SerializeField]
    private TextMeshProUGUI _productPrice;

    [HideInInspector]
    public string Label { get { return _productLabel.text; } set { _productLabel.text = value; } }
    [HideInInspector]
    public string Quantity { get { return _productQuantity.text; } set { _productQuantity.text = value; } }
    [HideInInspector]
    public string Price { get { return _productPrice.text; } set { _productPrice.text = value; } }
}