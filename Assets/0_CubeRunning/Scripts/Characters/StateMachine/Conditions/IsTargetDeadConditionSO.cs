using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsTargetDeadCondition", menuName = "State Machines/Conditions/Is Target Dead Condition")]
public class IsTargetDeadConditionSO : StateConditionSO
{
    protected override Condition CreateCondition() => new IsTargetDeadCondition();
}

public class IsTargetDeadCondition : Condition
{
    private Critter critterScript;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        critterScript = stateMachine.GetComponent<Critter>();
    }

    protected override bool Statement()
    {
        return critterScript.currentTarget == null || critterScript.currentTarget.IsDead;
    }
}