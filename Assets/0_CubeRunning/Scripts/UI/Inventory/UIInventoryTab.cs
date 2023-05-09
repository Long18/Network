using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryTab : MonoBehaviour
{
    public UnityAction<InventoryTabSO> TabClicked;

    [SerializeField] private Image tabImage = default;
    [SerializeField] private Button actionButton = default;
    [SerializeField] private Color selectedIconColor = default;
    [SerializeField] private Color deselectedIconColor = default;

    [ReadOnly] public InventoryTabSO currentTabType = default;

    public void SetTab(InventoryTabSO tabType, bool isSelected)
    {
        currentTabType = tabType;
        tabImage.sprite = tabType.TabIcon;

        UpdateState(isSelected);
    }

    public void UpdateState(bool isSelected)
    {
        actionButton.interactable = !isSelected;

        if (isSelected)
            tabImage.color = selectedIconColor;
        else
            tabImage.color = deselectedIconColor;
    }

    public void ClickButton()
    {
        TabClicked.Invoke(currentTabType);
    }
}