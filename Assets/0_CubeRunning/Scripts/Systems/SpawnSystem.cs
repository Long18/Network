using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSystem : MonoBehaviour
{
    [Header("Asset References")] [SerializeField]
    private InputReaderSO inputReaderSO = default;

    [SerializeField] private TransformAnchor playerTransformAnchor = default;
    [SerializeField] private TransformEventChannelSO playerInstantiatedChannel = default;
    [SerializeField] private PathStorageSO pathTaken = default;
    [SerializeField] private SaveSystem saveSystem = default;
    [SerializeField] private Protagonist playerPrefab = default;

    [Header("Listen Events")] [SerializeField]
    private VoidEventChannelSO onSceneReady = default; //Raised by SceneLoader when the scene is set to active

    [Header("Other")] [SerializeField] private LocationEntrance[] spawnLocations;
    [SerializeField] private Transform defaultSpawnPoint;
    private bool isMultiplayer = false;

    public void Awake()
    {
        isMultiplayer = saveSystem.CheckMultiplayer();
        spawnLocations ??= FindObjectsOfType<LocationEntrance>();
    }

    public void OnEnable()
    {
        onSceneReady.OnEventRaised += SpawnPlayer;
    }

    public void OnDisable()
    {
        onSceneReady.OnEventRaised -= SpawnPlayer;
        playerTransformAnchor.Unset();
    }


    private void SpawnPlayer()
    {
        Transform spawnLocation = GetSpawnLocation();
        Protagonist player = null;

        // for each player, i want random spawn location around 10m radius
        // var randNum = Random.Range(0, 10);
        // spawnLocation.position += new Vector3(spawnLocation.position.x + randNum, spawnLocation.position.y,
        //     spawnLocation.position.z + randNum);

        if (!isMultiplayer)
        {
            player = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        }
        else
        {
            // TODO: Implement multiplayer
//          player = PhotonNetwork.Instantiate("Player/Player", spawnLocation.position, spawnLocation.rotation);
        }


        playerInstantiatedChannel.RaiseEvent(player.transform);
        playerTransformAnchor.Provide(player.transform);

        inputReaderSO.EnableGameplayInput();
    }

    private Transform GetSpawnLocation()
    {
        if (pathTaken == null) return defaultSpawnPoint;

        int entranceIndex = Array.FindIndex(spawnLocations, element =>
            element.EntrancePath == pathTaken.lastPathTaken);

        if (entranceIndex == -1)
        {
            Debug.LogWarning("No entrance found for the path taken. Using default spawn location.");
            return defaultSpawnPoint;
        }
        else
        {
            return spawnLocations[entranceIndex].transform;
        }
    }
}