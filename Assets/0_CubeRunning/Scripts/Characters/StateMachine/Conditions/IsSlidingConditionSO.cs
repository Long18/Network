﻿using StateMachine;
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
        if (protagonistScript.lastHit == null)
            return false;

        float currentSlope = Vector3.Angle(Vector3.up, protagonistScript.lastHit.normal);
        return (currentSlope >= characterController.slopeLimit);
    }
}