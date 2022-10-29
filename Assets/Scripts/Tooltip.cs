using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject _tooltipPrefab;
    private TextMeshProUGUI _tooltipText;
    private Button _tooltipButton;

    [SerializeField]
    private Basket _basket;

    private GameObject _selectedProduct;

    private void Awake()
    {
        //_tooltipPrefab = Instantiate(_tooltipPrefab);
        _tooltipText = _tooltipPrefab.GetComponentInChildren<TextMeshProUGUI>();
        _tooltipButton = _tooltipPrefab.GetComponentInChildren<Button>();
        _tooltipButton.onClick.AddListener(() => Debug.Log("toevoegen aan mandje"));
    }

    public void OnProductSelected(SelectEnterEventArgs args)
    {
        var interactableGameObject = InteractableMonoBehaviour(args.interactableObject).gameObject;
        if (interactableGameObject.Equals(_selectedProduct)) return;
        if (interactableGameObject.TryGetComponent<Product>(out var product))
        {
            _selectedProduct = interactableGameObject;
            _tooltipText.text = product.ProductName;
            _tooltipPrefab.SetActive(true);
        }
    }

    public void OnProductDeselected(SelectExitEventArgs args)
    {

        _tooltipPrefab.SetActive(false);
        _selectedProduct = null;
    }

    private MonoBehaviour InteractableMonoBehaviour(IXRInteractable xrInteractable) => xrInteractable as MonoBehaviour;
}