using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/Has Hit the Head")]
public class HasHitHeadConditionSO : StateConditionSO<HasHitHeadCondition>
{
}

public class HasHitHeadCondition : Condition
{
    //Component references
    private Protagonist protagonistScript;
    private CharacterController characterController;
    private Transform transform;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        transform = stateMachine.GetComponent<Transform>();
        protagonistScript = stateMachine.GetComponent<Protagonist>();
        characterController = stateMachine.GetComponent<CharacterController>();
    }

    protected override bool Statement()
    {
        bool isMovingUpwards = protagonistScript.movementVector.y > 0f;
        if (isMovingUpwards)
        {
            if (characterController.collisionFlags == CollisionFlags.Above)
            {
                protagonistScript.jumpInput = false;
                protagonistScript.movementVector.y = 0f;

                return true;
            }
        }

        return false;
    }
}