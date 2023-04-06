using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have one float argument.
/// Example: An Achievement unlock event, where the float is the Achievement ID.
/// </summary>
[CreateAssetMenu(menuName = "Events/Bool Event Channel", order = 0)]
public class FloatEventChannelSO : ScriptableObject
{
    public event UnityAction<float> OnEventRaised;

#if UNITY_EDITOR
    [SerializeField] private float lastValue;
    public float LastValue
    {
        get { return lastValue; }
    }
#endif

    public void RaiseEvent(float value)
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