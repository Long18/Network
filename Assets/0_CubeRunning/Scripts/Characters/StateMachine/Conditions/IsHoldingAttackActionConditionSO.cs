using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Holding Attack Action")]
public class IsHoldingAttackActionConditionSO : StateConditionSO<IsHoldingAttackActionCondition>
{
}

public class IsHoldingAttackActionCondition : Condition
{
    //Component references
    private Protagonist protagonistScript;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonistScript = stateMachine.GetComponent<Protagonist>();
    }

    protected override bool Statement()
    {
        return protagonistScript.attackInput;
    }
}