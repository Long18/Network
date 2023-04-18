using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISettingsTabsFiller : MonoBehaviour
{
    [SerializeField] private UISettingsTabFiller[] settingTabsList = default;

    public UnityAction<SettingsType> ChooseTab;

    public void FillTabs(List<SettingsType> settingTabs)
    {
        for (int i = 0; i < settingTabs.Count; i++)
        {
            settingTabsList[i].SetTab(settingTabs[i], i == 0);
            settingTabsList[i].Clicked += ChangeTab;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < settingTabsList.Length; i++)
        {
            settingTabsList[i].Clicked -= ChangeTab;
        }
    }

    public void SelectTab(SettingsType tabType)
    {
        for (int i = 0; i < settingTabsList.Length; i++)
        {
            settingTabsList[i].SetTab(tabType);
        }
    }

    public void ChangeTab(SettingsType tabType) => ChooseTab.Invoke(tabType);
}