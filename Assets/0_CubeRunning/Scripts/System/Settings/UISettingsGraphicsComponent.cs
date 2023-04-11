using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;


[Serializable]
public class ShadowDistanceTier
{
    public float Distance;
    public string TierDescription;
}

public class UISettingsGraphicsComponent : MonoBehaviour
{
    [SerializeField]
    private List<ShadowDistanceTier> shadowDistanceTierList = new List<ShadowDistanceTier>(); // filled from inspector

    [SerializeField] private UniversalRenderPipelineAsset uRPAsset = default;

    private int savedResolutionIndex = default;
    private int savedAntiAliasingIndex = default;

    private int savedShadowDistanceTier = default;

    // private int savedShadowQualityIndex = default;
    private bool savedFullscreenState = default;

    private int currentResolutionIndex = default;
    private List<Resolution> resolutionsList = default;
    [SerializeField] UISettingItemFiller resolutionsField = default;

    // private int currentShadowQualityIndex = default;
    // private List<string> shadowQualityList = default;
    // [SerializeField] private UISettingItemFiller shadowQualityField = default;

    private int currentAntiAliasingIndex = default;
    private List<string> currentAntiAliasingList = default;
    [SerializeField] private UISettingItemFiller antiAliasingField = default;

    private int currentShadowDistanceTier = default;
    [SerializeField] private UISettingItemFiller shadowDistanceField = default;
    private bool isFullscreen = default;

    [SerializeField] private UISettingItemFiller fullscreenField = default;

    public event UnityAction<int, int, float, bool> Save = delegate { };

    private Resolution currentResolution;

    [SerializeField] private UIGenericButton saveButton;
    [SerializeField] private UIGenericButton resetButton;

    private void OnEnable()
    {
        resolutionsField.OnNextOption += NextResolution;
        resolutionsField.OnPreviousOption += PreviousResolution;

        shadowDistanceField.OnNextOption += NextShadowDistanceTier;
        shadowDistanceField.OnPreviousOption += PreviousShadowDistanceTier;

        // shadowQualityField.OnNextOption += NextShadowQuality;
        // shadowQualityField.OnPreviousOption += PreviousShadowQuality;

        fullscreenField.OnNextOption += NextFullscreenState;
        fullscreenField.OnPreviousOption += PreviousFullscreenState;

        antiAliasingField.OnNextOption += NextAntiAliasingTier;
        antiAliasingField.OnPreviousOption += PreviousAntiAliasingTier;

        saveButton.Clicked += SaveSettings;
        resetButton.Clicked += ResetSettings;
    }

    private void OnDisable()
    {
        ResetSettings();

        resolutionsField.OnNextOption -= NextResolution;
        resolutionsField.OnPreviousOption -= PreviousResolution;

        shadowDistanceField.OnNextOption -= NextShadowDistanceTier;
        shadowDistanceField.OnPreviousOption -= PreviousShadowDistanceTier;

        // shadowQualityField.OnNextOption -= NextShadowQuality;
        // shadowQualityField.OnPreviousOption -= PreviousShadowQuality;

        fullscreenField.OnNextOption -= NextFullscreenState;
        fullscreenField.OnPreviousOption -= PreviousFullscreenState;

        antiAliasingField.OnNextOption -= NextAntiAliasingTier;
        antiAliasingField.OnPreviousOption -= PreviousAntiAliasingTier;

        saveButton.Clicked -= SaveSettings;
        resetButton.Clicked -= ResetSettings;
    }

    public void Init()
    {
        resolutionsList = GetResolutionsList();
        currentShadowDistanceTier = GetCurrentShadowDistanceTier();
        // currentShadowQualityIndex = GetCurrentShadowQuality();
        currentAntiAliasingList = GetDropdownData(Enum.GetNames(typeof(MsaaQuality)));

        currentResolution = Screen.currentResolution;
        currentResolutionIndex = GetCurrentResolutionIndex();
        isFullscreen = GetCurrentFullscreenState();
        currentAntiAliasingIndex = GetCurrentAntialiasing();

        savedResolutionIndex = currentResolutionIndex;
        savedAntiAliasingIndex = currentAntiAliasingIndex;
        savedShadowDistanceTier = currentShadowDistanceTier;
        // savedShadowQualityIndex = currentShadowQualityIndex;
        savedFullscreenState = isFullscreen;
    }

