using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Create new settings SO")]
public class SettingsSO : ScriptableObject
{
    [SerializeField] float masterVolume = default;
    [SerializeField] float musicVolume = default;
    [SerializeField] float sfxVolume = default;
    [SerializeField] int resolutionsIndex = default;
    [SerializeField] int antiAliasingIndex = default;
    [SerializeField] float shadowDistance = default;
    [SerializeField] bool isFullscreen = default;
    [SerializeField] bool isMultiplay = default;
    [SerializeField] Locale currentLocale = default;
    public float MasterVolume => masterVolume;
    public float MusicVolume => musicVolume;
    public float SfxVolume => sfxVolume;
    public int ResolutionsIndex => resolutionsIndex;
    public int AntiAliasingIndex => antiAliasingIndex;
    public float ShadowDistance => shadowDistance;
    public bool IsFullscreen => isFullscreen;
    public bool IsMultiplay => isMultiplay;
    public Locale CurrentLocale => currentLocale;

    public void SaveAudioSettings(float newMusicVolume, float newSfxVolume, float newMasterVolume)
    {
        masterVolume = newMasterVolume;
        musicVolume = newMusicVolume;
        sfxVolume = newSfxVolume;
    }

    public void SaveGraphicsSettings(int newResolutionsIndex, int newAntiAliasingIndex, float newShadowDistance,
        bool fullscreenState)
    {
        resolutionsIndex = newResolutionsIndex;
        antiAliasingIndex = newAntiAliasingIndex;
        shadowDistance = newShadowDistance;
        isFullscreen = fullscreenState;
    }

    public void SaveLanguageSettings(Locale local)
    {
        currentLocale = local;
    }

    public void SaveGameMode(bool _isMultiplay)
    {
        isMultiplay = _isMultiplay;
    }

    public SettingsSO()
    {
    }

    public void LoadSavedSettings(Save savedFile)
    {
        masterVolume = savedFile.MasterVolume;
        musicVolume = savedFile.MusicVolume;
        sfxVolume = savedFile.SfxVolume;
        resolutionsIndex = savedFile.ResolutionsIndex;
        antiAliasingIndex = savedFile.AntiAliasingIndex;
        shadowDistance = savedFile.ShadowDistance;
        isFullscreen = savedFile.IsFullscreen;
        isMultiplay = savedFile.IsMultiplay;
        currentLocale = savedFile.CurrentLocale;
    }
}