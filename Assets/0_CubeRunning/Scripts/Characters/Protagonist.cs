using System;
using Photon.Pun;
using UnityEngine;

// ReSharper disable All

public class Protagonist : MonoBehaviour
{
    [SerializeField] private InputReaderSO inputReader = default;

    [SerializeField] private TransformAnchor gameplayCameraTransform = default;

    [SerializeField] private PhotonView view = default;

    private Vector2 inputVector;
    private float previousSpeed;

    [NonSerialized] public bool jumpInput;
    [NonSerialized] public bool climbInput;
    [NonSerialized] public bool extraActionInput;
    [NonSerialized] public bool attackInput;
    [NonSerialized] public bool isRunning;
    [NonSerialized] public Vector3 movementInput;
    [NonSerialized] public Vector3 movementVector;
    [NonSerialized] public ControllerColliderHit lastHit;

    public const float GRAVITY_MULTIPLIER = 5f;
    public const float MAX_FALL_SPEED = -50f;
    public const float MAX_RISE_SPEED = 100f;
    public const float GRAVITY_COMEBACK_MULTIPLIER = .03f;
    public const float GRAVITY_DIVIDER = .6f;
    public const float AIR_RESISTANCE = 5f;


    private void OnControllerColliderHit(ControllerColliderHit hit) => lastHit = hit;

    private void OnEnable()
    {
        inputReader.MoveEvent += OnMove;
        inputReader.JumpEvent += OnJumpInitiated;
        inputReader.JumpCanceledEvent += OnJumpCanceled;
        inputReader.StartedRunning += OnStartedRunning;
        inputReader.StoppedRunning += OnStoppedRunning;
        inputReader.ClimbEvent += OnClimbingInitiated;
        inputReader.ClimbCanceledEvent += OnClimbingCanceled;
        inputReader.AttackEvent += OnStartedAttack;
    }

    private void OnDisable()
    {
        inputReader.MoveEvent -= OnMove;
        inputReader.JumpEvent -= OnJumpInitiated;
        inputReader.JumpCanceledEvent -= OnJumpCanceled;
        inputReader.StoppedRunning -= OnStoppedRunning;
        inputReader.StartedRunning -= OnStartedRunning;
        inputReader.ClimbEvent -= OnClimbingInitiated;
        inputReader.ClimbCanceledEvent -= OnClimbingCanceled;
        inputReader.AttackEvent -= OnStartedAttack;
    }

    private void Update() => RecalculateMovement();

    private void RecalculateMovement()
    {
#if !UNITY_EDITOR
        if (!view.IsMine) return;
#endif

        float targetSpeed;
        Vector3 adjustedMovement;

        if (gameplayCameraTransform.isSet)
        {
            //Get the two axes from the camera and flatten them on the XZ plane
            Vector3 cameraForward = gameplayCameraTransform.Value.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = gameplayCameraTransform.Value.right;
            cameraRight.y = 0f;

            //Use the two axes, modulated by the corresponding inputs, and construct the final vector
            adjustedMovement = cameraRight.normalized * inputVector.x +
                               cameraForward.normalized * inputVector.y;
        }
        else
        {
            //No CameraManager exists in the scene, so the input is just used absolute in world-space
            Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
            adjustedMovement = new Vector3(inputVector.x, 0f, inputVector.y);
        }

        //Fix to avoid getting a Vector3.zero vector, which would result in the player turning to x:0, z:0
        if (inputVector.sqrMagnitude == 0f)
            adjustedMovement = transform.forward * (adjustedMovement.magnitude + .01f);

        //Accelerate/decelerate
        targetSpeed = Mathf.Clamp01(inputVector.magnitude);
        if (targetSpeed > 0f)
        {
            // This is used to set the speed to the maximum if holding the Shift key,
            // to allow keyboard players to "run"
            if (isRunning) targetSpeed = 1f;
            if (attackInput) targetSpeed = .05f;
        }

        targetSpeed = Mathf.Lerp(previousSpeed, targetSpeed, Time.deltaTime * 4f);

        movementInput = adjustedMovement.normalized * targetSpeed;
        previousSpeed = targetSpeed;
    }


    private void OnMove(Vector2 movement) => inputVector = movement;

    private void OnJumpInitiated() => jumpInput = true;
    private void OnJumpCanceled() => jumpInput = false;
    private void OnStoppedRunning() => isRunning = false;
    private void OnStartedRunning() => isRunning = true;

    private void OnStartedAttack() => attackInput = true;

    // Triggered from Animation Event
    public void ConsumeAttackInput() => attackInput = false;

    private void OnClimbingInitiated() => climbInput = true;
    private void OnClimbingCanceled() => climbInput = false;
}