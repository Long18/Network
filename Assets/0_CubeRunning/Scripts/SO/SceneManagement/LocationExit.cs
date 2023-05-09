using UnityEngine;

/// <summary>
/// This class goes on a trigger which, when entered, sends the player to another Location
/// </summary>
public class LocationExit : MonoBehaviour
{
    [SerializeField] private GameSceneSO locationToLoad = default;
    [SerializeField] private PathSO leadsToPath = default;
    [SerializeField] private PathStorageSO pathStorage = default; //This is where the last path taken will be stored

    [Header("Broadcasting on")] [SerializeField]
    private LoadEventChannelSO locationExitLoadChannel = default;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("LocationExit: OnTriggerEnter");
        if (other.CompareTag("Player"))
        {
            pathStorage.lastPathTaken = leadsToPath;
            locationExitLoadChannel.RequestLoadScene(locationToLoad, false, true);
        }
    }
}