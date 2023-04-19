using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameStateSO gameState = default;

    private void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        gameState.UpdateGameState(GameState.Gameplay);
    }
}