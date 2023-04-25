using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum GameState
{
    Gameplay = 0, // Default sate: player moves, attacks, etc.
    Pause = 1, // Pause menu is opened, the whole game world is frozen
    Inventory = 2,
    Combat = 3, // Enemy is nearby and alert, player can't open Inventory or initiate dialogues, but can pause the game
    Cutscene = 4,
    LocationTransition = 5, // When the character steps into LocationExit trigger, fade to black begins and control is removed from the player
    Dialogue = 6, // When the character is in a dialogue, the whole game world is frozen
}

[CreateAssetMenu(fileName = "GameState", menuName = "Gameplay/GameState", order = 1)]
public class GameStateSO : DescriptionBaseSO
{
    public GameState CurrentGameState => currentGameState;

    [Header("Game states")] [SerializeField, ReadOnly]
    private GameState currentGameState = default;

    [SerializeField, ReadOnly] private GameState previousGameState = default;

    [Header("Broadcasting on")] [SerializeField]
    private BoolEventChannelSO onCombatStateEvent = default;

    private List<Transform> alertEnemies;

    private void Start()
    {
        alertEnemies = new List<Transform>();
    }

    public void AddAlertEnemy(Transform enemy)
    {
        if (!alertEnemies.Contains(enemy))
        {
            alertEnemies.Add(enemy);
        }

        UpdateGameState(GameState.Combat);
    }

    public void RemoveAlertEnemy(Transform enemy)
    {
        if (alertEnemies.Contains(enemy))
        {
            alertEnemies.Remove(enemy);

            if (alertEnemies.Count == 0)
            {
                UpdateGameState(GameState.Gameplay);
            }
        }
    }

    public void UpdateGameState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
            return;

        if (newGameState == GameState.Combat)
            onCombatStateEvent.RaiseEvent(true);
        else
            onCombatStateEvent.RaiseEvent(false);

        previousGameState = currentGameState;
        currentGameState = newGameState;
    }

    public void ReturnToPreviousGameState()
    {
        if (previousGameState == currentGameState) return;

        if (previousGameState == GameState.Combat)
            onCombatStateEvent.RaiseEvent(false);
        else if (currentGameState == GameState.Combat)
            onCombatStateEvent.RaiseEvent(true);

        (previousGameState, currentGameState) = (currentGameState, previousGameState); // Swap values
        // - previousGameState is assigned the value of currentGameState
        // - currentGameState is assigned the value of previousGameState
        // - previousGameState is assigned the value of currentGameState
    }
}