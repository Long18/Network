using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;


/// <summary>
/// This class is responsible for starting the game by loading the persistent managers scene
/// and raising the event to load the Main Scene
/// </summary>
public class InitializationLoader : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameSceneSO managersScene = default;
    [SerializeField] private GameSceneSO sceneToLoad = default;

    [Header("Broadcasting on")] [SerializeField]
    private AssetReference requestLoadScene = default;

    private LoadEventChannelSO requestLoadSceneEventChannel;

    private const string ROOM_NAME = "WILLIAM";

    private void Start()
    {
#if UNITY_EDITOR
        LoadManagerScene();
#else
        PhotonNetwork.ConnectUsingSettings();
#endif
        Debug.Log("Connecting to master");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        JoinRoom(ROOM_NAME);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        LoadManagerScene();
    }


    private void JoinRoom(string roomName)
    {
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            CleanupCacheOnLeave = true,
            IsOpen = true,
        };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

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
            Debug.Log($"[InitializationLoader] Download progress: {progress}");
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