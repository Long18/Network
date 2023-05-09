using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIActionButton : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent buttonActionText = default;
    [SerializeField] private Button buttonAction = default;
    [SerializeField] private UIButtonPrompt buttonPromptSetter = default;
    [SerializeField] private InputReaderSO inputReader = default;

    public UnityAction Clicked;

    private bool _hasEvent = false;

    public void FillInventoryButton(ItemTypeSO itemType, bool isInteracable = true)
    {
        buttonAction.interactable = isInteracable;
        buttonActionText.StringReference = itemType.ActionName;

        bool isKeyboard = true;
        buttonPromptSetter.SetButtonPrompt(isKeyboard);
        if (isInteracable)
        {
            if (inputReader != null)
            {
                _hasEvent = true;
                inputReader.InventoryActionButtonEvent += ClickActionButton;
            }
        }
        else
        {
            if (inputReader != null)
                if (_hasEvent)
                    inputReader.InventoryActionButtonEvent -= ClickActionButton;
        }
    }

    public void ClickActionButton() => Clicked.Invoke();

    private void OnDisable()
    {
        if (inputReader != null)
            if (_hasEvent)
                inputReader.InventoryActionButtonEvent -= ClickActionButton;
    }
}