using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Holding Jump")]
public class IsHoldingJumpConditionSO : StateConditionSO<IsHoldingJumpCondition>
{
}

public class IsHoldingJumpCondition : Condition
{
    //Component references
    private Protagonist _protagonistScript;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        _protagonistScript = stateMachine.GetComponent<Protagonist>();
    }

    protected override bool Statement() => _protagonistScript.jumpInput;
}