using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ProductTooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject _tooltipPrefab;
    private TextMeshProUGUI _tooltipText;
    private Button _tooltipButton;

    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private ShoppingBasketManager _shoppingBasketManager;

    private GameObject _selectedGameObject;
    private Collider _selectedObjectcollider;

    private void Awake()
    {
        _tooltipPrefab = Instantiate(_tooltipPrefab);
        _tooltipText = _tooltipPrefab.GetComponentInChildren<TextMeshProUGUI>();
        _tooltipButton = _tooltipPrefab.GetComponentInChildren<Button>();
        _tooltipButton.onClick.AddListener(OnBasketButtonClick);
    }

    private void Update()
    {
        _tooltipPrefab.transform.LookAt(2 * _tooltipPrefab.transform.position - _mainCamera.transform.position);
    }

    public void OnProductSelected(SelectEnterEventArgs args)
    {
        var interactableGameObject = InteractableMonoBehaviour(args.interactableObject).gameObject;
        if (interactableGameObject.Equals(_selectedGameObject)) return;
        if (interactableGameObject.TryGetComponent<ProductBehaviour>(out var product))
        {
            _selectedGameObject = interactableGameObject;
            _selectedObjectcollider = _selectedGameObject.GetComponent<Collider>();
            _tooltipText.text = product.ProductName;
            _tooltipPrefab.transform.SetParent(interactableGameObject.transform, false);
            _tooltipPrefab.transform.position = _selectedObjectcollider.bounds.center + new Vector3(0f, _selectedObjectcollider.bounds.extents.y + 0.2f, 0f);
            _tooltipPrefab.SetActive(true);
        }
    }

    private void OnBasketButtonClick()
    {
        var productBehaviour = _selectedGameObject.GetComponent<ProductBehaviour>();
        _shoppingBasketManager.UpsertProduct(new Product(productBehaviour.ProductName, productBehaviour.ProductPrice, productBehaviour.PrefabName, productBehaviour.MaterialName), 1);
        _tooltipPrefab.SetActive(false);
        _selectedGameObject = null;
    }

    private MonoBehaviour InteractableMonoBehaviour(IXRInteractable xrInteractable) => xrInteractable as MonoBehaviour;
}
