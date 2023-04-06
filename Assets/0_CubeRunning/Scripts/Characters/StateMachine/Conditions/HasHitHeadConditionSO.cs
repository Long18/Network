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
            // Making sure the collision is near the top of the head
            float permittedDistance = characterController.radius / 2f;
            float topPositionY = transform.position.y + characterController.height;
            float distance = Mathf.Abs(protagonistScript.lastHit.point.y - topPositionY);
            if (distance <= permittedDistance)
            {
                protagonistScript.jumpInput = false;
                protagonistScript.movementVector.y = 0f;

                return true;
            }
        }

        return false;
    }
}