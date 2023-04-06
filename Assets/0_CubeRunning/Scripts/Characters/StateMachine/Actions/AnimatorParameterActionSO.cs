using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

/// <summary>
/// Flexible StateActionSO for the StateMachine which allows to set any parameter on the Animator, in any moment of the state (OnStateEnter, OnStateExit, or each OnUpdate).
/// </summary>
[CreateAssetMenu(fileName = "AnimatorParameterAction", menuName = "State Machines/Actions/Set Animator Parameter")]
public class AnimatorParameterActionSO : StateActionSO
{
    public ParameterType parameterType = default;
    public string parameterName = default;

    public bool boolValue = default;
    public int intValue = default;
    public float floatValue = default;

    public StateAction.SpecificMoment
        whenToRun = default; // Allows this StateActionSO type to be reused for all 3 state moments

    protected override StateAction CreateAction() => new AnimatorParameterAction(Animator.StringToHash(parameterName));

    public enum ParameterType
    {
        Bool,
        Int,
        Float,
        Trigger,
    }
}

public class AnimatorParameterAction : StateAction
{
    //Component references
    private Animator animator;

    private AnimatorParameterActionSO originSO =>
        (AnimatorParameterActionSO)base.OriginSO; // The SO this StateAction spawned from

    private int _parameterHash;

    public AnimatorParameterAction(int parameterHash)
    {
        _parameterHash = parameterHash;
    }

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        animator = stateMachine.GetComponent<Animator>();
    }

    public override void OnStateEnter()
    {
        if (originSO.whenToRun == SpecificMoment.OnStateEnter)
            SetParameter();
    }

    public override void OnStateExit()
    {
        if (originSO.whenToRun == SpecificMoment.OnStateExit)
            SetParameter();
    }

    private void SetParameter()
    {
        switch (originSO.parameterType)
        {
            case AnimatorParameterActionSO.ParameterType.Bool:
                animator.SetBool(_parameterHash, originSO.boolValue);
                break;
            case AnimatorParameterActionSO.ParameterType.Int:
                animator.SetInteger(_parameterHash, originSO.intValue);
                break;
            case AnimatorParameterActionSO.ParameterType.Float:
                animator.SetFloat(_parameterHash, originSO.floatValue);
                break;
            case AnimatorParameterActionSO.ParameterType.Trigger:
                animator.SetTrigger(_parameterHash);
                break;
        }
    }

    public override void OnUpdate()
    {
    }
}