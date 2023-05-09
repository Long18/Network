using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

/// <summary>
/// Takes care of all things dialogue, whether they are coming from within a Timeline or just from the interaction with a character, or by any other mean.
/// Keeps track of choices in the dialogue (if any) and then gives back control to gameplay when appropriate.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<ActorSO> actorsList = default;
    [SerializeField] private InputReaderSO inputReader = default;
    [SerializeField] private GameStateSO gameState = default;

    [Header("Listening on")] [SerializeField]
    private DialogueDataChannelSO startDialogue = default;

    [SerializeField] private DialogueChoiceChannelSO makeDialogueChoiceEvent = default;

    [Header("Broadcasting on")] [SerializeField]
    private DialogueLineChannelSO openUIDialogueEvent = default;

    [SerializeField] private DialogueChoicesChannelSO showChoicesUIEvent = default;
    [SerializeField] private IntEventChannelSO endDialogueWithTypeEvent = default;
    [SerializeField] private VoidEventChannelSO continueWithStep = default;
    [SerializeField] private VoidEventChannelSO playIncompleteDialogue = default;
    [SerializeField] private VoidEventChannelSO makeWinningChoice = default;
    [SerializeField] private VoidEventChannelSO makeLosingChoice = default;

    private int counterDialogue;
    private int counterLine;

    private bool reachedEndOfDialogue
    {
        get => counterDialogue >= currentDialogue.Lines.Count;
    }

    private bool reachedEndOfLine
    {
        get => counterLine >= currentDialogue.Lines[counterDialogue].TextList.Count;
    }

    private DialogueDataSO currentDialogue = default;

    private void Start()
    {
        startDialogue.OnEventRaised += DisplayDialogueData;
    }

    /// <summary>
    /// Displays DialogueData in the UI, one by one.
    /// </summary>
    public void DisplayDialogueData(DialogueDataSO dialogueDataSO)
    {
        if (gameState.CurrentGameState != GameState.Cutscene) // the dialogue state is implied in the cutscene state
            gameState.UpdateGameState(GameState.Dialogue);

        counterDialogue = 0;
        counterLine = 0;
        inputReader.EnableDialogueInput();
        inputReader.AdvanceDialogueEvent += OnAdvance;
        currentDialogue = dialogueDataSO;

        if (currentDialogue.Lines != null)
        {
            ActorSO currentActor =
                actorsList.Find(o =>
                    o.ActorID ==
                    currentDialogue.Lines[counterDialogue]
                        .Actor); // we don't add a controle, because we need a null reference exeption if the actor is not in the list
            DisplayDialogueLine(currentDialogue.Lines[counterDialogue].TextList[counterLine], currentActor);
        }
        else
        {
            Debug.LogError("Check Dialogue");
        }
    }

    /// <summary>
    /// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
    /// This function is also called by <c>DialogueBehaviour</c> from clips on Timeline during cutscenes.
    /// </summary>
    /// <param name="dialogueLine"></param>
    public void DisplayDialogueLine(LocalizedString dialogueLine, ActorSO actor)
    {
        openUIDialogueEvent.RaiseEvent(dialogueLine, actor);
    }

    private void OnAdvance()
    {
        counterLine++;
        if (!reachedEndOfLine)
        {
            ActorSO currentActor =
                actorsList.Find(o =>
                    o.ActorID ==
                    currentDialogue.Lines[counterDialogue]
                        .Actor); // we don't add a controle, because we need a null reference exeption if the actor is not in the list
            DisplayDialogueLine(currentDialogue.Lines[counterDialogue].TextList[counterLine], currentActor);
        }
        else if (currentDialogue.Lines[counterDialogue].Choices != null
                 && currentDialogue.Lines[counterDialogue].Choices.Count > 0)
        {
            if (currentDialogue.Lines[counterDialogue].Choices.Count > 0)
            {
                DisplayChoices(currentDialogue.Lines[counterDialogue].Choices);
            }
        }
        else
        {
            counterDialogue++;
            if (!reachedEndOfDialogue)
            {
                counterLine = 0;

                ActorSO currentActor =
                    actorsList.Find(o =>
                        o.ActorID ==
                        currentDialogue.Lines[counterDialogue]
                            .Actor); // we don't add a controle, because we need a null reference exeption if the actor is not in the list
                DisplayDialogueLine(currentDialogue.Lines[counterDialogue].TextList[counterLine], currentActor);
            }
            else
            {
                DialogueEndedAndCloseDialogueUI();
            }
        }
    }

    private void DisplayChoices(List<Choice> choices)
    {
        inputReader.AdvanceDialogueEvent -= OnAdvance;

        makeDialogueChoiceEvent.OnEventRaised += MakeDialogueChoice;
        showChoicesUIEvent.RaiseEvent(choices);
    }

    private void MakeDialogueChoice(Choice choice)
    {
        makeDialogueChoiceEvent.OnEventRaised -= MakeDialogueChoice;

        switch (choice.ActionType)
        {
            case ChoiceActionType.ContinueWithStep:
                if (continueWithStep != null)
                    continueWithStep.RaiseEvent();
                if (choice.NextDialogue != null)
                    DisplayDialogueData(choice.NextDialogue);
                break;

            case ChoiceActionType.WinningChoice:
                if (makeWinningChoice != null)
                    makeWinningChoice.RaiseEvent();
                break;

            case ChoiceActionType.LosingChoice:
                if (makeLosingChoice != null)
                    makeLosingChoice.RaiseEvent();
                break;

            case ChoiceActionType.DoNothing:
                if (choice.NextDialogue != null)
                    DisplayDialogueData(choice.NextDialogue);
                else
                    DialogueEndedAndCloseDialogueUI();
                break;

            case ChoiceActionType.IncompleteStep:
                if (playIncompleteDialogue != null)
                    playIncompleteDialogue.RaiseEvent();
                if (choice.NextDialogue != null)
                    DisplayDialogueData(choice.NextDialogue);
                break;
        }
    }

    public void CutsceneDialogueEnded()
    {
        if (endDialogueWithTypeEvent != null)
            endDialogueWithTypeEvent.RaiseEvent((int)DialogueType.DefaultDialogue);
    }

    private void DialogueEndedAndCloseDialogueUI()
    {
        //raise the special event for end of dialogue if any 
        currentDialogue.FinishDialogue();

        //raise end dialogue event 
        if (endDialogueWithTypeEvent != null)
            endDialogueWithTypeEvent.RaiseEvent((int)currentDialogue.DialogueType);

        inputReader.AdvanceDialogueEvent -= OnAdvance;
        gameState.ReturnToPreviousGameState();

        if (gameState.CurrentGameState == GameState.Gameplay
            || gameState.CurrentGameState == GameState.Combat)
            inputReader.EnableGameplayInput();
    }
}