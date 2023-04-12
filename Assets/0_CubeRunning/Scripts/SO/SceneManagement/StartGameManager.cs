using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>
public class StartGameManager : MonoBehaviour
{
    [SerializeField] private GameSceneSO locationsToLoad;
    [SerializeField] private SaveSystem saveSystem = default;
    [SerializeField] private bool showLoadScreen = default;

    [Header("Broadcasting on")] [SerializeField]
    private LoadEventChannelSO loadLocation = default;

    [Header("Listening to")] [SerializeField]
    private VoidEventChannelSO onNewGameButton = default;

    [SerializeField] private VoidEventChannelSO onContinueButton = default;
    [SerializeField] private VoidEventChannelSO onMultiplayerButton = default;

    private bool checkMultiplay;


    private void OnEnable()
    {
        checkMultiplay = saveSystem.CheckMultiplayer();
        onNewGameButton.OnEventRaised += StartNewGame;
        onContinueButton.OnEventRaised += ContinuePreviousGame;
        onMultiplayerButton.OnEventRaised += StartMultiplayerGame;
    }

    private void OnDisable()
    {
        onNewGameButton.OnEventRaised -= StartNewGame;
        onContinueButton.OnEventRaised -= ContinuePreviousGame;
        onMultiplayerButton.OnEventRaised -= StartMultiplayerGame;
    }

    private void StartNewGame()
    {
        checkMultiplay = false;

        saveSystem.WriteEmptySaveFile();
        saveSystem.SetNewGameData();
        loadLocation.RequestLoadScene(locationsToLoad, showLoadScreen);
    }

    private void ContinuePreviousGame()
    {
        StartCoroutine(LoadSaveGame());
    }

    private void StartMultiplayerGame()
    {
        // TODO: Implement multiplayer
        Debug.Log("Multiplayer not implemented yet");
        // stop game here
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator LoadSaveGame()
    {
        yield return StartCoroutine(saveSystem.LoadSavedInventory());

        saveSystem.LoadSavedQuestlineStatus();
        var locationGuid = saveSystem.saveData.LocationId;

        if (string.IsNullOrEmpty(locationGuid))
        {
            Debug.Log("Location guid is null or empty");
            loadLocation.RequestLoadScene(locationsToLoad, showLoadScreen);
            yield break;
        }

        var asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);

        yield return asyncOperationHandle;

        if (asyncOperationHandle.Result == null)
        {
            Debug.LogError("Location not found");
            yield break;
        }

        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            LocationSO location = asyncOperationHandle.Result;
            loadLocation.RequestLoadScene(location, showLoadScreen);
        }
    }
}