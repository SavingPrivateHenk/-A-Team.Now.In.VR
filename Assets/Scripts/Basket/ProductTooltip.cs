using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ProductTooltip : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;

    private Camera m_camera;
    private BasketManager m_basketManager;
    private SoundManager m_soundManager;
    private GameObject m_tooltip;
    private GameObject m_selected;
    private AudioClip m_audioClip;
    private TextMeshProUGUI m_productName;
    private TextMeshProUGUI m_productPrice;
    private TextMeshProUGUI m_productQuantity;

    private int m_tooltipQuantity;

    private void Awake()
    {
        m_audioClip = Resources.Load<AudioClip>("Audio/mixkit-unlock-game-notification-253");
        m_tooltip = Instantiate(Resources.Load<GameObject>("Prefaps/Basket/Product Tooltip"));
        m_tooltip.GetComponentOfNamedChild<Button>("Basket Button").onClick.AddListener(OnBasketButtonClick);
        m_tooltip.GetComponentOfNamedChild<Button>("Increment Button").onClick.AddListener(() => UpdateQuantity(1));
        m_tooltip.GetComponentOfNamedChild<Button>("Decrement Button").onClick.AddListener(() => UpdateQuantity(-1));
        m_productName = m_tooltip.GetComponentOfNamedChild<TextMeshProUGUI>("Productname");
        m_productPrice = m_tooltip.GetComponentOfNamedChild<TextMeshProUGUI>("Product Price");
        m_productQuantity = m_tooltip.GetComponentOfNamedChild<TextMeshProUGUI>("Product Quantity");
    }

    private void Start()
    {
        m_camera = GameManager.Camera;
        m_basketManager = GameManager.BasketManager;
        m_soundManager = GameManager.SoundManager;
    }

    private void LateUpdate()
    {
        m_tooltip.transform.LookAt(2 * m_tooltip.transform.position - m_camera.transform.position + m_camera.transform.rotation * Vector3.forward);
    }

    public void OnProductSelected(SelectEnterEventArgs args)
    {
        var interactableGameObject = args.interactableObject.transform.gameObject;
        if (interactableGameObject.Equals(m_selected)) return;
        if (interactableGameObject.TryGetComponent<ProductBehaviour>(out var product))
        {
            m_selected = interactableGameObject;
            m_productName.text = product.ProductName;
            m_productPrice.text = product.ProductPrice.ToString("C", new CultureInfo("nl-NL"));
            m_tooltipQuantity = 1;
            m_productQuantity.text = m_tooltipQuantity.ToString();

            var bounds = m_selected.GetComponent<Collider>().bounds;
            m_tooltip.transform.position = bounds.center + new Vector3(0f, bounds.extents.y + 0.3f, 0f) - m_camera.transform.forward;
            m_tooltip.SetActive(true);
        }
    }

    private void OnBasketButtonClick()
    {
        var behaviour = m_selected.GetComponent<ProductBehaviour>();
        m_basketManager.UpsertProduct(behaviour.ToValueType(), m_tooltipQuantity);
        m_soundManager.PlayClip(m_audioClip);
        m_tooltip.SetActive(false);
        m_selected = null;
    }

    private void UpdateQuantity(int change)
    {
        m_tooltipQuantity = Math.Max(m_tooltipQuantity + change, 1);
        m_productQuantity.text = m_tooltipQuantity.ToString();
    }
}