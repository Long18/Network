using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Character Controller Grounded")]
public class IsCharacterControllerGroundedConditionSO : StateConditionSO<IsCharacterControllerGroundedCondition>
{
}

public class IsCharacterControllerGroundedCondition : Condition
{
    private CharacterController characterController;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        characterController = stateMachine.GetComponent<CharacterController>();
    }

    protected override bool Statement() => characterController.isGrounded;
}