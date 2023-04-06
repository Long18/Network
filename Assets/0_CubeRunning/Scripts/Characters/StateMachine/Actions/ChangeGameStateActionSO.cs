using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine.Serialization;
using Moment = StateMachine.StateAction.SpecificMoment;

/// <summary>
/// This Action handles updating the game state.
/// </summary>
[CreateAssetMenu(fileName = "ChangeGameState", menuName = "State Machines/Actions/Change GameState")]
public class ChangeGameStateActionSO : StateActionSO
{
    [SerializeField] GameState newGameState = default;

    [SerializeField] Moment whenToRun = default;

    [SerializeField] private GameStateSO gameState = default;

    protected override StateAction CreateAction() => new ChangeGameStateAction(newGameState, gameState, whenToRun);
}

public class ChangeGameStateAction : StateAction
{
    [Tooltip("GameState to change to")] private GameState newGameState = default;
    private GameStateSO gameStateSO = default;
    private Moment whenToRun = default;
    private Transform transform = default;

    public ChangeGameStateAction(GameState newGameState, GameStateSO gameStateSO, Moment whenToRun)
    {
        this.newGameState = newGameState;
        this.gameStateSO = gameStateSO;
        this.whenToRun = whenToRun;
    }

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        transform = stateMachine.transform;
    }

    void ChangeState()
    {
        switch (newGameState)
        {
            case GameState.Combat:
                gameStateSO.AddAlertEnemy(transform);
                break;

            case GameState.Gameplay:
                gameStateSO.RemoveAlertEnemy(transform);
                break;

            default:
                gameStateSO.UpdateGameState(newGameState);
                break;
        }
    }

    public override void OnStateEnter()
    {
        if (whenToRun == Moment.OnStateEnter)
        {
            ChangeState();
        }
    }

    public override void OnStateExit()
    {
        if (whenToRun == Moment.OnStateExit)
        {
            ChangeState();
        }
    }

    public override void OnUpdate()
    {
    }
}