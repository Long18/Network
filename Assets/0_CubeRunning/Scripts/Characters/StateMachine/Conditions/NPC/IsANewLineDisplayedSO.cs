using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "IsANewLineDisplayed", menuName = "State Machines/Conditions/Is A New Line Displayed")]
public class IsANewLineDisplayedSO : StateConditionSO
{
    [SerializeField] private DialogueLineChannelSO onLineDisplayed = default;

    protected override Condition CreateCondition() => new IsANewLineDisplayedCondition(onLineDisplayed);
}

public class IsANewLineDisplayedCondition : Condition
{
    private DialogueLineChannelSO sayLineEvent;
    private bool isAnewLineDisplayed = false;


    public IsANewLineDisplayedCondition(DialogueLineChannelSO sayLineEvent)
    {
        this.sayLineEvent = sayLineEvent;
    }

    protected override bool Statement()
    {
        return isAnewLineDisplayed;
    }

    public override void OnStateEnter()
    {
        if (sayLineEvent != null)
        {
            sayLineEvent.OnEventRaised += OnLineDisplayed;
        }
    }

    public override void OnStateExit()
    {
        if (sayLineEvent != null)
        {
            sayLineEvent.OnEventRaised -= OnLineDisplayed;
        }

        isAnewLineDisplayed = false;
    }

    private void OnLineDisplayed(LocalizedString line, ActorSO actor)
    {
        isAnewLineDisplayed = true;
    }
}