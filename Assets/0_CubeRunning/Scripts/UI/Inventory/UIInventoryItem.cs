using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemCount = default;
    [SerializeField] private Image itemPreviewImage = default;
    [SerializeField] private Image bgImage = default;
    [SerializeField] private Image imgHover = default;
    [SerializeField] private Image imgSelected = default;
    [SerializeField] private Image bgInactiveImage = default;
    [SerializeField] private Button itemButton = default;
    [SerializeField] private LocalizeSpriteEvent bgLocalizedImage = default;

    public UnityAction<ItemSO> ItemSelected;

    [HideInInspector] public ItemStack currentItem;

    private bool _isSelected = false;

    private void OnEnable()
    {
        if (_isSelected) SelectItem();
    }

    public void SetItem(ItemStack itemStack, bool isSelected)
    {
        _isSelected = isSelected;
        itemPreviewImage.gameObject.SetActive(true);
        itemCount.gameObject.SetActive(true);
        bgImage.gameObject.SetActive(true);
        imgHover.gameObject.SetActive(true);
        imgSelected.gameObject.SetActive(true);
        itemButton.gameObject.SetActive(true);
        bgInactiveImage.gameObject.SetActive(false);

        UnHoverItem();
        currentItem = itemStack;

        imgSelected.gameObject.SetActive(isSelected);

        if (itemStack.Item.IsLocalized)
        {
            bgLocalizedImage.enabled = true;
            bgLocalizedImage.AssetReference = itemStack.Item.LocalizePreviewImage;
        }
        else
        {
            bgLocalizedImage.enabled = false;
            itemPreviewImage.sprite = itemStack.Item.PreviewImage;
        }

        itemCount.text = itemStack.Amount.ToString();
        bgImage.color = itemStack.Item.ItemType.TypeColor;
    }

    public void SetInactiveItem()
    {
        UnHoverItem();
        currentItem = null;
        itemPreviewImage.gameObject.SetActive(false);
        itemCount.gameObject.SetActive(false);
        bgImage.gameObject.SetActive(false);
        imgHover.gameObject.SetActive(false);
        imgSelected.gameObject.SetActive(false);
        itemButton.gameObject.SetActive(false);
        bgInactiveImage.gameObject.SetActive(true);
    }

    public void SelectFirstElement()
    {
        _isSelected = true;
        itemButton.Select();
        SelectItem();
    }

    public void HoverItem() => imgHover.gameObject.SetActive(true);

    public void UnHoverItem() => imgHover.gameObject.SetActive(false);

    public void SelectItem()
    {
        _isSelected = true;

        if (ItemSelected != null && currentItem != null && currentItem.Item != null)
        {
            imgSelected.gameObject.SetActive(true);
            ItemSelected.Invoke(currentItem.Item);
        }
        else
        {
            imgSelected.gameObject.SetActive(false);
        }
    }

    public void UnSelectItem()
    {
        _isSelected = false;
        imgSelected.gameObject.SetActive(false);
    }
}