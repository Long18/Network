using System;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/MultiInputSelectableElement")]
public class MultiInputSelectableElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    private MenuSelectionHandler menuSelectionHandler = default;

    private void Awake() =>
        menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();

    public void OnPointerEnter(PointerEventData eventData) => menuSelectionHandler.HandleMouseEnter(gameObject);

    public void OnPointerExit(PointerEventData eventData) => menuSelectionHandler.HandleMouseExit(gameObject);

    public void OnSelect(BaseEventData eventData) => menuSelectionHandler.UpdateSelection(gameObject);
}