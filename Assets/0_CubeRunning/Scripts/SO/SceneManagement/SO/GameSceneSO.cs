using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

[CreateAssetMenu(fileName = "GameScene", menuName = "SceneManagement/GameScene")]
public class GameSceneSO : DescriptionBaseSO
{
    public GameSceneType sceneType;
    public SceneAssetReference scene; //Used at runtime to load the scene from the right AssetBundle

    // public AudioCueSO musicTrack;
    public bool UnloadPreviousScene = true;


    [HideInInspector] public AsyncOperationHandle<SceneInstance> handle;

    /// <summary>
    /// Used by the SceneSelector tool to discern what type of scene it needs to load
    /// </summary>
    public enum GameSceneType
    {
        //Playable scenes
        Location = 0, //SceneSelector tool will also load PersistentManagers and Gameplay
        Menu = 1, //SceneSelector tool will also load Gameplay

        //Special scenes
        Initialisation = 2,
        PersistentManagers = 3,
        InteractiveManagers = 4,

        //Work in progress scenes that don't need to be played
        Art = 5,

        None = 100
    }
}