using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;


public class SettingsSystem : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO SaveSettingsEvent = default;
    [SerializeField] private SettingsSO currentSettings = default;
    [SerializeField] private UniversalRenderPipelineAsset urpAsset = default;
    [SerializeField] private SaveSystem saveSystem = default;

    [Header("Sound")] [SerializeField] private FloatEventChannelSO changeMasterVolumeEventChannel = default;
    [SerializeField] private FloatEventChannelSO changeSFXVolumeEventChannel = default;
    [SerializeField] private FloatEventChannelSO changeMusicVolumeEventChannel = default;

    private void Awake()
    {
        saveSystem.LoadSaveDataFromDisk();
        currentSettings.LoadSavedSettings(saveSystem.saveData);
        SetCurrentSettings();
    }

    private void OnEnable()
    {
        SaveSettingsEvent.OnEventRaised += SaveSettings;
    }

    private void OnDisable()
    {
        SaveSettingsEvent.OnEventRaised -= SaveSettings;
    }

    /// <summary>
    /// Set current settings 
    /// </summary>
    void SetCurrentSettings()
    {
        changeMusicVolumeEventChannel.RaiseEvent(currentSettings.MusicVolume); //raise event for volume change
        changeSFXVolumeEventChannel.RaiseEvent(currentSettings.SfxVolume); //raise event for volume change
        changeMasterVolumeEventChannel.RaiseEvent(currentSettings.MasterVolume); //raise event for volume change
        Resolution
            currentResolution =
                Screen.currentResolution; // get a default resolution in case saved resolution doesn't exist in the resolution List
        if (currentSettings.ResolutionsIndex < Screen.resolutions.Length)
            currentResolution = Screen.resolutions[currentSettings.ResolutionsIndex];
        Screen.SetResolution(currentResolution.width, currentResolution.height, currentSettings.IsFullscreen);
        urpAsset.shadowDistance = currentSettings.ShadowDistance;
        urpAsset.msaaSampleCount = currentSettings.AntiAliasingIndex;

        LocalizationSettings.SelectedLocale = currentSettings.CurrentLocale;
    }

    void SaveSettings() => saveSystem.SaveDataToDisk();
}