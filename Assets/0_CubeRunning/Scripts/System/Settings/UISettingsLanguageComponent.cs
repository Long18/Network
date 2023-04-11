using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UISettingsLanguageComponent : MonoBehaviour
{
    [SerializeField] private UISettingItemFiller languageFillter = default;
    [SerializeField] private UIGenericButton saveButton = default;
    [SerializeField] private UIGenericButton resetButton = default;

    public event UnityAction<Locale> Save = delegate { };

    private int currentSelectedOption = 0;
    private int savedSelectedOption = default;
    private AsyncOperationHandle initializeOperation;
    private List<string> languagesList = new List<string>();

    private void OnEnable()
    {
        initializeOperation = LocalizationSettings.SelectedLocaleAsync;

        if (initializeOperation.IsDone)
        {
            InitializeCompleted(initializeOperation);
        }
        else
        {
            initializeOperation.Completed += InitializeCompleted;
        }

        saveButton.Clicked += SaveSettings;
        resetButton.Clicked += ResetSettings;
        languageFillter.OnNextOption += NextOption;
        languageFillter.OnPreviousOption += PreviousOption;
    }

    private void OnDisable()
    {
        ResetSettings();

        saveButton.Clicked -= SaveSettings;
        resetButton.Clicked -= ResetSettings;
        languageFillter.OnNextOption -= NextOption;
        languageFillter.OnPreviousOption -= PreviousOption;
        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;
    }

    private void InitializeCompleted(AsyncOperationHandle obj)
    {
        initializeOperation.Completed -= InitializeCompleted;
        // Create an option in the dropdown for each Locale
        languagesList = new List<string>();

        List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;

        for (int i = 0; i < locales.Count; ++i)
        {
            var locale = locales[i];
            if (LocalizationSettings.SelectedLocale == locale) currentSelectedOption = i;

            var displayName = locales[i].Identifier.CultureInfo != null
                ? locales[i].Identifier.CultureInfo.DisplayName
                : locales[i].Identifier.Code;
            languagesList.Add(displayName);
        }

        languageFillter.FillSettingsFiled(languagesList.Count, currentSelectedOption,
            languagesList[currentSelectedOption]);
        savedSelectedOption = currentSelectedOption;
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }

    private void NextOption()
    {
        currentSelectedOption++;
        currentSelectedOption = Mathf.Clamp(currentSelectedOption, 0, languagesList.Count - 1);
        OnSelectionChanged();
    }

    private void PreviousOption()
    {
        currentSelectedOption--;
        currentSelectedOption = Mathf.Clamp(currentSelectedOption, 0, languagesList.Count - 1);
        OnSelectionChanged();
    }

    private void OnSelectionChanged()
    {
        // Unsubscribe from SelectedLocaleChanged event so we don't get a callback when we change the locale.
        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;

        var locale = LocalizationSettings.AvailableLocales.Locales[currentSelectedOption];
        LocalizationSettings.SelectedLocale = locale;

        // Re-subscribe to SelectedLocaleChanged event so we get a callback when the locale is changed.
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }

    private void LocalizationSettings_SelectedLocaleChanged(Locale locale)
    {
        var selectedIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
        languageFillter.FillSettingsFiled(languagesList.Count, selectedIndex, languagesList[selectedIndex]);
    }

    public void SaveSettings()
    {
        Locale currentLocale = LocalizationSettings.AvailableLocales.Locales[currentSelectedOption];
        savedSelectedOption = currentSelectedOption;
        Save.Invoke(currentLocale);
    }

    public void ResetSettings()
    {
        currentSelectedOption = savedSelectedOption;
        OnSelectionChanged();
    }
}