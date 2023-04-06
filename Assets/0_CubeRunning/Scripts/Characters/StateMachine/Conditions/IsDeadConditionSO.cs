using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsDeadCondition", menuName = "State Machines/Conditions/Is Dead")]
public class IsDeadConditionSO : StateConditionSO
{
    protected override Condition CreateCondition() => new IsDeadCondition();
}

public class IsDeadCondition : Condition
{
    private Damageable damageableScript;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        damageableScript = stateMachine.GetComponent<Damageable>();
    }

    protected override bool Statement()
    {
        return damageableScript.IsDead;
    }
}