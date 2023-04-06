﻿using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ShakeCamAction", menuName = "State Machines/Actions/Shake Cam Action")]
public class ShakeCamActionSO : StateActionSO
{
    public VoidEventChannelSO camShakeEvent;
    protected override StateAction CreateAction() => new ShakeCamAction();
}

public class ShakeCamAction : StateAction
{
    protected new ShakeCamActionSO OriginSO => (ShakeCamActionSO)base.OriginSO;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
    }

    public override void OnUpdate()
    {
    }

    public override void OnStateEnter()
    {
        OriginSO.camShakeEvent.RaiseEvent();
    }

    public override void OnStateExit()
    {
    }
}