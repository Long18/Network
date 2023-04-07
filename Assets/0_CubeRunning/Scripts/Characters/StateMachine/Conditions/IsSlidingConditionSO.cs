using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "IsSliding", menuName = "State Machines/Conditions/Is Sliding")]
public class IsSlidingConditionSO : StateConditionSO<IsSlidingCondition>
{
}

public class IsSlidingCondition : Condition
{
    private CharacterController characterController;
    private Protagonist protagonistScript;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        characterController = stateMachine.GetComponent<CharacterController>();
        protagonistScript = stateMachine.GetComponent<Protagonist>();
    }

    protected override bool Statement()
    {
        //First frame fail check
        if (protagonistScript.lastHit == null)
            return false;

        float stepHeight = protagonistScript.lastHit.point.y - protagonistScript.transform.position.y;
        bool isWalkableStep = stepHeight <= characterController.stepOffset;

        float currentSlope = Vector3.Angle(Vector3.up, protagonistScript.lastHit.normal);
        bool isSlopeTooSteep = currentSlope >= characterController.slopeLimit;

        if (!isSlopeTooSteep)
        {
            //Pendence is within slope limits
            return false;
        }
        else
        {
            //If the slope is too steep, we prevent sliding if it's within the step limit
            return !isWalkableStep;
        }
    }
}