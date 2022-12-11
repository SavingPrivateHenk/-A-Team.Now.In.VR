using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class BasketManager : MonoBehaviour
{
    public ActionBasedController BasketAnchor { get; set; }
    public InputActionReference ToggleBasket { get; set; }
    public Scene Scene { get; set; }
    public float Total { get; private set; }
    public Dictionary<Product, int> Products { get; private set; } = new();
    public Dictionary<Product, ProductElement> Elements { get; private set; } = new();

    private GameObject m_basket;
    private GameObject m_basketContainer;
    private TextMeshProUGUI m_basketTotal;

    private void Start()
    {
        ToggleBasket.action.performed += ToggleBasketUI;
        m_basket = Instantiate(Resources.Load<GameObject>("Prefaps/Basket/Shopping Basket"), BasketAnchor.transform);
        m_basketContainer = m_basket.GetNamedChild("Basket Container");
        m_basketTotal = m_basket.GetComponentOfNamedChild<TextMeshProUGUI>("Basket Total");
        m_basket.GetNamedChild("Betalen Button").SetActive(!Scene.name.Equals("Personal Room Scene", StringComparison.OrdinalIgnoreCase));

        foreach (var product in Persistence.Instance.Products)
        {
            InsertProduct(product.Key, product.Value);
        }
        Persistence.Instance.Products.Clear();
    }

    public void UpsertProduct(Product product, int change)
    {
        switch (Products.TryGetValue(product, out var current),
            current + change is var updated && updated > 0)
        {
            case (true, true): UpdateProduct(product, updated, change); break;
            case (true, false): RemoveProduct(product); break;
            default: InsertProduct(product, change); break;
        }
    }

    public void RemoveProduct(Product product)
    {
        if (Elements.TryGetValue(product, out var element))
        {
            Elements.Remove(product);
            Destroy(element.gameObject);
        }
        if (Products.TryGetValue(product, out var quantity))
        {
            Products.Remove(product);
            UpdateTotal(quantity * product.Price * -1);
        }
    }

    private void InsertProduct(Product product, int quantity)
    {
        var element = ProductElement.Create(ref product, this, m_basketContainer.transform);
        Products[product] = quantity;
        Elements[product] = element;
        element.UpdateQuantity(quantity);
        UpdateTotal(product.Price * quantity);
    }

    private void UpdateProduct(Product product, int quantity, int difference)
    {
        Products[product] = quantity;
        Elements[product].UpdateQuantity(quantity);
        UpdateTotal(difference * product.Price);
    }

    private void UpdateTotal(float difference)
    {
        Total += difference;
        m_basketTotal.text = Total.ToString("C", new CultureInfo("nl-NL"));
    }

    private void ToggleBasketUI(InputAction.CallbackContext context) => m_basket.SetActive(!m_basket.activeSelf);

    private void OnDestroy()
    {
        Persistence.Instance.MergeProducts(Products);
        ToggleBasket.action.performed -= ToggleBasketUI;
    }
}