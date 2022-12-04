using System.Globalization;
using TMPro;
using UnityEngine;

public class ProductElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_nameField;
    [SerializeField]
    private TextMeshProUGUI m_totalField;
    [SerializeField]
    private TextMeshProUGUI m_quantityField;

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
        m_nameField.text = Name;
        m_totalField.text = Total.ToString("C", new CultureInfo("nl-NL"));
        m_quantityField.text = Quantity.ToString();
    }
}