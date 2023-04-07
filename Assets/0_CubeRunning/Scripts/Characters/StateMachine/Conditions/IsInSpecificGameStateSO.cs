using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is In Particular GameState")]
public class IsInSpecificGameStateSO : StateConditionSO<IsInSpecificGameStateCondition>
{
    public GameState gameStateToCheck;
    public GameStateSO gameStateSO;

    protected override Condition CreateCondition() => new IsInSpecificGameStateCondition();
}

public class IsInSpecificGameStateCondition : Condition
{
    private IsInSpecificGameStateSO originSO =>
        (IsInSpecificGameStateSO)base.OriginSO; // The SO this Condition spawned from

    protected override bool Statement()
    {
        return originSO.gameStateToCheck == originSO.gameStateSO.CurrentGameState;
    }
}