    public void Setup()
    {
        Init();
        SetResolutionField();
        SetShadowDistance();
        // SetShadowQuality();
        SetFullscreen();
        SetAntiAliasingField();
    }

    #region Resolution

    private void SetResolutionField()
    {
        string displayText = resolutionsList[currentResolutionIndex].ToString();
        displayText = displayText.Substring(0, displayText.IndexOf("@"));

        resolutionsField.FillSettings(resolutionsList.Count, currentResolutionIndex, displayText);
    }

    private List<Resolution> GetResolutionsList()
    {
        List<Resolution> options = new List<Resolution>();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            options.Add(Screen.resolutions[i]);
        }

        return options;
    }

    private int GetCurrentResolutionIndex()
    {
        if (resolutionsList == null)
        {
            resolutionsList = GetResolutionsList();
        }

        int index = resolutionsList.FindIndex(o =>
            o.width == currentResolution.width && o.height == currentResolution.height);
        return index;
    }

    private void NextResolution()
    {
        currentResolutionIndex++;
        currentResolutionIndex = Mathf.Clamp(currentResolutionIndex, 0, resolutionsList.Count - 1);
        OnResolutionChange();
    }

    private void PreviousResolution()
    {
        currentResolutionIndex--;
        currentResolutionIndex = Mathf.Clamp(currentResolutionIndex, 0, resolutionsList.Count - 1);
        OnResolutionChange();
    }

    private void OnResolutionChange()
    {
        currentResolution = resolutionsList[currentResolutionIndex];
        Screen.SetResolution(currentResolution.width, currentResolution.height, isFullscreen);
        SetResolutionField();
    }

    #endregion

    #region Shadow Quality

    //
    // private void SetShadowQuality()
    // {
    //     shadowQualityField.FillSettings_Localized(shadowQualityList.Count, currentShadowQualityIndex,
    //         shadowQualityList[currentShadowQualityIndex]);
    // }
    //
    // private int GetCurrentShadowQuality()
    // {
    //     int index = -1;
    //     if (uRPAsset != null && shadowQualityList.Exists(a => a == uRPAsset.shadowCascadeOption.ToString()))
    //         index = shadowQualityList.FindIndex(a => a == uRPAsset.shadowCascadeOption.ToString());
    //     else
    //     {
    //         Debug.LogError("Current shadow quality is not in the tier List " + uRPAsset.shadowCascadeOption.ToString());
    //     }
    //
    //     return index;
    // }
    //
    // private void NextShadowQuality()
    // {
    //     currentShadowQualityIndex++;
    //     currentShadowQualityIndex = Mathf.Clamp(currentShadowQualityIndex, 0, shadowQualityList.Count - 1);
    //     OnShadowQualityChange();
    // }
    //
    // private void PreviousShadowQuality()
    // {
    //     currentShadowQualityIndex--;
    //     currentShadowQualityIndex = Mathf.Clamp(currentShadowQualityIndex, 0, shadowQualityList.Count - 1);
    //     OnShadowQualityChange();
    // }
    //
    // private void OnShadowQualityChange()
    // {
    //     uRPAsset.shadowCascadeOption = (ShadowCascadesOption)currentShadowQualityIndex;
    //     SetShadowQuality();
    // }

    #endregion

    #region ShadowDistance

    private void SetShadowDistance()
    {
        shadowDistanceField.FillSettings_Localized(shadowDistanceTierList.Count, currentShadowDistanceTier,
            shadowDistanceTierList[currentShadowDistanceTier].TierDescription);
    }

    private int GetCurrentShadowDistanceTier()
    {
        int tierIndex = -1;
        if (shadowDistanceTierList.Exists(a => a.Distance == uRPAsset.shadowDistance))
            tierIndex = shadowDistanceTierList.FindIndex(a => a.Distance == uRPAsset.shadowDistance);
        else
        {
            Debug.LogError("Current shadow distance is not in the tier List " + uRPAsset.shadowDistance);
        }

        return tierIndex;
    }

    private void NextShadowDistanceTier()
    {
        currentShadowDistanceTier++;
        currentShadowDistanceTier = Mathf.Clamp(currentShadowDistanceTier, 0, shadowDistanceTierList.Count);
        OnShadowDistanceChange();
    }

    private void PreviousShadowDistanceTier()
    {
        currentShadowDistanceTier--;
        currentShadowDistanceTier = Mathf.Clamp(currentShadowDistanceTier, 0, shadowDistanceTierList.Count);
        OnShadowDistanceChange();
    }

    private void OnShadowDistanceChange()
    {
        uRPAsset.shadowDistance = shadowDistanceTierList[currentShadowDistanceTier].Distance;
        SetShadowDistance();
    }

    #endregion

    #region fullscreen

    private void SetFullscreen()
    {
        if (isFullscreen)
        {
            fullscreenField.FillSettings_Localized(2, 1, "On");
        }
        else
        {
            fullscreenField.FillSettings_Localized(2, 0, "Off");
        }
    }

    private bool GetCurrentFullscreenState() => Screen.fullScreen;

    private void NextFullscreenState()
    {
        isFullscreen = true;
        OnFullscreenChange();
    }

    private void PreviousFullscreenState()
    {
        isFullscreen = false;
        OnFullscreenChange();
    }

    private void OnFullscreenChange()
    {
        Screen.fullScreen = isFullscreen;
        SetFullscreen();
    }

    #endregion

    #region Anti Aliasing

    private void SetAntiAliasingField()
    {
        string optionDisplay = currentAntiAliasingList[currentAntiAliasingIndex].Replace("_", "");
        antiAliasingField.FillSettings(currentAntiAliasingList.Count, currentAntiAliasingIndex, optionDisplay);
    }

    private int GetCurrentAntialiasing()
    {
        return uRPAsset.msaaSampleCount - 1;
    }

    private void NextAntiAliasingTier()
    {
        currentAntiAliasingIndex++;
        currentAntiAliasingIndex = Mathf.Clamp(currentAntiAliasingIndex, 0, currentAntiAliasingList.Count - 1);
        OnAntiAliasingChange();
    }

    private void PreviousAntiAliasingTier()
    {
        currentAntiAliasingIndex--;
        currentAntiAliasingIndex = Mathf.Clamp(currentAntiAliasingIndex, 0, currentAntiAliasingList.Count - 1);
        OnAntiAliasingChange();
    }

    private void OnAntiAliasingChange()
    {
        uRPAsset.msaaSampleCount = currentAntiAliasingIndex;
        SetAntiAliasingField();
    }

    #endregion

    private List<string> GetDropdownData(string[] optionNames, params string[] customOptions)
    {
        List<string> options = new List<string>();
        foreach (string option in optionNames)
        {
            options.Add(option);
        }

        foreach (string option in customOptions)
        {
            options.Add(option);
        }

        return options;
    }

    public void SaveSettings()
    {
        savedResolutionIndex = currentResolutionIndex;
        savedAntiAliasingIndex = currentAntiAliasingIndex;
        savedShadowDistanceTier = currentShadowDistanceTier;
        // savedShadowQualityIndex = currentShadowQualityIndex;
        savedFullscreenState = isFullscreen;
        float shadowDistance = shadowDistanceTierList[currentShadowDistanceTier].Distance;
        Save.Invoke(currentResolutionIndex, currentAntiAliasingIndex, shadowDistance, isFullscreen);
    }

    public void ResetSettings()
    {
        currentResolutionIndex = savedResolutionIndex;
        OnResolutionChange();
        currentAntiAliasingIndex = savedAntiAliasingIndex;
        OnAntiAliasingChange();
        currentShadowDistanceTier = savedShadowDistanceTier;
        OnShadowDistanceChange();
        // currentShadowQualityIndex = savedShadowQualityIndex;
        // OnShadowQualityChange();
        isFullscreen = savedFullscreenState;
        OnFullscreenChange();
    }
}