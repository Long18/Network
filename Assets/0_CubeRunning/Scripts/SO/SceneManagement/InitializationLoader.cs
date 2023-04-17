using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;


/// <summary>
/// This class is responsible for starting the game by loading the persistent managers scene
/// and raising the event to load the Main Scene
/// </summary>
public class InitializationLoader : MonoBehaviour
{
    [SerializeField] private GameSceneSO managersScene = default;
    [SerializeField] private GameSceneSO sceneToLoad = default;

    [Header("Broadcasting on")] [SerializeField]
    private AssetReference requestLoadScene = default;

    private void Awake() => LoadManagerScene();

    #region Class Methods

    /// <summary>
    /// You mus to load this managers scene first
    /// </summary>
    private void LoadManagerScene()
    {
        Addressables.LoadSceneAsync(managersScene.scene, LoadSceneMode.Additive).Completed += OnManagerSceneLoaded;
    }


    private void OnManagerSceneLoaded(AsyncOperationHandle<SceneInstance> obj) => StartCoroutine(DownloadScene());

    /// <summary>
    /// This function is used to load the scene from the addressable
    /// and get the progress of the download
    /// </summary>
    private IEnumerator DownloadScene()
    {
        var handle = Addressables.LoadAssetAsync<LoadEventChannelSO>(requestLoadScene);
        handle.Completed += OnRequestLoadSceneEventAssetLoaded;

        while (!handle.IsDone)
        {
            var status = handle.GetDownloadStatus();
            float progress = status.Percent;
            
            // TODO: Get the progress of the download and send it to the loading screen
            
#if UNITY_EDITOR
            Debug.Log($"[InitializationLoader] Download progress: {progress}");
#endif
            yield return null;
        }
    }

    /// <summary>
    /// This function is loaded when the scene is loaded 
    /// and unload the current scene (Boot Scene)
    /// </summary>
    /// <param name="requestScene">The scene you want to load</param>
    private void OnRequestLoadSceneEventAssetLoaded(AsyncOperationHandle<LoadEventChannelSO> requestScene)
    {
        requestScene.Result.RequestLoadScene(sceneToLoad, true);

        SceneManager.UnloadSceneAsync(0);
    }

    #endregion
}