using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;


public class UIManager : MonoBehaviour
{
    [Header("Scene UI")] [SerializeField] private MenuSelectionHandler selectionHandler = default;
    [SerializeField] private UIConfirmation popupPanel = default;
    [SerializeField] private UIInteraction interactionPanel = default;
    [SerializeField] private UIInventory inventoryPanel = default;
    [SerializeField] private UIDialogueManager dialogueManager = default;
    [SerializeField] private UIPause pauseScreen = default;
    [SerializeField] private UISettingsController settingScreen = default;
    [SerializeField] private GameObject switchTabDisplay = default;

    [Header("Gameplay")] [SerializeField] private GameStateSO gameStateManager = default;
    [SerializeField] private MenuSO mainMenu = default;
    [SerializeField] private InputReaderSO inputReader = default;
    [SerializeField] private ActorSO mainProtagonist = default;

    [Header("Dialogue")] [SerializeField] private DialogueLineChannelSO openUIDialogueEvent = default;
    [SerializeField] private IntEventChannelSO closeUIDialogueEvent = default;

    [Header("Interaction")] [SerializeField]
    private InteractionUIEventChannelSO setInteractionEvent = default;

    [Header("Inventory")] [SerializeField] private VoidEventChannelSO openInventoryScreenForShoppingEvent = default;

    [Header("Listening on")] [SerializeField]
    private VoidEventChannelSO onSceneReady = default;

    [Header("Broadcasting on ")] [SerializeField]
    private LoadEventChannelSO loadMenuEvent = default;

    [SerializeField] private VoidEventChannelSO onInteractionEndedEvent = default;

    private bool isForShopping = false;

    private void OnEnable()
    {
        onSceneReady.OnEventRaised += ResetUI;
        openUIDialogueEvent.OnEventRaised += OpenUIDialogue;
        closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;
        inputReader.MenuPauseEvent +=
            OpenUIPause; // subscription to open Pause UI event happens in OnEnabled, but the close Event is only subscribed to when the popup is open
        openInventoryScreenForShoppingEvent.OnEventRaised += SetInventoryScreenForShopping;
        setInteractionEvent.OnEventRaised += SetInteractionPanel;
        inputReader.OpenInventoryEvent += SetInventoryScreen;
        inventoryPanel.Closed += CloseInventoryScreen;
    }

    private void OnDisable()
    {
        onSceneReady.OnEventRaised -= ResetUI;
        openUIDialogueEvent.OnEventRaised -= OpenUIDialogue;
        closeUIDialogueEvent.OnEventRaised -= CloseUIDialogue;
        inputReader.MenuPauseEvent -= OpenUIPause;
        openInventoryScreenForShoppingEvent.OnEventRaised -= SetInventoryScreenForShopping;
        setInteractionEvent.OnEventRaised -= SetInteractionPanel;
        inputReader.OpenInventoryEvent -= SetInventoryScreen;
        inventoryPanel.Closed -= CloseInventoryScreen;
    }

    private void ResetUI()
    {
        dialogueManager.gameObject.SetActive(false);
        pauseScreen.gameObject.SetActive(false);
        interactionPanel.gameObject.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);

