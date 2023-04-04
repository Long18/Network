using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Ascend", menuName = "State Machines/Actions/Ascend")]
public class AscendActionSO : StateActionSO<AscendAction>
{
    [Tooltip(
        "The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")]
    public float initialJumpForce = 6f;
}

public class AscendAction : StateAction
{
    private Protagonist protagonist;

    private float verticalMovement;
    private float gravityContributionMultiplier;
    private const float GRAVITY_COMEBACK_MULTIPLIER = .03f;
    private const float GRAVITY_DIVIDER = .6f;
    private const float GRAVITY_MULTIPLIER = 5f;
    private AscendActionSO originSO => (AscendActionSO)base.OriginSO; // The SO this StateAction spawned from

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonist = stateMachine.GetComponent<Protagonist>();
    }

    public override void OnStateEnter()
    {
        verticalMovement = originSO.initialJumpForce;
    }

    public override void OnUpdate()
    {
        gravityContributionMultiplier += GRAVITY_COMEBACK_MULTIPLIER;
        gravityContributionMultiplier *= GRAVITY_DIVIDER; //Reduce the gravity effect
        verticalMovement += Physics.gravity.y * GRAVITY_MULTIPLIER * Time.deltaTime * gravityContributionMultiplier;
        //Note that even if it's added, the above value is negative due to Physics.gravity.y

        protagonist.movementVector.y = verticalMovement;
    }
}