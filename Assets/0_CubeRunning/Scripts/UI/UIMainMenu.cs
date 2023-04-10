using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton = default;
    [SerializeField] private Button continueSinglePlayerButton = default;

    public UnityAction SinglePlayerButtonAction;
    public UnityAction ContinueSinglePlayerButtonAction;
    public UnityAction MultiPlayerButtonAction;
    public UnityAction SettingsButtonAction;
    public UnityAction CreditsButtonAction;
    public UnityAction ExitButtonAction;

    public void SetMenuScreen(bool hasSaveData)
    {
        continueSinglePlayerButton.interactable = hasSaveData;
        if (hasSaveData)
        {
            continueSinglePlayerButton.Select();
        }
        else
        {
            singlePlayerButton.Select();
        }
    }

    public void SinglePlayerButton() => SinglePlayerButtonAction.Invoke();

    public void ContinueSinglePlayerButton() => ContinueSinglePlayerButtonAction.Invoke();

    public void MultiPlayerButton() => MultiPlayerButtonAction.Invoke();

    public void SettingsButton() => SettingsButtonAction.Invoke();

    public void CreditsButton() => CreditsButtonAction.Invoke();

    public void ExitButton() => ExitButtonAction.Invoke();
}