        Time.timeScale = 1;
    }

    private void OpenUIDialogue(LocalizedString dialogueLine, ActorSO actor)
    {
        bool isPlayerTalking = (actor == mainProtagonist);
        dialogueManager.SetDialogue(dialogueLine, actor, isPlayerTalking);
        interactionPanel.gameObject.SetActive(false);
        dialogueManager.gameObject.SetActive(true);
    }

    private void CloseUIDialogue(int dialogueType)
    {
        selectionHandler.Unselect();
        dialogueManager.gameObject.SetActive(false);
        onInteractionEndedEvent.RaiseEvent();
    }

    private void OpenUIPause()
    {
        inputReader.MenuPauseEvent -= OpenUIPause; // you can open UI pause menu again, if it's closed

        Time.timeScale = 0; // Pause time

        pauseScreen.SettingsScreenOpened +=
            OpenSettingScreen; //once the UI Pause popup is open, listen to open Settings 
        pauseScreen.BackToMainRequested +=
            ShowBackToMenuConfirmationPopup; //once the UI Pause popup is open, listen to back to menu button
        pauseScreen.Resumed += CloseUIPause; //once the UI Pause popup is open, listen to unpause event

        pauseScreen.gameObject.SetActive(true);

        inputReader.EnableMenuInput();
        gameStateManager.UpdateGameState(GameState.Pause);
    }

    private void CloseUIPause()
    {
        Time.timeScale = 1; // unpause time

        inputReader.MenuPauseEvent += OpenUIPause; // you can open UI pause menu again, if it's closed

        // once the popup is closed, you can't listen to the following events 
        pauseScreen.SettingsScreenOpened -=
            OpenSettingScreen; //once the UI Pause popup is open, listen to open Settings 
        pauseScreen.BackToMainRequested -=
            ShowBackToMenuConfirmationPopup; //once the UI Pause popup is open, listen to back to menu button
        pauseScreen.Resumed -= CloseUIPause; //once the UI Pause popup is open, listen to unpause event

        pauseScreen.gameObject.SetActive(false);

        gameStateManager.ReturnToPreviousGameState();

        if (gameStateManager.CurrentGameState == GameState.Gameplay
            || gameStateManager.CurrentGameState == GameState.Combat)
        {
            inputReader.EnableGameplayInput();
        }

        selectionHandler.Unselect();
    }

    private void SetInventoryScreenForShopping()
    {
        if (gameStateManager.CurrentGameState == GameState.Gameplay)
        {
            isForShopping = true;
            interactionPanel.gameObject.SetActive(false);
            OpenInventoryScreen();
        }
    }

    private void SetInventoryScreen()
    {
        if (gameStateManager.CurrentGameState == GameState.Gameplay)
        {
            isForShopping = false;
            OpenInventoryScreen();
        }
    }

    private void OpenInventoryScreen()
    {
        inputReader.MenuPauseEvent -= OpenUIPause;
        inputReader.MenuUnpauseEvent -= CloseUIPause;

        inputReader.MenuCloseEvent += CloseInventoryScreen;
        inputReader.CloseInventoryEvent += CloseInventoryScreen;

        if (isForShopping)
        {
            inventoryPanel.FillInventory(InventoryTabType.Sword, true);
        }
        else
        {
            inventoryPanel.FillInventory();
        }

        inventoryPanel.gameObject.SetActive(true);
        switchTabDisplay.SetActive(true);
        inputReader.EnableMenuInput();

        gameStateManager.UpdateGameState(GameState.Inventory);
    }

    private void CloseInventoryScreen()
    {
        inputReader.MenuPauseEvent += OpenUIPause;

        inputReader.MenuCloseEvent -= CloseInventoryScreen;
        inputReader.CloseInventoryEvent -= CloseInventoryScreen;

        switchTabDisplay.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);

        if (isForShopping)
        {
            onInteractionEndedEvent.RaiseEvent();
        }

        selectionHandler.Unselect();
        gameStateManager.ReturnToPreviousGameState();
        if (gameStateManager.CurrentGameState == GameState.Gameplay ||
            gameStateManager.CurrentGameState == GameState.Combat)
            inputReader.EnableGameplayInput();
    }

    private void OpenSettingScreen()
    {
        settingScreen.Closed += CloseSettingScreen; // sub to close setting event with event 

        pauseScreen.gameObject.SetActive(false); // Set pause screen to inactive

        settingScreen.gameObject.SetActive(true); // set Setting screen to active 
    }

    private void CloseSettingScreen()
    {
        //unsub from close setting events 
        settingScreen.Closed -= CloseSettingScreen;

        selectionHandler.Unselect();
        pauseScreen.gameObject.SetActive(true); // Set pause screen to inactive

        settingScreen.gameObject.SetActive(false);
    }

    private void ShowBackToMenuConfirmationPopup()
    {
        pauseScreen.gameObject.SetActive(false); // Set pause screen to inactive

        popupPanel.ClosePanelAction += HideBackToMenuConfirmationPopup;

        popupPanel.ConfirmationResponseAction += BackToMainMenu;

        inputReader.EnableMenuInput();
        popupPanel.gameObject.SetActive(true);
        popupPanel.SetPanel(ConfirmationType.BackToMenu);
    }

    private void BackToMainMenu(bool confirm)
    {
        HideBackToMenuConfirmationPopup(); // hide confirmation screen, show close UI pause, 

        if (confirm)
        {
            CloseUIPause(); //close ui pause to unsub from all events 
            loadMenuEvent.RequestLoadScene(mainMenu, false); //load main menu
        }
    }

    private void HideBackToMenuConfirmationPopup()
    {
        popupPanel.ClosePanelAction -= HideBackToMenuConfirmationPopup;
        popupPanel.ConfirmationResponseAction -= BackToMainMenu;

        popupPanel.gameObject.SetActive(false);
        selectionHandler.Unselect();
        pauseScreen.gameObject.SetActive(true); // Set pause screen to inactive
    }

    private void SetInteractionPanel(bool isOpen, InteractionType type)
    {
        if (gameStateManager.CurrentGameState != GameState.Combat)
        {
            Debug.Log($"type: {type}");
            if (isOpen)
            {
                interactionPanel.FillInteractionPanel(type);
            }

            interactionPanel.gameObject.SetActive(isOpen);
        }
        else if (!isOpen)
        {
            interactionPanel.gameObject.SetActive(isOpen);
        }
    }
}