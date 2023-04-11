using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UISettingsTabFiller : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent localizedTabTitle = default;
    [SerializeField] private Image backgroundSelectedTab = default;
    [SerializeField] private Color colorSelectedTab = default;
    [SerializeField] private Color colorUnselectedTab = default;

    private SettingsType currentTabType;

    public UnityAction<SettingsType> Clicked = default;

    public void SetTab(SettingsType settingTab, bool isSelected)
    {
        localizedTabTitle.StringReference.TableEntryReference = settingTab.ToString();
        currentTabType = settingTab;
        if (isSelected)
            SelectTab();
        else
            UnselectTab();
    }

    public void SetTab(SettingsType tabType)
    {
        bool isSelected = (currentTabType == tabType);
        if (isSelected)
            SelectTab();
        else
            UnselectTab();
    }

    private void SelectTab()
    {
        backgroundSelectedTab.enabled = true;
        localizedTabTitle.GetComponent<TextMeshProUGUI>().color = colorSelectedTab;
    }

    private void UnselectTab()
    {
        backgroundSelectedTab.enabled = false;
        localizedTabTitle.GetComponent<TextMeshProUGUI>().color = colorUnselectedTab;
    }

    public void Click() => Clicked.Invoke(currentTabType);
}