using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Input Reader", menuName = "Data/Input System/Input Reader")]
public class InputReaderSO : DescriptionBaseSO, GameInput.IGameplayActions, GameInput.IMenusActions,
    GameInput.IDialoguesActions
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

    // Shared between menus 
    public event UnityAction MoveSelectionEvent = delegate { };

    // Dialogues    
    public event UnityAction AdvanceDialogueEvent = delegate { };

    // Menus
    public event UnityAction MenuMouseMoveEvent = delegate { };
    public event UnityAction MenuClickButtonEvent = delegate { };
    public event UnityAction MenuUnpauseEvent = delegate { };
    public event UnityAction MenuPauseEvent = delegate { };
    public event UnityAction MenuCloseEvent = delegate { };
    public event UnityAction OpenInventoryEvent = delegate { }; // Used to bring up the inventory
    public event UnityAction CloseInventoryEvent = delegate { }; // Used to bring up the inventory
    public event UnityAction<float> TabSwitchedEvent = delegate { };

    private GameInput gameInput;

#if UNITY_EDITOR
    private string statusInput = "Default";
    public string StatusInput => statusInput;
#endif

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();

            gameInput.Menus.SetCallbacks(this);
            gameInput.Gameplay.SetCallbacks(this);
            gameInput.Dialogues.SetCallbacks(this);
        }
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
                AttackEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                AttackCanceledEvent.Invoke();
                break;
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            JumpEvent.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            JumpCanceledEvent.Invoke();
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            ClimbEvent.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            ClimbCanceledEvent.Invoke();
    }

    public void OnMouseControlCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            EnableMouseControlCameraEvent.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            DisableMouseControlCameraEvent.Invoke();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                StartedRunning.Invoke();
                break;
            case InputActionPhase.Canceled:
                StoppedRunning.Invoke();
                break;
        }
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
        }
    }

    public void OnRotateCamera(InputAction.CallbackContext context)
    {
        CameraMoveEvent.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
    }

    private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (gameStateManager.CurrentGameState == GameState.Gameplay &&
            context.phase == InputActionPhase.Performed)
            InteractEvent.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MenuPauseEvent.Invoke();
    }


    public void OnMoveSelection(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MoveSelectionEvent.Invoke();
    }

    public void OnAdvanceDialogue(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            AdvanceDialogueEvent.Invoke();
    }


    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MenuClickButtonEvent.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MenuCloseEvent.Invoke();
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MenuMouseMoveEvent.Invoke();
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MenuUnpauseEvent.Invoke();
    }

    public void OnChangeTab(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            TabSwitchedEvent.Invoke(context.ReadValue<float>());
    }

    public void OnInventoryActionButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            InventoryActionButtonEvent.Invoke();
    }

    public void OnSaveActionButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            SaveActionButtonEvent.Invoke();
    }

    public void OnResetActionButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            ResetActionButtonEvent.Invoke();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
    }

    public void OnCloseInventory(InputAction.CallbackContext context) => CloseInventoryEvent.Invoke();

    public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;

    public void EnableGameplayInput()
    {
        gameInput.Gameplay.Enable();
        gameInput.Menus.Disable();
        gameInput.Dialogues.Disable();
#if UNITY_EDITOR
        statusInput = "Gameplay";
#endif
    }

    public void EnableMenuInput()
    {
        gameInput.Menus.Enable();
        gameInput.Gameplay.Disable();
        gameInput.Dialogues.Disable();
#if UNITY_EDITOR
        statusInput = "Menu";
#endif
    }

    public void EnableDialogueInput()
    {
        gameInput.Dialogues.Enable();
        gameInput.Gameplay.Disable();
        gameInput.Menus.Disable();
#if UNITY_EDITOR
        statusInput = "Dialog";
#endif
    }

    public void DisableAllInput()
    {
        gameInput.Gameplay.Disable();
        gameInput.Menus.Disable();
        gameInput.Dialogues.Disable();
#if UNITY_EDITOR
        statusInput = "Disabled";
#endif
    }
}