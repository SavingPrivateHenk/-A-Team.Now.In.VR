using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ProductTooltip : MonoBehaviour
{
    private GameObject m_tooltip;
    private TextMeshProUGUI m_tooltipText;
    private Button m_tooltipButton;
    private ShoppingBasketManager m_shoppingBasketManager;
    private GameObject m_selectedGameObject;
    private Collider m_selectedObjectcollider;

    private void Awake()
    {
        m_tooltip = Instantiate(Resources.Load<GameObject>("Prefaps/Basket/Product Tooltip"));
        m_tooltipText = m_tooltip.GetComponentInChildren<TextMeshProUGUI>();
        m_tooltipButton = m_tooltip.GetComponentInChildren<Button>();
        m_tooltipButton.onClick.AddListener(OnBasketButtonClick);
        m_shoppingBasketManager = GetComponent<ShoppingBasketManager>();
    }

    private void Update()
    {
        m_tooltip.transform.LookAt(2 * m_tooltip.transform.position - Camera.main.transform.position);
    }

    public void OnProductSelected(SelectEnterEventArgs args)
    {
        var interactableGameObject = InteractableMonoBehaviour(args.interactableObject).gameObject;
        if (interactableGameObject.Equals(m_selectedGameObject)) return;
        if (interactableGameObject.TryGetComponent<ProductBehaviour>(out var product))
        {
            m_selectedGameObject = interactableGameObject;
            m_selectedObjectcollider = m_selectedGameObject.GetComponent<Collider>();
            m_tooltipText.text = product.ProductName;
            m_tooltip.transform.SetParent(interactableGameObject.transform, false);
            m_tooltip.transform.position = m_selectedObjectcollider.bounds.center + new Vector3(0f, m_selectedObjectcollider.bounds.extents.y + 0.2f, 0f);
            m_tooltip.SetActive(true);
        }
    }

    private void OnBasketButtonClick()
    {
        var productBehaviour = m_selectedGameObject.GetComponent<ProductBehaviour>();
        m_shoppingBasketManager.UpsertProduct(new Product(productBehaviour.ProductName, productBehaviour.ProductPrice, productBehaviour.PrefabName, productBehaviour.MaterialName), 1);
        m_tooltip.SetActive(false);
        m_selectedGameObject = null;
    }

    private MonoBehaviour InteractableMonoBehaviour(IXRInteractable xrInteractable) => xrInteractable as MonoBehaviour;
}
