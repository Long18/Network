using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class UIPause : MonoBehaviour
{
    [SerializeField] private InputReaderSO inputReader = default;
    [SerializeField] private UIGenericButton resumeButton = default;
    [SerializeField] private UIGenericButton settingsButton = default;
    [SerializeField] private UIGenericButton backToMenuButton = default;


    [Header("Listening to")] [SerializeField]
    private BoolEventChannelSO onPauseOpened = default;

    public event UnityAction Resumed = default;
    public event UnityAction SettingsScreenOpened = default;
    public event UnityAction BackToMainRequested = default;

    private void OnEnable()
    {
        onPauseOpened.RaiseEvent(true);

        resumeButton.SetButton(true);
        inputReader.MenuCloseEvent += Resume;
        resumeButton.Clicked += Resume;
        settingsButton.Clicked += OpenSettingsScreen;
        backToMenuButton.Clicked += BackToMainMenuConfirmation;
    }

    private void OnDisable()
    {
        onPauseOpened.RaiseEvent(false);

        inputReader.MenuCloseEvent -= Resume;
        resumeButton.Clicked -= Resume;
        settingsButton.Clicked -= OpenSettingsScreen;
        backToMenuButton.Clicked -= BackToMainMenuConfirmation;
    }

    void Resume() => Resumed.Invoke();

    void OpenSettingsScreen() => SettingsScreenOpened.Invoke();

    void BackToMainMenuConfirmation() => BackToMainRequested.Invoke();

    public void CloseScreen() => Resumed.Invoke();
}