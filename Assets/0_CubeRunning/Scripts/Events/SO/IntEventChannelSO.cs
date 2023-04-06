using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have a int argument.
/// Example: An event to change the score
/// </summary>
[CreateAssetMenu(menuName = "Events/Int Event Channel", order = 0)]
public class IntEventChannelSO : DescriptionBaseSO
{
    public event UnityAction<int> OnEventRaised;

#if UNITY_EDITOR
    [SerializeField] private int lastValue;
    public int LastValue
    {
        get { return lastValue; }
    }
#endif

    public void RaiseEvent(int value)
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