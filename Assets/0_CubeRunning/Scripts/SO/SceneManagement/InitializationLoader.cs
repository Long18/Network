using System;
using System.Collections;
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

    private LoadEventChannelSO requestLoadSceneEventChannel;

    private void Awake() => LoadManagerScene();

    #region Class Methods

    /// <summary>
    /// You mus to load this managers scene first
    /// </summary>
    private void LoadManagerScene() =>
        Addressables.LoadSceneAsync(managersScene.scene, LoadSceneMode.Additive).Completed += OnManagerSceneLoaded;

    private void OnManagerSceneLoaded(AsyncOperationHandle<SceneInstance> obj) => StartCoroutine(DownloadScene());

    /// <summary>
    /// This function is used to load the scene from the addressable 
    /// </summary>
    private IEnumerator DownloadScene()
    {
        var handle = Addressables.LoadAssetAsync<LoadEventChannelSO>(requestLoadScene);
        handle.Completed += OnRequestLoadSceneEventAssetLoaded;

        while (!handle.IsDone)
        {
            var status = handle.GetDownloadStatus();
            float progress = status.Percent;
#if UNITY_EDITOR
            Debug.Log($"Download progress: {progress}");
#endif
            yield return null;
        }
    }

    /// <summary>
    /// This function is loaded when the scene is loaded 
    /// and unload the current scene (Boot Scene)
    /// </summary>
    /// <param name="obj">The scene you want to load</param>
    private void OnRequestLoadSceneEventAssetLoaded(AsyncOperationHandle<LoadEventChannelSO> obj)
    {
        requestLoadSceneEventChannel = obj.Result;

        requestLoadSceneEventChannel.RequestLoadScene(sceneToLoad);
        SceneManager.UnloadSceneAsync(0);
    }

    #endregion
}