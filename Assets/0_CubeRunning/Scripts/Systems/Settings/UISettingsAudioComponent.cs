using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class UISettingsAudioComponent : MonoBehaviour
{
    [SerializeField] UISettingItemFiller masterVolumeField;
    [SerializeField] UISettingItemFiller musicVolumeField;
    [SerializeField] UISettingItemFiller sfxVolumeField;

    [SerializeField] UIGenericButton saveButton;
    [SerializeField] UIGenericButton resetButton;


    [Header("Broadcasting")] [SerializeField]
    private FloatEventChannelSO masterVolumeEventChannel = default;

    [SerializeField] private FloatEventChannelSO sFXVolumeEventChannel = default;
    [SerializeField] private FloatEventChannelSO musicVolumeEventChannel = default;
    private float musicVolume { get; set; }
    private float sfxVolume { get; set; }
    private float masterVolume { get; set; }
    private float savedMusicVolume { get; set; }
    private float savedSfxVolume { get; set; }
    private float savedMasterVolume { get; set; }

    int maxVolume = 10;

    public event UnityAction<float, float, float> Save = delegate { };

    private void OnEnable()
    {
        musicVolumeField.OnNextOption += IncreaseMusicVolume;
        musicVolumeField.OnPreviousOption += DecreaseMusicVolume;
        saveButton.Clicked += SaveVolumes;
        resetButton.Clicked += ResetVolumes;
        sfxVolumeField.OnNextOption += IncreaseSFXVolume;
        sfxVolumeField.OnPreviousOption += DecreaseSFXVolume;
        masterVolumeField.OnNextOption += IncreaseMasterVolume;
        masterVolumeField.OnPreviousOption += DecreaseMasterVolume;
    }

    private void OnDisable()
    {
        ResetVolumes(); // reset volumes on disable. If not saved, it will reset to initial volumes. 
        musicVolumeField.OnNextOption -= IncreaseMusicVolume;
        musicVolumeField.OnPreviousOption -= DecreaseMusicVolume;
        saveButton.Clicked -= SaveVolumes;
        resetButton.Clicked -= ResetVolumes;
        sfxVolumeField.OnNextOption -= IncreaseSFXVolume;
        sfxVolumeField.OnPreviousOption -= DecreaseSFXVolume;
        masterVolumeField.OnNextOption -= IncreaseMasterVolume;
        masterVolumeField.OnPreviousOption -= DecreaseMasterVolume;
    }

    public void Setup(float musicVolume, float sfxVolume, float masterVolume)
    {
        this.masterVolume = masterVolume;
        this.musicVolume = sfxVolume;
        this.sfxVolume = musicVolume;

        savedMasterVolume = this.masterVolume;
        savedMusicVolume = this.musicVolume;
        savedSfxVolume = this.sfxVolume;

        SetMusicVolumeField();
        SetSfxVolumeField();
        SetMasterVolumeField();
    }

    private void SetMusicVolumeField()
    {
        int paginationCount = maxVolume + 1; // adding a page in the pagination since the count starts from 0
        int selectedPaginationIndex = Mathf.RoundToInt(maxVolume * musicVolume);
        string selectedOption = Mathf.RoundToInt(maxVolume * musicVolume).ToString();

        SetMusicVolume();

        musicVolumeField.FillSettings(paginationCount, selectedPaginationIndex, selectedOption);
    }

    private void SetSfxVolumeField()
    {
        int paginationCount = maxVolume + 1; // adding a page in the pagination since the count starts from 0
        int selectedPaginationIndex = Mathf.RoundToInt(maxVolume * sfxVolume);
        string selectedOption = Mathf.RoundToInt(maxVolume * sfxVolume).ToString();

        SetSfxVolume();

        sfxVolumeField.FillSettings(paginationCount, selectedPaginationIndex, selectedOption);
    }

    private void SetMasterVolumeField()
    {
        int paginationCount = maxVolume + 1; // adding a page in the pagination since the count starts from 0
        int selectedPaginationIndex = Mathf.RoundToInt(maxVolume * masterVolume);
        string selectedOption = Mathf.RoundToInt(maxVolume * masterVolume).ToString();

        SetMasterVolume();

        masterVolumeField.FillSettings(paginationCount, selectedPaginationIndex, selectedOption);
    }

    private void SetMusicVolume() => musicVolumeEventChannel.RaiseEvent(musicVolume); //raise event for volume change

    private void SetSfxVolume() => sFXVolumeEventChannel.RaiseEvent(sfxVolume); //raise event for volume change

    private void SetMasterVolume() => masterVolumeEventChannel.RaiseEvent(masterVolume); //raise event for volume change

    private void IncreaseMasterVolume()
    {
        masterVolume += 1 / (float)maxVolume;
        masterVolume = Mathf.Clamp(masterVolume, 0, 1);
        SetMasterVolumeField();
    }

    private void DecreaseMasterVolume()
    {
        masterVolume -= 1 / (float)maxVolume;
        masterVolume = Mathf.Clamp(masterVolume, 0, 1);
        SetMasterVolumeField();
    }

    private void IncreaseMusicVolume()
    {
        musicVolume += 1 / (float)maxVolume;
        musicVolume = Mathf.Clamp(musicVolume, 0, 1);
        SetMusicVolumeField();
    }

    private void DecreaseMusicVolume()
    {
        musicVolume -= 1 / (float)maxVolume;
        musicVolume = Mathf.Clamp(musicVolume, 0, 1);
        SetMusicVolumeField();
    }

    private void IncreaseSFXVolume()
    {
        sfxVolume += 1 / (float)maxVolume;
        sfxVolume = Mathf.Clamp(sfxVolume, 0, 1);

        SetSfxVolumeField();
    }

    private void DecreaseSFXVolume()
    {
        sfxVolume -= 1 / (float)maxVolume;
        sfxVolume = Mathf.Clamp(sfxVolume, 0, 1);
        SetSfxVolumeField();
    }

    private void ResetVolumes() => Setup(savedMusicVolume, savedSfxVolume, savedMasterVolume);

    private void SaveVolumes()
    {
        savedMasterVolume = masterVolume;
        savedMusicVolume = musicVolume;
        savedSfxVolume = sfxVolume;
        //save Audio
        Save.Invoke(musicVolume, sfxVolume, masterVolume);
    }
}