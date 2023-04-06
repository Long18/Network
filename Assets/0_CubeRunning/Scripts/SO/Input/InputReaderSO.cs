using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Input Reader", menuName = "Data/Input System/Input Reader")]
public class InputReaderSO : DescriptionBaseSO, GameInput.IGameplayActions
{
    [Space] [SerializeField] private GameStateSO gameStateManager;

    // Assign delegate{} to events to initialise them with an empty delegate
    // so we can skip the null check when we use them
    //Gameplay
    public event UnityAction JumpEvent = delegate { };
    public event UnityAction JumpCanceledEvent = delegate { };
    public event UnityAction ClimbEvent = delegate { };
    public event UnityAction ClimbCanceledEvent = delegate { };
    public event UnityAction AttackEvent = delegate { };
    public event UnityAction AttackCanceledEvent = delegate { };
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<Vector2, bool> CameraMoveEvent = delegate { };
    public event UnityAction EnableMouseControlCameraEvent = delegate { };
    public event UnityAction DisableMouseControlCameraEvent = delegate { };
    public event UnityAction StartedRunning = delegate { };
    public event UnityAction StoppedRunning = delegate { };

    //UI
    public event UnityAction interactEvent = delegate { }; // Used to talk, pickup objects, interact with tools
    public event UnityAction extraActionEvent = delegate { }; // Used to bring up the inventory
    public event UnityAction pauseEvent = delegate { };

    private GameInput gameInput;

    private void OnEnable()
    {
        if (gameInput != null) return;

        gameInput = new GameInput();
        gameInput.Gameplay.SetCallbacks(this);
        // UI
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                AttackEvent?.Invoke();
                break;
            case InputActionPhase.Canceled:
                AttackCanceledEvent?.Invoke();
                break;
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (MoveEvent == null) return;
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (JumpEvent == null && context.phase != InputActionPhase.Performed) return;
        JumpEvent?.Invoke();

        if (JumpCanceledEvent == null && context.phase != InputActionPhase.Canceled) return;
        JumpCanceledEvent?.Invoke();
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (ClimbEvent == null && context.phase != InputActionPhase.Performed) return;
        ClimbEvent?.Invoke();

        if (ClimbCanceledEvent == null && context.phase != InputActionPhase.Canceled) return;
        ClimbCanceledEvent?.Invoke();
    }

    public void OnMouseControlCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            EnableMouseControlCameraEvent?.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            DisableMouseControlCameraEvent?.Invoke();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                StartedRunning?.Invoke();
                break;
            case InputActionPhase.Canceled:
                StoppedRunning?.Invoke();
                break;
        }
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        Debug.Log("Open Inventory");
    }

    public void OnRotateCamera(InputAction.CallbackContext context)
    {
        if (CameraMoveEvent == null) return;
        CameraMoveEvent.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
    }

    private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

    public void OnExtraAction(InputAction.CallbackContext context)
    {
        if (extraActionEvent == null && context.phase != InputActionPhase.Performed) return;
        extraActionEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (interactEvent == null && context.phase != InputActionPhase.Performed) return;
        interactEvent?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (pauseEvent == null && context.phase != InputActionPhase.Performed) return;
        pauseEvent?.Invoke();
    }


    public void EnableGameplayInput()
    {
        gameInput.Gameplay.Enable();
    }

    public void DisableAllInput()
    {
        gameInput.Gameplay.Disable();
    }
}