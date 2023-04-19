using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "HorizontalMove", menuName = "State Machines/Actions/Horizontal Move")]
public class HorizontalMoveActionSO : StateActionSO<HorizontalMoveAction>
{
    [Tooltip("Horizontal XZ plane speed multiplier")]
    public float speed = 8f;
}

public class HorizontalMoveAction : StateAction
{
    //Component references
    private Protagonist protagonistScript;

    private HorizontalMoveActionSO originSO =>
        (HorizontalMoveActionSO)base.OriginSO; // The SO this StateAction spawned from

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonistScript = stateMachine.GetComponent<Protagonist>();
    }

    public override void OnUpdate()
    {
        protagonistScript.movementVector.x = protagonistScript.movementInput.x * originSO.speed;
        protagonistScript.movementVector.z = protagonistScript.movementInput.z * originSO.speed;
    }
}