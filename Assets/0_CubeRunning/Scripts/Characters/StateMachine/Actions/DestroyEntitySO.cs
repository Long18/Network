using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "DestroyEntity", menuName = "State Machines/Actions/Destroy Entity")]
public class DestroyEntitySO : StateActionSO
{
    protected override StateAction CreateAction() => new DestroyEntity();
}

public class DestroyEntity : StateAction
{
    private GameObject gameObject;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        gameObject = stateMachine.gameObject;
    }

    public override void OnUpdate()
    {
    }

    public override void OnStateEnter()
    {
        GameObject.Destroy(gameObject);
    }
}