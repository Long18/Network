using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "IsNPCSayingTheLine", menuName = "State Machines/Conditions/Is NPC Saying The Line")]
public class IsNPCSayingTheLineSO : StateConditionSO
{
    [SerializeField] private DialogueLineChannelSO onLineDisplayed = default;
    [SerializeField] private ActorSO protagonistActor;

    protected override Condition CreateCondition() =>
        new IsNPCSayingTheLineCondition(onLineDisplayed, protagonistActor);
}

public class IsNPCSayingTheLineCondition : Condition
{
    private DialogueLineChannelSO sayLineEvent;
    private ActorSO protagonistActor;
    private bool isNPCSayingTheLine = false;

    public IsNPCSayingTheLineCondition(DialogueLineChannelSO sayLineEvent, ActorSO protagonistActor)
    {
        this.sayLineEvent = sayLineEvent;
        this.protagonistActor = protagonistActor;
    }

    protected override bool Statement()
    {
        return isNPCSayingTheLine;
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
    }

    private void OnLineDisplayed(LocalizedString line, ActorSO actor)
    {
        if (actor.ActorName == protagonistActor.ActorName)
        {
            isNPCSayingTheLine = false;
        }
        else
        {
            isNPCSayingTheLine = true;
        }
    }
}