using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_nameField;
    [SerializeField]
    private TextMeshProUGUI m_totalField;
    [SerializeField]
    private TextMeshProUGUI m_quantityField;
    [SerializeField]
    private Button m_removeButton;

    [HideInInspector]
    public Product Product { get; private set; }
    [HideInInspector]
    public ShoppingBasketManager Manager { get; private set; }

    private void Awake()
    {
        m_removeButton.onClick.AddListener(() => Manager.RemoveProduct(Product));
    }

    public static ProductElement Create(ref Product product, ShoppingBasketManager manager, Transform parent)
    {
        var prefab = Resources.Load<GameObject>("Prefaps/Basket/Product Element");
        var instance = Instantiate(prefab, parent).GetComponent<ProductElement>();
        instance.Product = product;
        instance.Manager = manager;
        instance.Initialize();
        return instance;
    }

    private void Initialize()
    {
        m_nameField.text = Product.Name;
    }

    public void UpdateQuantity(int quantity)
    {
        var total = Product.Price * quantity;
        m_totalField.text = total.ToString("C", new CultureInfo("nl-NL"));
        m_quantityField.text = quantity.ToString();
    }
}