using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Data")] [SerializeField] private ActorSO actor = default;

    [SerializeField] private DialogueDataSO defaultDialogue = default;

    // [SerializeField] private QuestManagerSO questData = default; In current game we don't need this
    [SerializeField] private GameStateSO gameStateManager = default;

    [Header("Listen Event")] [SerializeField]
    private VoidEventChannelSO winDialogueEvent = default;

    [SerializeField] private VoidEventChannelSO loseDialogueEvent = default;
    [SerializeField] private IntEventChannelSO endDialogueEvent = default;

    [Header("Boardcast Event")] [SerializeField]
    private DialogueDataChannelSO startDialogueEvent = default;

    [Header("Dialogue Shot Camera")] public GameObject dialogueShot;

    private DialogueDataSO currentDialogue;

    public bool IsInDialogue; //Consumed by the state machine

    private void Start()
    {
        if (!dialogueShot) return;

        dialogueShot.transform.parent = null;
        dialogueShot.SetActive(false);
    }

    private void PlayDefaultDialogue()
    {
        if (defaultDialogue == null) return;

        currentDialogue = defaultDialogue;
        StartDialogue();
    }

    /// <summary>
    /// Start a dialogue when interaction with NPC
    /// Some step need to be instance and do not need the interact button
    /// when interaction again, restart same dialogue
    /// </summary>
    public void InteractWithCharacter()
    {
        if (gameStateManager.CurrentGameState != GameState.Gameplay) return;

        DialogueDataSO
            displayDialogue =
                default; // TODO: questData.InteractWithCharacter(actor, false, false); -> If we need this, we need to create a QuestManagerSO
        if (displayDialogue != null)
        {
            currentDialogue = defaultDialogue;
            StartDialogue();
        }
        else
        {
            PlayDefaultDialogue();
        }
    }

    private void StartDialogue()
    {
        startDialogueEvent.RaiseEvent(currentDialogue);

        endDialogueEvent.OnEventRaised += EndDialogue;
        SetConversation(false);
        winDialogueEvent.OnEventRaised += WinDialogue;
        loseDialogueEvent.OnEventRaised += LoseDialogue;

        IsInDialogue = true;
        if (dialogueShot) dialogueShot.SetActive(true);
    }

    private void EndDialogue(int type)
    {
        endDialogueEvent.OnEventRaised -= EndDialogue;
        winDialogueEvent.OnEventRaised -= WinDialogue;
        loseDialogueEvent.OnEventRaised -= LoseDialogue;
        SetConversation(true);

        IsInDialogue = false;
        if (dialogueShot) dialogueShot.SetActive(false);
    }

    private void LoseDialogue()
    {
        // if(questData != null) return; // TODO: If we need this, we need to create a QuestManagerSO

        DialogueDataSO
            displayDialogue =
                default; // TODO: questData.InteractWithCharacter(actor, false, true); -> If we need this, we need to create a QuestManagerSO

        if (displayDialogue == null) return;

        currentDialogue = displayDialogue;
        StartDialogue();
    }

    private void WinDialogue()
    {
        // if(questData != null) return; // TODO: If we need this, we need to create a QuestManagerSO

        DialogueDataSO
            displayDialogue =
                default; // TODO: questData.InteractWithCharacter(actor, true, false); -> If we need this, we need to create a QuestManagerSO

        if (displayDialogue == null) return;

        currentDialogue = displayDialogue;
        StartDialogue();
    }

    private void SetConversation(bool isResumed)
    {
        GameObject[] talkingTo = gameObject.GetComponent<NPC>().talkingTo;

        if (talkingTo == null) return;

        NPCState state = isResumed ? NPCState.Idle : NPCState.Talk;

        for (int i = 0; i < talkingTo.Length; ++i)
        {
            talkingTo[i].GetComponent<NPC>().npcState = state;
        }
    }
}