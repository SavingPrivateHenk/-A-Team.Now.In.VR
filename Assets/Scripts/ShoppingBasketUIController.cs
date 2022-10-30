using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingBasketUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _shoppingBasketUIContent;
    [SerializeField]
    private GameObject _shoppingBasketUIItemPrefab;
    [SerializeField]
    private TextMeshProUGUI _shoppingBasketUITotal;

    public void AddItem(string name, string price, string quantity)
    {
        var item = Instantiate(
            _shoppingBasketUIItemPrefab,
            _shoppingBasketUIContent.transform)
            .GetComponent<ShoppingBasketUIItem>();

        item.Label = name;
        item.Price = $"€{price}";
        item.Quantity = quantity;
    }

    public void UpdateItem(string name, string price, string quantity)
    {
        foreach (Transform transform in _shoppingBasketUIContent.transform)
        {
            var item = transform.gameObject.GetComponent<ShoppingBasketUIItem>();
            if (item.Label.Equals(name))
            {
                item.Quantity = quantity;
                item.Price = $"€{price}";
            }
        }
    }

    public void RemoveItem(string name)
    {
        foreach (Transform transform in _shoppingBasketUIContent.transform)
        {
            var item = transform.gameObject.GetComponent<ShoppingBasketUIItem>();
            if (item.Label.Equals(name))
            {
                Destroy(transform.gameObject);
            }
        }
    }

    public void UpdateTotal(string total)
    {
        _shoppingBasketUITotal.text = $"Totaal: €{total}";
    }
}