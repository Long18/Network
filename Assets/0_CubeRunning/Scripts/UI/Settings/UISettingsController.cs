using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Serialization;

[Serializable]
public enum SettingFieldType
{
    Language = 0,
    Volume_SFx = 1,
    Volume_Music = 2,
    Resolution = 3,
    FullScreen = 4,
    ShadowDistance = 5,
    AntiAliasing = 6,
    ShadowQuality = 7,
    Volume_Master = 8,
}

public enum SettingsType
{
    Language = 0,
    Graphics = 1,
    Audio = 2,
}

[Serializable]
public class SettingTab
{
    public SettingsType SettingsType;
    public LocalizedString title;
}

[Serializable]
public class SettingField
{
    public SettingsType SettingsType;
    public SettingFieldType settingFieldType;
    public LocalizedString title;
}

public class UISettingsController : MonoBehaviour
{
    [SerializeField, ReadOnly] private SettingsType currentSelectedTab = SettingsType.Language;

    [SerializeField] private UISettingsLanguageComponent languageComponent;
    [SerializeField] private UISettingsGraphicsComponent graphicComponent;
    [SerializeField] private UISettingsAudioComponent audioComponent;
    [SerializeField] private UISettingsTabsFiller settingTabFiller = default;
    [SerializeField] private SettingsSO currentSettings = default;
    [SerializeField] private List<SettingsType> settingTabsList = new();
    [SerializeField] private InputReaderSO inputReader = default;
    [SerializeField] private VoidEventChannelSO saveSettingsEvent = default;

    public UnityAction Closed;

    private void OnEnable()
    {
        languageComponent.Save += SaveLaguageSettings;
        audioComponent.Save += SaveAudioSettings;
        graphicComponent.Save += SaveGraphicsSettings;

        inputReader.MenuCloseEvent += CloseScreen;
        inputReader.TabSwitchedEvent += TabSwitched;

        settingTabFiller.FillTabs(settingTabsList);
        settingTabFiller.ChooseTab += OpenSetting;

        OpenSetting(SettingsType.Audio);
    }

    private void OnDisable()
    {
        inputReader.MenuCloseEvent -= CloseScreen;
        inputReader.TabSwitchedEvent -= TabSwitched;

        languageComponent.Save -= SaveLaguageSettings;
        audioComponent.Save -= SaveAudioSettings;
        graphicComponent.Save -= SaveGraphicsSettings;
    }

    public void CloseScreen() => Closed?.Invoke();

    private void OpenSetting(SettingsType settingsType)
    {
        currentSelectedTab = settingsType;

        switch (settingsType)
        {
            case SettingsType.Language:
                currentSettings.SaveLanguageSettings(currentSettings.CurrentLocale);
                break;
            case SettingsType.Graphics:
                graphicComponent.Setup();
                break;
            case SettingsType.Audio:
                audioComponent.Setup(currentSettings.MusicVolume, currentSettings.SfxVolume,
                    currentSettings.MasterVolume);
                break;
            default:
                break;
        }

        languageComponent.gameObject.SetActive(settingsType == SettingsType.Language);
        audioComponent.gameObject.SetActive(settingsType == SettingsType.Audio);
        graphicComponent.gameObject.SetActive(settingsType == SettingsType.Graphics);
        settingTabFiller.SelectTab(settingsType);
    }

    private void TabSwitched(float orientation)
    {
        if (orientation == 0) return;

        bool isLeft = orientation < 0;
        int index = settingTabsList.FindIndex(a => a == currentSelectedTab);

        if (index == -1) return;

        if (isLeft) index--;
        else index++;

        index = Mathf.Clamp(index, 0, settingTabsList.Count - 1);

        OpenSetting(settingTabsList[index]);
    }

    public void SaveLaguageSettings(Locale locale)
    {
        currentSettings.SaveLanguageSettings(locale);
        saveSettingsEvent.RaiseEvent();
    }

    public void SaveGraphicsSettings(int newResolutionsIndex, int newAntiasingIndex, float newShadowDistance,
        bool fullScreen)
    {
        currentSettings.SaveGraphicsSettings(newResolutionsIndex, newAntiasingIndex, newShadowDistance, fullScreen);
        saveSettingsEvent.RaiseEvent();
    }

    public void SaveAudioSettings(float musicVol, float sfxVol, float masterVol)
    {
        currentSettings.SaveAudioSettings(musicVol, sfxVol, masterVol);
        saveSettingsEvent.RaiseEvent();
    }
}