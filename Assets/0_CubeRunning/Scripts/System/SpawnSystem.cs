using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnSystem : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private int defaultSpawnIndex = 0;

    [Header("Project References")] [SerializeField]
    private Protagonist playerPrefab = null;

    [Header("Scene References")] [SerializeField]
    private CameraManager cameraManager;

    [SerializeField] private Transform[] spawnLocations;

    void Start()
    {
        try
        {
            Spawn(defaultSpawnIndex);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SpawnSystem] Failed to spawn player. {e.Message}");
        }
    }

    void Reset()
    {
        AutoFill();
    }

    /// <summary>
    /// This function tries to autofill some of the parameters of the component, so it's easy to drop in a new scene
    /// </summary>
    [ContextMenu("Attempt Auto Fill")]
    private void AutoFill()
    {
        if (cameraManager == null)
            cameraManager = FindObjectOfType<CameraManager>();

        if (spawnLocations == null || spawnLocations.Length == 0)
            spawnLocations = transform.GetComponentsInChildren<Transform>(true)
                .Where(t => t != this.transform)
                .ToArray();
    }

    private void Spawn(int spawnIndex)
    {
        Transform spawnLocation = GetSpawnLocation(spawnIndex, spawnLocations);
        Protagonist playerInstance = InstantiatePlayer(playerPrefab, spawnLocation, cameraManager);
        SetupCameras(playerInstance);
    }

    private Transform GetSpawnLocation(int index, Transform[] spawnLocations)
    {
        if (spawnLocations == null || spawnLocations.Length == 0)
            throw new Exception("No spawn locations set.");

        index = Mathf.Clamp(index, 0, spawnLocations.Length - 1);
        return spawnLocations[index];
    }

    private Protagonist InstantiatePlayer(Protagonist playerPrefab, Transform spawnLocation,
        CameraManager _cameraManager)
    {
        if (playerPrefab == null)
            throw new Exception("Player Prefab can't be null.");

        Protagonist playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);

        return playerInstance;
    }

    private void SetupCameras(Protagonist player)
    {
        player.gameplayCamera = cameraManager.mainCamera.transform;
        cameraManager.SetupProtagonistVirtualCamera(player.transform);
    }
}