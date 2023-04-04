using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/Started Moving")]
public class IsMovingConditionSO : StateConditionSO<IsMovingCondition>
{
    public float value = 0.02f;
}

public class IsMovingCondition : Condition
{
    private Protagonist protagonist;
    private IsMovingConditionSO originSO => (IsMovingConditionSO)base.OriginSO; // The SO this Condition spawned from

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonist = stateMachine.GetComponent<Protagonist>();
    }

    protected override bool Statement()
    {
        Vector3 movementVector = protagonist.movementInput;
        movementVector.y = 0f;
        return movementVector.sqrMagnitude > originSO.value;
    }

    public override void OnStateExit()
    {
        protagonist.movementVector = Vector3.zero;
    }
}