using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ResetHealth", menuName = "State Machines/Actions/Reset Health")]
public class ResetHealthSO : StateActionSO
{
    protected override StateAction CreateAction() => new ResetHealth();
}

public class ResetHealth : StateAction
{
    private Damageable damageableEntity;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        damageableEntity = stateMachine.GetComponent<Damageable>();
    }

    public override void OnUpdate()
    {
    }

    public override void OnStateExit()
    {
        damageableEntity.Revive();
    }
}