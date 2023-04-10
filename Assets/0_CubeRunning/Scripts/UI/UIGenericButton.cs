using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class UIGenericButton : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent buttonText = default;
    [SerializeField] private MultiInputButton button = default;

    public UnityAction Clicked = default;

    private bool isDefaultSelection = false;

    private void OnDisable()
    {
        button.IsSelected = false;
        isDefaultSelection = false;
    }

    public void SetButton(bool isSelect)
    {
        isDefaultSelection = isSelect;
        if (isSelect)
            button.UpdateSelected();
    }

    public void SetButton(LocalizedString localizedString, bool isSelected)
    {
        buttonText.StringReference = localizedString;

        if (isSelected)
            SelectButton();
    }

    public void SetButton(string tableEntryReference, bool isSelected)
    {
        buttonText.StringReference.TableEntryReference = tableEntryReference;

        if (isSelected)
            SelectButton();
    }

    private void SelectButton() => button.Select();

    public void Click() => Clicked.Invoke();
}