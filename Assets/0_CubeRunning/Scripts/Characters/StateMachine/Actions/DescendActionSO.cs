using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Actions/Descend")]
public class DescendActionSO : StateActionSO<DescendAction>
{
}

public class DescendAction : StateAction
{
    //Component references
    private Protagonist protagonistScript;

    private float verticalMovement;
    private const float GRAVITY_MULTIPLIER = 5f;
    private const float MAX_FALL_SPEED = -50f;
    private const float MAX_RISE_SPEED = 100f;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonistScript = stateMachine.GetComponent<Protagonist>();
    }

    public override void OnStateEnter()
    {
        verticalMovement = protagonistScript.movementVector.y;

        //Prevents a double jump if the player keeps holding the jump button
        //Basically it "consumes" the input
        protagonistScript.jumpInput = false;
    }

    public override void OnUpdate()
    {
        verticalMovement += Physics.gravity.y * GRAVITY_MULTIPLIER * Time.deltaTime;
        //Note that even if it's added, the above value is negative due to Physics.gravity.y

        //Cap the maximum so the player doesn't reach incredible speeds when freefalling from high positions
        verticalMovement = Mathf.Clamp(verticalMovement, MAX_FALL_SPEED, MAX_RISE_SPEED);

        protagonistScript.movementVector.y = verticalMovement;
    }
}