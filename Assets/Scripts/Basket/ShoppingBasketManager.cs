using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShoppingBasketManager : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<Product, int> Products { get; private set; } = new();
    [HideInInspector]
    public float TotalPrice;

    public InputActionReference ToggleUIAction;
    public GameObject ShoppingBasketAnchor;

    [SerializeField]
    private GameObject _shoppingBasketPrefab;
    private GameObject _shoppingBasket;
    private TextMeshProUGUI _shoppingBasketTotal;

    [SerializeField]
    private GameObject _productElementPrefab;
    private GameObject _productElementContainer;

    private void Awake()
    {
        ToggleUIAction.action.performed += ToggleUI;
        _shoppingBasket = Instantiate(_shoppingBasketPrefab, ShoppingBasketAnchor.transform);
        _productElementContainer = _shoppingBasket.GetNamedChild("Basket Container");
        _shoppingBasketTotal = _shoppingBasket.GetNamedChild("Basket Total").GetComponent<TextMeshProUGUI>();
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
        var instance = Instantiate(_productElementPrefab, _productElementContainer.transform);
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

    private void UpdateTotalField(float change) => _shoppingBasketTotal.text = "Totaal: " + (TotalPrice += change).ToString("C", new CultureInfo("nl-NL"));

    private void ToggleUI(InputAction.CallbackContext context) => _shoppingBasket.SetActive(!_shoppingBasket.activeSelf);

    private bool TryFindElement(string name, out ProductElement element) => _productElementContainer.TryFindComponentInChildren(true, element => element.Name.Equals(name), out element);

    private void OnDestroy()
    {
        Persistence.Instance.Products = Products;
        ToggleUIAction.action.performed -= ToggleUI;
    }
}