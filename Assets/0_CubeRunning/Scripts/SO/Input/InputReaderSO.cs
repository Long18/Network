using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Input Reader", menuName = "Data/Input System/Input Reader")]
public class InputReaderSO : DescriptionBaseSO, GameInput.IGameplayActions, GameInput.IMenusActions
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
    public event UnityAction InteractEvent = delegate { };
    public event UnityAction InventoryActionButtonEvent = delegate { };
    public event UnityAction SaveActionButtonEvent = delegate { };
    public event UnityAction ResetActionButtonEvent = delegate { };

    // Shared between menus and dialogues
    public event UnityAction MoveSelectionEvent = delegate { };

    // Menus
    public event UnityAction MenuMouseMoveEvent = delegate { };
    public event UnityAction MenuClickButtonEvent = delegate { };
    public event UnityAction MenuUnpauseEvent = delegate { };
    public event UnityAction MenuPauseEvent = delegate { };
    public event UnityAction MenuCloseEvent = delegate { };
    public event UnityAction OpenInventoryEvent = delegate { }; // Used to bring up the inventory
    public event UnityAction CloseInventoryEvent = delegate { }; // Used to bring up the inventory
    public event UnityAction<float> TabSwitched = delegate { };

    private GameInput gameInput;

    private void OnEnable()
    {
        if (gameInput != null) return;

        gameInput = new GameInput();
        gameInput.Menus.SetCallbacks(this);
        gameInput.Gameplay.SetCallbacks(this);
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
        if (context.phase != InputActionPhase.Performed) return;
    }

    public void OnRotateCamera(InputAction.CallbackContext context)
    {
        if (CameraMoveEvent == null) return;
        CameraMoveEvent?.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
    }

    private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (gameStateManager.CurrentGameState == GameState.Gameplay &&
            context.phase != InputActionPhase.Performed) return;
        InteractEvent?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        MenuPauseEvent?.Invoke();
    }


    public void OnMoveSelection(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        MoveSelectionEvent.Invoke();
    }


    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        MenuClickButtonEvent.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        MenuCloseEvent?.Invoke();
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        MenuMouseMoveEvent?.Invoke();
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        MenuUnpauseEvent?.Invoke();
    }

    public void OnChangeTab(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        TabSwitched?.Invoke(context.ReadValue<float>());
    }

    public void OnInventoryActionButton(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        InventoryActionButtonEvent?.Invoke();
    }

    public void OnSaveActionButton(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        SaveActionButtonEvent?.Invoke();
    }

    public void OnResetActionButton(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        ResetActionButtonEvent?.Invoke();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
    }

    public void OnCloseInventory(InputAction.CallbackContext context) => CloseInventoryEvent?.Invoke();


    public void EnableGameplayInput()
    {
        gameInput.Menus.Disable();
        gameInput.Gameplay.Enable();
    }

    public void EnableMenuInput()
    {
        gameInput.Menus.Enable();
        gameInput.Gameplay.Disable();
    }

    public void DisableAllInput()
    {
        gameInput.Gameplay.Disable();
        gameInput.Menus.Disable();
    }
}