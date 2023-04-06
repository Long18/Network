using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This class is used for Events that have no arguments.
/// Example: An event to start the game
/// </summary>
[CreateAssetMenu(menuName = "Events/Void Event Channel", order = 0)]
public class VoidEventChannelSO : DescriptionBaseSO
{
    public event UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        if (OnEventRaised == null)
        {
            Debug.LogWarning($"No one listen to this event {name}");
            return;
        }

        OnEventRaised.Invoke();
    }
}