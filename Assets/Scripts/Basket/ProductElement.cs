using System.Globalization;
using TMPro;
using UnityEngine;

public class ProductElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameField;
    [SerializeField]
    private TextMeshProUGUI _totalField;
    [SerializeField]
    private TextMeshProUGUI _quantityField;

    [HideInInspector]
    public string Name { get; private set; }
    [HideInInspector]
    public float Price { get; private set; }
    [HideInInspector]
    public int Quantity { get; private set; }
    [HideInInspector]
    public float Total { get; private set; }

    public void UpdateProperties(string name, float price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        Total = price * quantity;
        UpdateFields();
    }

    private void UpdateFields()
    {
        _nameField.text = Name;
        _totalField.text = Total.ToString("C", new CultureInfo("nl-NL"));
        _quantityField.text = Quantity.ToString();
    }
}