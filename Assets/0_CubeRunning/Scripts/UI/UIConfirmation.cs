using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public enum ButtonType
{
    None = 0,
    Confirm = 1,
    Cancel = 2,
    Close = 3,
}

public enum ConfirmationType
{
    None = 0,
    NewGame = 1,
    BackToMenu = 2,
    Quit = 3,
}

public class UIConfirmation : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent titleText = default;
    [SerializeField] private LocalizeStringEvent descriptionText = default;
    [SerializeField] private Button buttonClose = default;
    [SerializeField] private UIGenericButton popupButton1 = default;
    [SerializeField] private UIGenericButton popupButton2 = default;
    [SerializeField] private InputReaderSO inputReader = default;

    private ConfirmationType actualType;

    public event UnityAction<bool> ConfirmationResponseAction;
    public event UnityAction ClosePanelAction;

    private void OnDisable()
    {
        popupButton2.Clicked -= CancelButtonClicked;
        popupButton1.Clicked -= ConfirmButtonClicked;
        inputReader.MenuCloseEvent -= CloseMenuButtonClicked;
    }

    public void SetPanel(ConfirmationType confirmationType)
    {
        actualType = confirmationType;
        bool isConfirmation = false;
        bool hasExitButton = false;
        titleText.StringReference.TableEntryReference = actualType.ToString() + "_Popup_Title";
        descriptionText.StringReference.TableEntryReference = actualType.ToString() + "_Popup_Description";
        string tableEntryReferenceConfirm = ButtonType.Confirm + "_" + actualType;
        string tableEntryReferenceCancel = ButtonType.Cancel + "_" + actualType;
        switch (actualType)
        {
            case ConfirmationType.NewGame:
            case ConfirmationType.BackToMenu:
                isConfirmation = true;

                popupButton1.SetButton(tableEntryReferenceConfirm, true);
                popupButton2.SetButton(tableEntryReferenceCancel, false);
                hasExitButton = true;
                break;
            case ConfirmationType.Quit:
                isConfirmation = true;

                popupButton1.SetButton(tableEntryReferenceConfirm, true);
                popupButton2.SetButton(tableEntryReferenceCancel, false);
                hasExitButton = false;
                break;
            default:
                isConfirmation = false;
                hasExitButton = false;
                break;
        }

        if (isConfirmation)
        {
            popupButton1.gameObject.SetActive(true);
            popupButton2.gameObject.SetActive(true);

            popupButton2.Clicked += CancelButtonClicked;
            popupButton1.Clicked += ConfirmButtonClicked;
        }
        else
        {
            popupButton1.gameObject.SetActive(true);
            popupButton2.gameObject.SetActive(false);

            popupButton2.Clicked += ConfirmButtonClicked;
        }

        buttonClose.gameObject.SetActive(hasExitButton);

        if (hasExitButton)
            inputReader.MenuCloseEvent += CloseMenuButtonClicked;
    }

    public void CloseMenuButtonClicked() => ClosePanelAction?.Invoke();
    private void ConfirmButtonClicked() => ConfirmationResponseAction?.Invoke(true);
    private void CancelButtonClicked() => ConfirmationResponseAction?.Invoke(false);
}