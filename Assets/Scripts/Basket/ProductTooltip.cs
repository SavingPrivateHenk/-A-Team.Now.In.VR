using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ProductTooltip : MonoBehaviour
{
    private GameObject m_tooltip;

    private TextMeshProUGUI m_productName;
    private TextMeshProUGUI m_productPrice;
    private TextMeshProUGUI m_productQuantity;

    private Button m_basketButton;
    private Button m_incrementButton;
    private Button m_decrementButton;

    private ShoppingBasketManager m_shoppingBasketManager;
    private GameObject m_selectedGameObject;
    private Collider m_selectedObjectcollider;

    private int m_tooltipQuantity;

    private void Awake()
    {
        m_tooltip = Instantiate(Resources.Load<GameObject>("Prefaps/Basket/Product Tooltip"));

        m_productName = m_tooltip.GetNamedChild("Productname").GetComponent<TextMeshProUGUI>();
        m_productPrice = m_tooltip.GetNamedChild("Product Price").GetComponent<TextMeshProUGUI>();
        m_productQuantity = m_tooltip.GetNamedChild("Product Quantity").GetComponent<TextMeshProUGUI>();

        m_basketButton = m_tooltip.GetNamedChild("Basket Button").GetComponent<Button>();
        m_incrementButton = m_tooltip.GetNamedChild("Increment Button").GetComponent<Button>();
        m_decrementButton = m_tooltip.GetNamedChild("Decrement Button").GetComponent<Button>();

        m_basketButton.onClick.AddListener(OnBasketButtonClick);
        m_incrementButton.onClick.AddListener(() => UpdateQuantity(1));
        m_decrementButton.onClick.AddListener(() => UpdateQuantity(-1));

        m_shoppingBasketManager = GetComponent<ShoppingBasketManager>();
    }

    private void LateUpdate()
    {
        m_tooltip.transform.LookAt(2 * m_tooltip.transform.position - Camera.main.transform.position + Camera.main.transform.rotation * Vector3.forward);
    }

    public void OnProductSelected(SelectEnterEventArgs args)
    {
        var interactableGameObject = InteractableMonoBehaviour(args.interactableObject).gameObject;
        if (interactableGameObject.Equals(m_selectedGameObject)) return;
        if (interactableGameObject.TryGetComponent<ProductBehaviour>(out var product))
        {
            m_selectedGameObject = interactableGameObject;
            m_selectedObjectcollider = m_selectedGameObject.GetComponent<Collider>();
            m_productName.text = product.ProductName;
            m_productPrice.text = product.ProductPrice.ToString("C", new CultureInfo("nl-NL"));
            m_tooltipQuantity = 1;
            m_productQuantity.text = m_tooltipQuantity.ToString();
            m_tooltip.transform.position = m_selectedObjectcollider.bounds.center + new Vector3(0f, m_selectedObjectcollider.bounds.extents.y + 0.3f, 0f) - Camera.main.transform.forward;
            m_tooltip.SetActive(true);
        }
    }

    private void OnBasketButtonClick()
    {
        var productBehaviour = m_selectedGameObject.GetComponent<ProductBehaviour>();
        m_shoppingBasketManager.UpsertProduct(new Product(productBehaviour.ProductName, productBehaviour.ProductPrice, productBehaviour.PrefabName, productBehaviour.MaterialName), m_tooltipQuantity);
        m_tooltip.SetActive(false);
        m_selectedGameObject = null;
    }

    private void UpdateQuantity(int change)
    {
        m_tooltipQuantity = Math.Max(m_tooltipQuantity + change, 1);
        m_productQuantity.text = m_tooltipQuantity.ToString();
    }

    private MonoBehaviour InteractableMonoBehaviour(IXRInteractable xrInteractable) => xrInteractable as MonoBehaviour;
}