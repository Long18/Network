using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Protagonist : MonoBehaviour
{
    [SerializeField] private InputReaderSO inputReader = default;
    public Transform gameplayCamera;

    private Vector2 previousMovementInput;

    public bool jumpInput;
    public bool extraActionInput;
    public Vector3 movementInput;
    public Vector3 movementVector;
    public ControllerColliderHit lastHit;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        lastHit = hit;
    }

    private void OnEnable()
    {
        inputReader.jumpEvent += OnJumpInitiated;
        inputReader.jumpCanceledEvent += OnJumpCanceled;
        inputReader.moveEvent += OnMove;
        inputReader.extraActionEvent += OnExtraAction;
    }

    private void OnDisable()
    {
        inputReader.jumpEvent -= OnJumpInitiated;
        inputReader.jumpCanceledEvent -= OnJumpCanceled;
        inputReader.moveEvent -= OnMove;
        inputReader.extraActionEvent -= OnExtraAction;
    }

    private void Update()
    {
        RecalculateMovement();
    }

    private void RecalculateMovement()
    {
        Vector3 cameraForward = gameplayCamera.forward;
        cameraForward.y = 0f;
        Vector3 cameraRight = gameplayCamera.right;
        cameraRight.y = 0f;

        Vector3 adjustedMovement = cameraRight.normalized * previousMovementInput.x +
                                   cameraForward.normalized * previousMovementInput.y;

        movementInput = Vector3.ClampMagnitude(adjustedMovement, 1f);
    }


    private void OnJumpInitiated()
    {
        jumpInput = true;
    }

    private void OnJumpCanceled()
    {
        jumpInput = false;
    }

    private void OnMove(Vector2 movement)
    {
        previousMovementInput = movement;
    }

    private void OnExtraAction()
    {
        extraActionInput = true;
    }
}