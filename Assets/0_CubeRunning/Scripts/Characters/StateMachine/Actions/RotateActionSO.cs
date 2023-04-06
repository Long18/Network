using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RotateAction", menuName = "State Machines/Actions/Rotate")]
public class RotateActionSO : StateActionSO<RotateAction>
{
    [Tooltip("Smoothing for rotating the character to their movement direction")]
    public float turnSmoothTime = 0.2f;
}

public class RotateAction : StateAction
{
    //Component references
    private Protagonist protagonistScript;
    private Transform transform;

    private float
        turnSmoothSpeed; //Used by Mathf.SmoothDampAngle to smoothly rotate the character to their movement direction

    private const float ROTATION_TRESHOLD = .02f; // Used to prevent NaN result causing rotation in a non direction
    private RotateActionSO originSO => (RotateActionSO)base.OriginSO; // The SO this StateAction spawned from

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonistScript = stateMachine.GetComponent<Protagonist>();
        transform = stateMachine.GetComponent<Transform>();
    }

    public override void OnUpdate()
    {
        Vector3 horizontalMovement = protagonistScript.movementVector;
        horizontalMovement.y = 0f;

        if (horizontalMovement.sqrMagnitude >= ROTATION_TRESHOLD)
        {
            float targetRotation =
                Mathf.Atan2(protagonistScript.movementVector.x, protagonistScript.movementVector.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetRotation,
                ref turnSmoothSpeed,
                originSO.turnSmoothTime);
        }
    }
}