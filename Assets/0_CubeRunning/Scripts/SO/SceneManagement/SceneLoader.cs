using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneManagerSO sceneManager = default;
    [SerializeField] private GameSceneSO interactiveManager = default;
    [SerializeField] private InputReaderSO inputReader = default;

    [Header("Listening to")] [SerializeField]
    private LoadEventChannelSO loadLocation = default;

    [SerializeField] private LoadEventChannelSO loadMenu = default;
    [SerializeField] private LoadEventChannelSO coldStartupLocation = default;

    [Header("Broadcasting on")] [SerializeField]
    private BoolEventChannelSO toggleLoadingScreen = default;

    [SerializeField] private VoidEventChannelSO onSceneReady = default; //picked up by the SpawnSystem
    [SerializeField] private FadeChannelSO fadeRequestChannel = default;
    [SerializeField] private FloatEventChannelSO loadingProgressChannel = default;

    private GameSceneSO sceneToLoad;
    private GameSceneSO currentlyLoadedScene;
    private AsyncOperationHandle<SceneInstance> loadingOperationHandle;
    private AsyncOperationHandle<SceneInstance> gameplayManagerLoadingOpHandle;

    private bool showLoadingScreen;
    private SceneInstance gameplayManagerSceneInstance;
    private float fadeDuration = 1f;

    private bool isLoading = false; //To prevent a new loading request while already loading a new scene

    private void OnEnable()
    {
        loadLocation.OnLoadingRequested += LoadLocation;
        loadMenu.OnLoadingRequested += LoadMenu;
#if UNITY_EDITOR
        coldStartupLocation.OnLoadingRequested += LocationColdStartup;
#endif
    }

    private void OnDisable()
    {
        loadLocation.OnLoadingRequested -= LoadLocation;
        loadMenu.OnLoadingRequested -= LoadMenu;
#if UNITY_EDITOR
        coldStartupLocation.OnLoadingRequested -= LocationColdStartup;
#endif
    }

#if UNITY_EDITOR
    /// <summary>
    /// This special loading function is only used in the editor,
    /// when the developer presses Play in a Location scene, without passing by Initialisation.
    /// </summary>
    /// <param name="currentlyOpenedLocation">Check current type scene.</param>
    private void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen, bool fadeScreen)
    {
        currentlyLoadedScene = currentlyOpenedLocation;
        switch (currentlyLoadedScene.sceneType)
        {
            case GameSceneSO.GameSceneType.Location:
                gameplayManagerLoadingOpHandle =
                    Addressables.LoadSceneAsync(interactiveManager.scene, LoadSceneMode.Additive, true);
                gameplayManagerLoadingOpHandle.WaitForCompletion();
                gameplayManagerSceneInstance = gameplayManagerLoadingOpHandle.Result;
                StartGamePlay();
                break;
            default:
                StartGamePlay();
                break;
        }
    }

#endif

    /// <summary>
    /// This function loads the location scenes passed as array parameter.
    /// </summary>
    private void LoadLocation(GameSceneSO locationToLoad, bool showLoading, bool fadeScreen)
    {
        // Prevent a double loading request
        if (isLoading) return;

        sceneToLoad = locationToLoad;
        showLoadingScreen = showLoading;
        isLoading = true;

        // In case we are coming from the main menu, we need to load the manager scene first
        if (!gameplayManagerSceneInstance.Scene.IsValid())
            StartCoroutine(DownloadAsset(interactiveManager.scene));
        else
            StartCoroutine(UnloadPreviousScene());
    }

    private void OnGameplayManagerLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        gameplayManagerSceneInstance = gameplayManagerLoadingOpHandle.Result;
        StartCoroutine(UnloadPreviousScene());
    }

    /// <summary>
    /// Prepares to load the main menu scene, first removing the Interactive scene in case the game is coming back from gameplay to menus
    /// </summary>
    private void LoadMenu(GameSceneSO menuToLoad, bool showLoading, bool fadeScreen)
    {
        // Prevent a double loading request
        if (isLoading) return;

        sceneToLoad = menuToLoad;
        showLoadingScreen = showLoading;
        isLoading = true;

        //In case we are coming from a Location back to the main menu, we need to get rid of the persistent Gameplay manager scene
        if (gameplayManagerSceneInstance.Scene.IsValid())
            Addressables.UnloadSceneAsync(gameplayManagerLoadingOpHandle);

        StartCoroutine(UnloadPreviousScene());
    }

    /// <summary>
    /// In both Location and Menu scenes, we need to unload the previous scene before loading the new one. 
    /// </summary>
    private IEnumerator UnloadPreviousScene()
    {
        inputReader.DisableAllInput();
        fadeRequestChannel.FadeOut(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);


        if (currentlyLoadedScene != null && sceneToLoad.UnloadPreviousScene)
        {
            if (currentlyLoadedScene.handle.IsValid())
            {
                // Unload the scene through Addressables, i.e. through the AssetReference system
                // every assets usage should be through the AssetReference system
                var handle = Addressables.UnloadSceneAsync(currentlyLoadedScene.handle, true);
                handle.Completed += handleOP => Resources.UnloadUnusedAssets();
            }
#if UNITY_EDITOR
            else
            {
                // When cold start gameplay => we will need to unload main/gameplay manager scene
                // because OperationHandle is not valid
                Debug.Log($"SceneLoader::UnloadPreviousScene: Unloading scene: {currentlyLoadedScene.scene}");
                SceneManager.UnloadSceneAsync(currentlyLoadedScene.scene.editorAsset.name);
            }
#endif
        }

        LoadNewScene();
        yield break;
    }

    /// <summary>
    /// kicks off the loading of the new scene
    /// </summary>
    private void LoadNewScene()
    {
        if (showLoadingScreen) toggleLoadingScreen.RaiseEvent(true);
        StartCoroutine(DownloadAsset(sceneToLoad.scene, true));
    }

    private IEnumerator DownloadAsset(SceneAssetReference scene, bool isLoadNewScreen = false)
    {
        if (isLoadNewScreen)
        {
            loadingOperationHandle = Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive, true, 0);
            sceneToLoad.handle = loadingOperationHandle;
            loadingOperationHandle.Completed += OnNewSceneLoaded;

            while (!loadingOperationHandle.IsDone)
            {
                var status = loadingOperationHandle.GetDownloadStatus();
                float progress = status.Percent;

                loadingProgressChannel.RaiseEvent(progress);
                yield return null;
            }
        }
        else
        {
            gameplayManagerLoadingOpHandle = Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive, true);
            gameplayManagerLoadingOpHandle.Completed += OnGameplayManagerLoaded;

            while (!gameplayManagerLoadingOpHandle.IsDone)
            {
                var status = gameplayManagerLoadingOpHandle.GetDownloadStatus();
                float progress = status.Percent;

                loadingProgressChannel.RaiseEvent(progress);
                yield return null;
            }
        }
    }

    private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        sceneManager.lastScene = currentlyLoadedScene;
        sceneManager.currentScene = sceneToLoad;
        currentlyLoadedScene = sceneToLoad;

        Scene s = obj.Result.Scene;
        SceneManager.SetActiveScene(s);
        LightProbes.TetrahedralizeAsync();

        isLoading = false;
        if (showLoadingScreen) toggleLoadingScreen.RaiseEvent(false);

        fadeRequestChannel.FadeIn(fadeDuration);

        StartGamePlay();
    }

    private void StartGamePlay() => onSceneReady.RaiseEvent();
}