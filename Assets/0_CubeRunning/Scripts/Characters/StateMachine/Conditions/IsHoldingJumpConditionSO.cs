using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Holding Jump")]
public class IsHoldingJumpConditionSO : StateConditionSO<IsHoldingJumpCondition>
{
}

public class IsHoldingJumpCondition : Condition
{
    //Component references
    private Protagonist protagonistScript;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonistScript = stateMachine.GetComponent<Protagonist>();
    }

    protected override bool Statement() => protagonistScript.jumpInput;
}