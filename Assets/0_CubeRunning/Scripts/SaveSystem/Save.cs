using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

/// <summary>
/// This class contains all the variables that will be serialized and saved to a file.<br/>
/// Can be considered as a save file structure or format.
/// </summary>
[Serializable]
public class Save
{
    // This is test data, written according to TestScript.cs class
    // This will change according to whatever data that needs to be stored

    // The variables need to be public, else we would have to write trivial getter/setter functions.
    public string LocationId;
    public List<SerializedItemStack> ItemStacks = new List<SerializedItemStack>();
    public List<string> FinishedQuestlineItemsGUIds = new List<string>();

    public float MasterVolume = default;
    public float MusicVolume = default;
    public float SfxVolume = default;
    public int ResolutionsIndex = default;
    public int AntiAliasingIndex = default;
    public float ShadowDistance = default;
    public bool IsFullscreen = default;
    public bool IsMultiplay = default;
    public Locale CurrentLocale = default;

    public void SaveSettings(SettingsSO settings)
    {
        MasterVolume = settings.MasterVolume;
        MusicVolume = settings.MusicVolume;
        SfxVolume = settings.SfxVolume;
        ResolutionsIndex = settings.ResolutionsIndex;
        AntiAliasingIndex = settings.AntiAliasingIndex;
        ShadowDistance = settings.ShadowDistance;
        IsFullscreen = settings.IsFullscreen;
        CurrentLocale = settings.CurrentLocale;
    }

    public void ResetSettings()
    {
        LocationId = default;
        ItemStacks = default;
        FinishedQuestlineItemsGUIds = default;

        MasterVolume = default;
        MusicVolume = default;
        SfxVolume = default;
        ResolutionsIndex = default;
        AntiAliasingIndex = default;
        ShadowDistance = default;
        IsFullscreen = default;
        CurrentLocale = default;
    }

    public string ToJson() => JsonUtility.ToJson(this);

    public void LoadFromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);
}