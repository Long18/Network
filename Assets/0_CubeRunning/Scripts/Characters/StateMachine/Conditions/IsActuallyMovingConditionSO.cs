using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "IsActuallyMoving", menuName = "State Machines/Conditions/Is Actually Moving")]
public class IsActuallyMovingConditionSO : StateConditionSO
{
    [SerializeField] private float value = 0.02f;

    protected override Condition CreateCondition() => new IsActuallyMovingCondition(value);
}

public class IsActuallyMovingCondition : Condition
{
    private float value;
    private CharacterController characterController;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        characterController = stateMachine.GetComponent<CharacterController>();
    }

    public IsActuallyMovingCondition(float _value)
    {
        this.value = _value;
    }

    protected override bool Statement()
    {
        return characterController.velocity.sqrMagnitude > value * value;
    }
}