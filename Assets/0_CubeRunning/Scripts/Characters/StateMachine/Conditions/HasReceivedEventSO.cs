using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "State Machines/Conditions/Has Received Event")]
public class HasReceivedEventSO : StateConditionSO<HasReceivedEventCondition>
{
    public VoidEventChannelSO VoidEvent;
}

public class HasReceivedEventCondition : Condition
{
    private HasReceivedEventSO originSO => (HasReceivedEventSO)base.OriginSO; // The SO this Condition spawned from

    private bool eventTriggered;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        eventTriggered = false;
        originSO.VoidEvent.OnEventRaised += EventReceived;
    }

    protected override bool Statement()
    {
        return eventTriggered;
    }

    private void EventReceived()
    {
        eventTriggered = true;
    }

    public override void OnStateExit()
    {
        eventTriggered = false;
    }
}