﻿using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine.Serialization;

/// <summary>
/// This Action handles horizontal movement while in the air, keeping momentum, simulating air resistance, and accelerating towards the desired speed.
/// </summary>
[CreateAssetMenu(fileName = "AerialMovement", menuName = "State Machines/Actions/Aerial Movement")]
public class AerialMovementActionSO : StateActionSO
{
    public float Speed => speed;
    public float Acceleration => acceleration;

    [Tooltip("Desired horizontal movement speed while in the air")] [SerializeField] [Range(0.1f, 100f)]
    private float speed = 10f;

    [Tooltip("The acceleration applied to reach the desired speed")] [SerializeField] [Range(0.1f, 100f)]
    private float acceleration = 20f;

    protected override StateAction CreateAction() => new AerialMovementAction();
}

public class AerialMovementAction : StateAction
{
    private new AerialMovementActionSO OriginSO => (AerialMovementActionSO)base.OriginSO;

    private Protagonist protagonist;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonist = stateMachine.GetComponent<Protagonist>();
    }

    public override void OnUpdate()
    {
        Vector3 velocity = protagonist.movementVector;
        Vector3 input = protagonist.movementInput;
        float speed = OriginSO.Speed;
        float acceleration = OriginSO.Acceleration;

        SetVelocityPerAxis(ref velocity.x, input.x, acceleration, speed);
        SetVelocityPerAxis(ref velocity.z, input.z, acceleration, speed);

        protagonist.movementVector = velocity;
    }

    private void SetVelocityPerAxis(ref float currentAxisSpeed, float axisInput, float acceleration, float targetSpeed)
    {
        if (axisInput == 0f)
        {
            if (currentAxisSpeed != 0f)
            {
                ApplyAirResistance(ref currentAxisSpeed);
            }
        }
        else
        {
            (float absVel, float absInput) = (Mathf.Abs(currentAxisSpeed), Mathf.Abs(axisInput));
            (float signVel, float signInput) = (Mathf.Sign(currentAxisSpeed), Mathf.Sign(axisInput));
            targetSpeed *= absInput;

            if (signVel != signInput || absVel < targetSpeed)
            {
                currentAxisSpeed += axisInput * acceleration;
                currentAxisSpeed = Mathf.Clamp(currentAxisSpeed, -targetSpeed, targetSpeed);
            }
            else
            {
                ApplyAirResistance(ref currentAxisSpeed);
            }
        }
    }

    private void ApplyAirResistance(ref float value)
    {
        float sign = Mathf.Sign(value);

        value -= sign * Protagonist.AIR_RESISTANCE * Time.deltaTime;
        if (Mathf.Sign(value) != sign)
            value = 0;
    }
}