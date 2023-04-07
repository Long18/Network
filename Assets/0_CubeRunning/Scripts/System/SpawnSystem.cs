using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSystem : MonoBehaviourPunCallbacks
{
    [Header("Asset References")] [SerializeField]
    private InputReaderSO inputReaderSO = default;

    [SerializeField] private TransformAnchor playerTransformAnchor = default;
    [SerializeField] private TransformEventChannelSO playerInstantiatedChannel = default;
    [SerializeField] private PathStorageSO pathTaken = default;

    [Header("Listen Events")] [SerializeField]
    private VoidEventChannelSO onSceneReady = default; //Raised by SceneLoader when the scene is set to active

    private LocationEntrance[] spawnLocations;
    private Transform defaultSpawnPoint;


    public void Awake()
    {
        spawnLocations = GameObject.FindObjectsOfType<LocationEntrance>();
        defaultSpawnPoint = transform.GetChild(0);
    }

    public override void OnEnable()
    {
        onSceneReady.OnEventRaised += SpawnPlayer;
    }

    public override void OnDisable()
    {
        onSceneReady.OnEventRaised -= SpawnPlayer;
        playerTransformAnchor.Unset();
    }


    private void SpawnPlayer()
    {
        Debug.Log($"[Photon] Spawn player");
        Transform spawnLocation = GetSpawnLocation();

        // for each player, i want random spawn location around 10m radius
        var randNum = Random.Range(0, 10);
        spawnLocation.position += new Vector3(spawnLocation.position.x + randNum, spawnLocation.position.y,
            spawnLocation.position.z + randNum);

        GameObject player = null;

#if UNITY_EDITOR
        // Singleplayer
        player =
            Instantiate(Resources.Load("Player/Player"), spawnLocation.position, spawnLocation.rotation) as GameObject;
#else
        // Multiplayer
        player = PhotonNetwork.Instantiate("Player/Player", spawnLocation.position, spawnLocation.rotation);
#endif


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