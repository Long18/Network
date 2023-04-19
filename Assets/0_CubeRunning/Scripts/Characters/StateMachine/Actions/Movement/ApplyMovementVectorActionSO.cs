using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyMovementVector", menuName = "State Machines/Actions/Apply Movement Vector")]
public class ApplyMovementVectorActionSO : StateActionSO<ApplyMovementVecTorAction>
{
}

public class ApplyMovementVecTorAction : StateAction
{
    private Protagonist protagonist;
    private CharacterController characterController;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonist = stateMachine.GetComponent<Protagonist>();
        characterController = stateMachine.GetComponent<CharacterController>();
    }

    public override void OnUpdate()
    {
        characterController.Move(protagonist.movementVector * Time.deltaTime);
        protagonist.movementVector = characterController.velocity;
    }
}