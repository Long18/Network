using System;
using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using Moment = StateMachine.StateAction.SpecificMoment;

/// <summary>
/// Flexible StateActionSO for the StateMachine which allows to set any parameter on the Animator, in any moment of the state (OnStateEnter, OnStateExit, or each OnUpdate).
/// </summary>
[CreateAssetMenu(fileName = "AnimatorMoveSpeedAction", menuName = "State Machines/Actions/Set Animator Move Speed")]
public class AnimatorMoveSpeedActionSO : StateActionSO
{
    public string parameterName = default;

    protected override StateAction CreateAction() => new AnimatorMoveSpeedAction(Animator.StringToHash(parameterName));
}

public class AnimatorMoveSpeedAction : StateAction
{
    //Component references
    private Animator animator;
    private Protagonist protagonist;

    private AnimatorParameterActionSO originSO =>
        (AnimatorParameterActionSO)base.OriginSO; // The SO this StateAction spawned from

    private int parameterHash;

    public AnimatorMoveSpeedAction(int parameterHash)
    {
        this.parameterHash = parameterHash;
    }

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        animator = stateMachine.GetComponent<Animator>();
        protagonist = stateMachine.GetComponent<Protagonist>();
    }

    public override void OnUpdate()
    {
        //TODO: do we like that we're using the magnitude here, per frame? Can this be done in a smarter way?
        float normalisedSpeed = protagonist.movementInput.magnitude;
        animator.SetFloat(parameterHash, normalisedSpeed);
    }
}