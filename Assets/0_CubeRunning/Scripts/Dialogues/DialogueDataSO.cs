using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor.Localization;
using UnityEditor;
#endif

public enum DialogueType
{
    StartDialogue,
    CompletionDialogue,
    IncompletionDialogue,
    DefaultDialogue,
}

public enum ChoiceActionType
{
    DoNothing,
    ContinueWithStep,
    WinningChoice,
    LosingChoice,
    IncompleteStep
}

/// <summary>
/// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
/// In future versions it might contain support for branching conversations.
/// </summary>
[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogues/Dialogue Data")]
public class DialogueDataSO : ScriptableObject
{
    [SerializeField] private List<Line> lines = default;
    [SerializeField] private DialogueType dialogueType = default;
    [SerializeField] private VoidEventChannelSO endOfDialogueEvent = default;

    public VoidEventChannelSO EndOfDialogueEvent => endOfDialogueEvent;
    public List<Line> Lines => lines;

    public DialogueType DialogueType
    {
        get { return dialogueType; }
        set { dialogueType = value; }
    }

    public void FinishDialogue()
    {
        if (EndOfDialogueEvent != null)
            EndOfDialogueEvent.RaiseEvent();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        SetDialogueLines(this.name);
    }

    public DialogueDataSO(string dialogueName)
    {
        SetDialogueLines(dialogueName);
    }

    void SetDialogueLines(string dialogueName)
    {
        if (lines == null)
            lines = new List<Line>();

        lines.Clear();
        int dialogueIndex = 0;
        Line dialogueLine = new Line();

        do
        {
            dialogueIndex++;
            dialogueLine = new Line("D" + dialogueIndex + "-" + dialogueName);
            if (dialogueLine.TextList != null)
                lines.Add(dialogueLine);
        } while (dialogueLine.TextList != null);
    }

    /// <summary>
    /// This function is only useful for the Questline Tool in Editor to remove a Questline
    /// </summary>
    /// <returns>The local path</returns>
    public string GetPath()
    {
        return AssetDatabase.GetAssetPath(this);
    }
#endif
}

[Serializable]
public class Choice
{
    [SerializeField] private LocalizedString response = default;
    [SerializeField] private DialogueDataSO nextDialogue = default;
    [SerializeField] private ChoiceActionType actionType = default;

    public LocalizedString Response => response;
    public DialogueDataSO NextDialogue => nextDialogue;
    public ChoiceActionType ActionType => actionType;

    public void SetNextDialogue(DialogueDataSO dialogue)
    {
        nextDialogue = dialogue;
    }

    public Choice(Choice choice)
    {
        response = choice.Response;
        nextDialogue = choice.NextDialogue;
        actionType = ActionType;
    }

    public Choice(LocalizedString response)
    {
        this.response = response;
    }

    public void SetChoiceAction(Comment comment)
    {
        actionType = (ChoiceActionType)Enum.Parse(typeof(ChoiceActionType), comment.CommentText);
    }
}

[Serializable]
public class Line
{
    [SerializeField] private ActorID actorID = default;
    [SerializeField] private List<LocalizedString> textList = default;
    [SerializeField] private List<Choice> choices = null;

    public ActorID Actor => actorID;
    public List<LocalizedString> TextList => textList;
    public List<Choice> Choices => choices;

    public Line()
    {
        textList = null;
    }

    public void SetActor(Comment comment)
    {
        actorID = (ActorID)Enum.Parse(typeof(ActorID), comment.CommentText);
    }

#if UNITY_EDITOR
    public Line(string _name)
    {
        StringTableCollection collection = LocalizationEditorSettings.GetStringTableCollection("Dialogue");
        textList = null;
        if (collection != null)
        {
            int lineIndex = 0;
            LocalizedString _dialogueLine = null;
            do
            {
                lineIndex++;
                string key = "L" + lineIndex + "-" + _name;
                if (collection.SharedData.Contains(key))
                {
                    SetActor(collection.SharedData.GetEntry(key).Metadata.GetMetadata<Comment>());
                    _dialogueLine = new LocalizedString()
                        { TableReference = "Dialogue", TableEntryReference = key };
                    if (textList == null)
                        textList = new List<LocalizedString>();
                    textList.Add(_dialogueLine);
                }
                else
                {
                    _dialogueLine = null;
                }  
            } while (_dialogueLine != null);

            int choiceIndex = 0;
            Choice choice = null;
            do
            {
                choiceIndex++;
                string key = "C" + choiceIndex + "-" + _name;

                if (collection.SharedData.Contains(key))
                {
                    LocalizedString _choiceLine = new LocalizedString()
                        { TableReference = "Dialogue", TableEntryReference = key };
                    choice = new Choice(_choiceLine);
                    choice.SetChoiceAction(collection.SharedData.GetEntry(key).Metadata.GetMetadata<Comment>());

                    if (choices == null)
                    {
                        choices = new List<Choice>();
                    }

                    choices.Add(choice);
                }
                else
                {
                    choice = null;
                }
            } while (choice != null);
        }
        else
        {
            textList = null;
        }
    }
#endif
}