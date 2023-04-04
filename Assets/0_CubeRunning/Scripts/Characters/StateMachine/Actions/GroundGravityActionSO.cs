using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundGravity", menuName = "State Machines/Actions/Ground Gravity")]
public class GroundGravityActionSO : StateActionSO<GroundGravityAction>
{
    [Tooltip("Vertical movement pulling down the player to keep it anchored to the ground.")]
    public float verticalPull = -5f;
}

public class GroundGravityAction : StateAction
{
    private Protagonist protagonist;

    private GroundGravityActionSO originSO =>
        (GroundGravityActionSO)base.OriginSO; // The SO this StateAction spawned from

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonist = stateMachine.GetComponent<Protagonist>();
    }

    public override void OnUpdate()
    {
        protagonist.movementVector.y = originSO.verticalPull;
    }
}