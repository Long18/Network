using System.Collections;
using Cinemachine;
using UnityEngine;

public class LocationEntrance : MonoBehaviour
{
    [SerializeField] private PathSO entrancePath;
    [SerializeField] private PathStorageSO pathStorage = default; //This is where the last path taken has been stored
    [SerializeField] private CinemachineVirtualCamera entranceShot;

    [Header("Listen event")] [SerializeField]
    private VoidEventChannelSO onSceneReady;

    public PathSO EntrancePath => entrancePath;

    private void Awake()
    {
        if (pathStorage.lastPathTaken == entrancePath)
        {
            entranceShot.Priority = 100;
            onSceneReady.OnEventRaised += PlanTransition;
        }
    }

    private void PlanTransition()
    {
        StartCoroutine(TransitionToGameCamera());
    }

    private IEnumerator TransitionToGameCamera()
    {
        yield return new WaitForSeconds(.1f);

        entranceShot.Priority = -1;
        onSceneReady.OnEventRaised -= PlanTransition;
    }
}