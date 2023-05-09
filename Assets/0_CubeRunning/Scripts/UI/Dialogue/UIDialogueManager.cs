using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class UIDialogueManager : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent lineText = default;
    [SerializeField] private LocalizeStringEvent actorNameText = default;
    [SerializeField] private GameObject actorNamePanel = default;
    [SerializeField] private GameObject mainProtagonistNamePanel = default;
    [SerializeField] private UIDialogueChoicesManager choicesManager = default;

    [Header("Listening to")] [SerializeField]
    private DialogueChoicesChannelSO showChoicesEvent = default;

    private void OnEnable()
    {
        showChoicesEvent.OnEventRaised += ShowChoices;
    }

    private void OnDisable()
    {
        showChoicesEvent.OnEventRaised -= ShowChoices;
    }

    public void SetDialogue(LocalizedString dialogueLine, ActorSO actor, bool isMainProtagonist)
    {
        choicesManager.gameObject.SetActive(false);
        lineText.StringReference = dialogueLine;

        actorNamePanel.SetActive(!isMainProtagonist);
        mainProtagonistNamePanel.SetActive(isMainProtagonist);

        if (!isMainProtagonist)
        {
            actorNameText.StringReference = actor.ActorName;
        }
    }

    private void ShowChoices(List<Choice> choices)
    {
        choicesManager.FillChoices(choices);
        choicesManager.gameObject.SetActive(true);
    }

    private void HideChoices()
    {
        choicesManager.gameObject.SetActive(false);
    }
}