using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShoppingBasketManager : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<Product, int> Products { get; private set; } = new();
    [HideInInspector]
    public float TotalPrice { get; set; }

    public InputActionReference ToggleUIAction;
    public GameObject ShoppingBasketAnchor;

    private GameObject m_shoppingBasket;
    private GameObject m_productElementContainer;
    private GameObject m_shoppingBasketBetalen;
    
    private TextMeshProUGUI m_shoppingBasketTotal;

    private bool m_isPersonalRoomScene;

    private void Awake()
    {
        ToggleUIAction.action.performed += ToggleUI;
        m_shoppingBasket = Instantiate(Resources.Load<GameObject>("Prefaps/Basket/Shopping Basket"), ShoppingBasketAnchor.transform);
        m_productElementContainer = m_shoppingBasket.GetNamedChild("Basket Container");
        m_shoppingBasketTotal = m_shoppingBasket.GetNamedChild("Basket Total").GetComponent<TextMeshProUGUI>();
        m_shoppingBasketBetalen = m_shoppingBasket.GetNamedChild("Betalen Button");
        m_isPersonalRoomScene = SceneManager.GetActiveScene().name.Equals("Personal Room Scene", StringComparison.OrdinalIgnoreCase);
        m_shoppingBasketBetalen.SetActive(!m_isPersonalRoomScene);
    }

    private void Start()
    {
        foreach (var product in Persistence.Instance.Products)
        {
            InsertProduct(product.Key, product.Value);
        }
    }

    public bool UpsertProduct(Product product, int quantity)
    {
        return Products.ContainsKey(product)
            ? UpdateProduct(product, quantity)
            : InsertProduct(product, quantity);
    }

    public bool RemoveProduct(Product product)
    {
        if (!Products.ContainsKey(product)) return false;

        if (TryFindElement(product.Name, out var element) &&
            Products.TryGetValue(product, out var quantity) &&
            Products.Remove(product))
        {
            Destroy(element.gameObject);
            UpdateTotalField(quantity * product.Price * -1);
            return true;
        }
        return false;
    }

    private bool InsertProduct(Product product, int quantity)
    {
        var instance = Instantiate(Resources.Load<GameObject>("Prefaps/Basket/Product Element"), m_productElementContainer.transform);
        if (!instance.TryGetComponent<ProductElement>(out var element)) return false;
        Products.Add(product, quantity);
        element.UpdateProperties(product.Name, product.Price, quantity);
        UpdateTotalField(product.Price * quantity);
        return true;
    }

    private bool UpdateProduct(Product product, int change)
    {
        if (!TryFindElement(product.Name, out var element)) return false;
        var totalQuantity = Products[product] + change;
        if (totalQuantity <= 0) return RemoveProduct(product);
        Products[product] = totalQuantity;
        element.UpdateProperties(product.Name, product.Price, totalQuantity);
        UpdateTotalField(product.Price * change);
        return true;
    }

    private void UpdateTotalField(float change) => m_shoppingBasketTotal.text = (TotalPrice += change).ToString("C", new CultureInfo("nl-NL"));

    private void ToggleUI(InputAction.CallbackContext context) => m_shoppingBasket.SetActive(!m_shoppingBasket.activeSelf);

    private bool TryFindElement(string name, out ProductElement element) => m_productElementContainer.TryFindComponentInChildren(true, element => element.Name.Equals(name), out element);

    private void OnDestroy()
    {
        Persistence.Instance.Products = Products;
        ToggleUIAction.action.performed -= ToggleUI;
    }
}