using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
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
    [SerializeField] private UIGenericButton confirmButton = default;
    [SerializeField] private UIGenericButton cancelButton = default;
    [SerializeField] private InputReaderSO inputReader = default;

    private ConfirmationType actualType;

    public event UnityAction<bool> ConfirmationResponseAction = delegate { };
    public event UnityAction ClosePanelAction = delegate { };

    private void OnDisable()
    {
        cancelButton.Clicked -= CancelButtonClicked;
        confirmButton.Clicked -= ConfirmButtonClicked;
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

                confirmButton.SetButton(tableEntryReferenceConfirm, true);
                cancelButton.SetButton(tableEntryReferenceCancel, false);
                hasExitButton = true;
                break;
            case ConfirmationType.Quit:
                isConfirmation = true;

                confirmButton.SetButton(tableEntryReferenceConfirm, true);
                cancelButton.SetButton(tableEntryReferenceCancel, false);
                hasExitButton = false;
                break;
            default:
                isConfirmation = false;
                hasExitButton = false;
                break;
        }

        if (isConfirmation)
        {
            confirmButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);

            confirmButton.Clicked += ConfirmButtonClicked;
            cancelButton.Clicked += CancelButtonClicked;
        }
        else
        {
            confirmButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(false);

            confirmButton.Clicked += ConfirmButtonClicked;
        }

        buttonClose.gameObject.SetActive(hasExitButton);

        if (hasExitButton)
            inputReader.MenuCloseEvent += CloseMenuButtonClicked;
    }

    public void CloseMenuButtonClicked() => ClosePanelAction.Invoke();
    private void ConfirmButtonClicked() => ConfirmationResponseAction.Invoke(true);
    private void CancelButtonClicked() => ConfirmationResponseAction.Invoke(false);
}