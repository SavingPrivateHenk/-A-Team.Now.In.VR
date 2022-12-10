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

    private readonly Dictionary<Product, ProductElement> Elements = new();

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
        if (Elements.TryGetValue(product, out var element) &&
            Products.TryGetValue(product, out var quantity))
        {
            Destroy(element.gameObject);
            UpdateTotal(quantity * product.Price * -1);
            return Elements.Remove(product) && Products.Remove(product);
        }
        return false;
    }

    private bool InsertProduct(Product product, int quantity)
    {
        var element = ProductElement.Create(ref product, this, m_productElementContainer.transform);
        element.UpdateQuantity(quantity);
        Products.Add(product, quantity);
        Elements.Add(product, element);
        UpdateTotal(product.Price * quantity);
        return true;
    }

    private bool UpdateProduct(Product product, int change)
    {
        if (Elements.TryGetValue(product, out var element) &&
            Products.TryGetValue(product, out var quantity))
        {
            var updatedQuantity = quantity + change;
            if (updatedQuantity < 1) return RemoveProduct(product);
            Products[product] = updatedQuantity;
            element.UpdateQuantity(quantity);
            UpdateTotal(product.Price * change);
            return true;
        }
        return false;
    }

    private void UpdateTotal(float change)
    {
        TotalPrice += change;
        m_shoppingBasketTotal.text = TotalPrice.ToString("C", new CultureInfo("nl-NL"));
    }

    private void ToggleUI(InputAction.CallbackContext context) => m_shoppingBasket.SetActive(!m_shoppingBasket.activeSelf);

    private void OnDestroy()
    {
        Persistence.Instance.Products = Products;
        ToggleUIAction.action.performed -= ToggleUI;
    }
}