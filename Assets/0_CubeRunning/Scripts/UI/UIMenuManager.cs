using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private UIConfirmation confirmationPanel = default;
    [SerializeField] private UIMainMenu mainMenuPanel = default;

    [SerializeField] private SaveSystem saveSystem = default;

    [SerializeField] private InputReaderSO inputReader = default;

    [Header("Broadcasting on")] [SerializeField]
    private VoidEventChannelSO startNewSinglePlayerGameEvent = default;

    [SerializeField] private VoidEventChannelSO startNewMultiPlayerGameEvent = default;
    [SerializeField] private VoidEventChannelSO continueGameEvent = default;

    private bool hasSaveData;

    private IEnumerator Start()
    {
        inputReader.EnableMenuInput();
        yield return new WaitForSeconds(0.4f);
        SetMenuScreen();
    }

    private void SetMenuScreen()
    {
        hasSaveData = saveSystem.LoadSaveDataFromDisk();
        mainMenuPanel.SetMenuScreen(hasSaveData);

        mainMenuPanel.ContinueSinglePlayerButtonAction += continueGameEvent.RaiseEvent;
        mainMenuPanel.SinglePlayerButtonAction += ButtonSinglePlayerClicked;
        mainMenuPanel.MultiPlayerButtonAction += ButtonMultiPlayerClicked;
        mainMenuPanel.SettingsButtonAction += OpenSettingsScreen;
        mainMenuPanel.CreditsButtonAction += OpenCreditsScreen;
        mainMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;
    }

    private void ButtonSinglePlayerClicked()
    {
        if (!hasSaveData)
            ConfirmStartNewSinglePlayerGame();
        else
            ShowStartNewSinglePlayerGameConfirmationPopup();
    }

    private void ConfirmStartNewSinglePlayerGame() => startNewSinglePlayerGameEvent.RaiseEvent();

    private void ButtonMultiPlayerClicked() => startNewMultiPlayerGameEvent.RaiseEvent();

    private void ShowStartNewSinglePlayerGameConfirmationPopup()
    {
        confirmationPanel.ConfirmationResponseAction += StartNewSinglePlayerGameConfirmationResponse;
        confirmationPanel.ClosePanelAction += HideConfirmationPanel;

        confirmationPanel.gameObject.SetActive(true);
        confirmationPanel.SetPanel(ConfirmationType.NewMultiPlayerGame);
    }

    private void StartNewSinglePlayerGameConfirmationResponse(bool isNewSinglePlayerGameConfirmed)
    {
        confirmationPanel.ConfirmationResponseAction -= StartNewSinglePlayerGameConfirmationResponse;
        confirmationPanel.ClosePanelAction -= HideConfirmationPanel;

        if (isNewSinglePlayerGameConfirmed)
            ConfirmStartNewSinglePlayerGame();
        else
            continueGameEvent.RaiseEvent();
    }

    private void HideConfirmationPanel()
    {
        confirmationPanel.ClosePanelAction -= HideConfirmationPanel;
        confirmationPanel.gameObject.SetActive(false);
        mainMenuPanel.SetMenuScreen(hasSaveData);
    }

    private void OpenSettingsScreen()
    {
        // TODO: Set active setting panel 

        // TODO: Listen to close button
    }

    private void CloseSettingsScreen()
    {
        // TODO: Unlisten to close button

        // TODO: Set active false

        mainMenuPanel.SetMenuScreen(hasSaveData);
    }

    private void OpenCreditsScreen()
    {
        // TODO: Set active credits panel

        // TODO: Listen to close button
    }

    private void CloseCreditsScreen()
    {
        // TODO: Unlisten to close button

        // TODO: Set active false

        mainMenuPanel.SetMenuScreen(hasSaveData);
    }

    private void ShowExitConfirmationPopup()
    {
        confirmationPanel.ConfirmationResponseAction += CloseExitConfirmationPopup;
        confirmationPanel.gameObject.SetActive(true);
        confirmationPanel.SetPanel(ConfirmationType.Quit);
    }

    private void CloseExitConfirmationPopup(bool isConfirmed)
    {
        confirmationPanel.ConfirmationResponseAction -= CloseExitConfirmationPopup;
        confirmationPanel.gameObject.SetActive(false);
        if (isConfirmed)
        {
#if !UNITY_EDITOR
            Application.Quit();
#else
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        mainMenuPanel.SetMenuScreen(hasSaveData);
    }

    private void OnDestroy()
    {
        confirmationPanel.ConfirmationResponseAction -= CloseExitConfirmationPopup;
        confirmationPanel.ConfirmationResponseAction -= StartNewSinglePlayerGameConfirmationResponse;
    }
}