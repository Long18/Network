using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine.Serialization;

/// <summary>
/// An Action to clear a <see cref="Protagonist.movementVector"/> at the <see cref="StateAction.SpecificMoment"/> <see cref="StopMovementActionSO.Moment"/>
/// </summary>
[CreateAssetMenu(fileName = "StopMovementAction", menuName = "State Machines/Actions/Stop Movement")]
public class StopMovementActionSO : StateActionSO
{
    [FormerlySerializedAs("_moment")] [SerializeField]
    private StateAction.SpecificMoment moment = default;

    public StateAction.SpecificMoment Moment => moment;

    protected override StateAction CreateAction() => new StopMovement();
}

public class StopMovement : StateAction
{
    private Protagonist protagonist;
    private new StopMovementActionSO OriginSO => (StopMovementActionSO)base.OriginSO;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonist = stateMachine.GetComponent<Protagonist>();
    }

    public override void OnUpdate()
    {
        if (OriginSO.Moment == SpecificMoment.OnUpdate)
            protagonist.movementVector = Vector3.zero;
    }

    public override void OnStateEnter()
    {
        if (OriginSO.Moment == SpecificMoment.OnStateEnter)
            protagonist.movementVector = Vector3.zero;
    }

    public override void OnStateExit()
    {
        if (OriginSO.Moment == SpecificMoment.OnStateExit)
            protagonist.movementVector = Vector3.zero;
    }
}