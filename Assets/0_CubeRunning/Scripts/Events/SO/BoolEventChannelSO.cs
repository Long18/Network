using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have a bool argument.
/// Example: An event to toggle a UI interface
/// </summary>
/// 
[CreateAssetMenu(menuName = "Events/Bool Event Channel", order = 0)]
public class BoolEventChannelSO : DescriptionBaseSO
{
    public event UnityAction<bool> OnEventRaised;

#if UNITY_EDITOR
    [SerializeField] private bool lastValue;
    public bool LastValue
    {
        get { return lastValue; }
    }
#endif

    public void RaiseEvent(bool value)
    {
        if (OnEventRaised == null)
        {
            Debug.LogWarning($"No one listen to this event {name}");
            return;
        }

        OnEventRaised.Invoke(value);
#if UNITY_EDITOR
        lastValue = value;
#endif
    }
}