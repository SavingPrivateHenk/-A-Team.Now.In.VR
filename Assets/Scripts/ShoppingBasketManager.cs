using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShoppingBasketManager : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<string, (float price, int quantity, string prefab, string material)> Items { get; set; } = new();

    [SerializeField]
    private InputActionReference _inputActionReference;

    [Space]
    [SerializeField]
    private GameObject _shoppingBasketUI;
    private ShoppingBasketUIController _shoppingBasketUIController;

    private void Awake()
    {
        _inputActionReference.action.performed += ToggleUIState;
        _shoppingBasketUIController = _shoppingBasketUI.GetComponent<ShoppingBasketUIController>();
        
    }

    private void Start()
    {
        Items = DataPersistence.Instance.BasketItems;
        
        AddItem("Chair A", 1, 3, "Chair A", "Solid/Green");

        foreach (var item in Items)
        {
            var val = item.Value;
            _shoppingBasketUIController.AddItem(
                item.Key, (val.price * val.quantity).ToString(), val.quantity.ToString());
        }
        UpdateShoppingBasketTotal();
    }

    private void ToggleUIState(InputAction.CallbackContext callbackContext)
    {
        _shoppingBasketUI.SetActive(!_shoppingBasketUI.activeSelf);
    }

    public Item GetFromBasket()
    {
        if (Items.Count == 0)
        {
            return null;
        }
        (float, int, string, string) item = Items.First().Value;
        Item product = new Item();
        product.ProductName = Items.First().Key;
        product.ProductPrice = item.Item1;
        product.PrefabName = item.Item3;
        product.MaterialName = item.Item4;
        return product;
    }

    public void AddToBasket(Product product, int quantity)
    {
        if (Items.ContainsKey(product.ProductName))
        {
            UpdateItem(product.ProductName, quantity);
        }
        else
        {
            AddItem(product.ProductName, product.ProductPrice, quantity, product.PrefabName, product.MaterialName);
        }
    }
    public void RemoveFromBasket(Product product) => RemoveItem(product.ProductName);

    public void RemoveOneFromBasket(Item product)
    {
        if (!Items.ContainsKey(product.ProductName))
        {
            return;
        }

        int quantity = Items[product.ProductName].quantity;
        
        if(quantity == 1)
        {
            RemoveItem(product.ProductName);
            return;
        }
        UpdateItem(product.ProductName, -1);
    }


    private void AddItem(string name, float price, int quantity, string prefab, string material)
    {
        Items.Add(name, (price, quantity, prefab, material));
        _shoppingBasketUIController.AddItem(
            name, price.ToString(), quantity.ToString());
        UpdateShoppingBasketTotal();
    }

    private void UpdateItem(string name, int quantity)
    {
        var item = Items[name];
        item = Items[name] = (item.price, item.quantity + quantity, item.prefab, item.material);
        _shoppingBasketUIController.UpdateItem(
            name, (item.quantity * item.price).ToString(), item.quantity.ToString());
        UpdateShoppingBasketTotal();
    }

    private void RemoveItem(string name)
    {
        Items.Remove(name);
        _shoppingBasketUIController.RemoveItem(name);
        UpdateShoppingBasketTotal();
    }

    private void UpdateShoppingBasketTotal()
    {
        float total = 0f;
        foreach (var item in Items)
        {
            total += item.Value.price * item.Value.quantity;
        }
        _shoppingBasketUIController.UpdateTotal(total.ToString());
    }

    private void OnDestroy()
    {
        _inputActionReference.action.performed -= ToggleUIState;
    }
}