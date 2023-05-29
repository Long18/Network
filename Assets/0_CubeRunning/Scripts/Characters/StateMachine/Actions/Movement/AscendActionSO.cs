using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Ascend", menuName = "State Machines/Actions/Ascend")]
public class AscendActionSO : StateActionSO<AscendAction>
{
    [Tooltip(
        "The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")]
    public float initialJumpForce = 6f;

    public AscendActionType ascendActionType = AscendActionType.None;
}

public class AscendAction : StateAction
{
    //Component references
    private Protagonist protagonistScript;

    private float verticalMovement;
    private float gravityContributionMultiplier;

    private AscendActionSO originSO => (AscendActionSO)base.OriginSO; // The SO this StateAction spawned from

    private AscendActionType ascendActionType => originSO.ascendActionType;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonistScript = stateMachine.GetComponent<Protagonist>();
    }

    public override void OnStateEnter()
    {
        verticalMovement = originSO.initialJumpForce;
    }

    public override void OnUpdate()
    {
        gravityContributionMultiplier += Protagonist.GRAVITY_COMEBACK_MULTIPLIER;
        gravityContributionMultiplier *= Protagonist.GRAVITY_DIVIDER; //Reduce the gravity effect

        //Note that deltaTime is used even though it's going to be used in ApplyMovementVectorAction, this is because it represents an acceleration, not a speed
        verticalMovement += Physics.gravity.y * Protagonist.GRAVITY_MULTIPLIER * gravityContributionMultiplier *
                            Time.deltaTime;

        //Note that even if it's added, the above value is negative due to Physics.gravity.y

        protagonistScript.movementVector.y = verticalMovement;
    }
}

public enum AscendActionType
{
    None = 0,
    Climbing = 1,
    Jumping = 2,
}