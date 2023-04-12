#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class EditorColdStartup : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private GameSceneSO thisSceneSO = default;
    [SerializeField] private GameSceneSO mainManagersSO = default;

    [Header("Raise event")] [SerializeField]
    private AssetReference notifyColdStartupChannel = default;

    [SerializeField] private VoidEventChannelSO onSceneReadyChannel = default;
    [SerializeField] private PathStorageSO pathStorage = default;
    [SerializeField] private SaveSystem saveSystem = default;

    private bool isColdStart = false;

    private void Awake()
    {
        if (!SceneManager.GetSceneByName(mainManagersSO.scene.editorAsset.name).isLoaded)
        {
            isColdStart = true;

            //Reset the path taken, so the character will spawn in this location's default spawn point
            pathStorage.lastPathTaken = null;
        }

        CreateSaveFileIfNotPresent();
    }

    private void Start()
    {
        if (AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilderIndex == 2) return;

        if (isColdStart)
        {
            Addressables.LoadSceneAsync(mainManagersSO.scene, LoadSceneMode.Additive, true).Completed +=
                LoadEventChannel;
        }

        CreateSaveFileIfNotPresent();
    }

    private void CreateSaveFileIfNotPresent()
    {
        if (saveSystem != null && !saveSystem.LoadSaveDataFromDisk()) return;
        // saveSystem.SetNewGameData();
    }

    private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
    {
        Addressables.LoadAssetAsync<LoadEventChannelSO>(notifyColdStartupChannel).Completed +=
            OnNotifyChannelLoaded;
    }

    private void OnNotifyChannelLoaded(AsyncOperationHandle<LoadEventChannelSO> obj)
    {
        if (thisSceneSO != null)
            obj.Result.RequestLoadScene(thisSceneSO);
        else
            onSceneReadyChannel.RaiseEvent();
    }
#endif
}