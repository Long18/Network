using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have one transform argument.
/// Example: Spawn system initializes player, where the transform is the position of player.
/// </summary>
[CreateAssetMenu(menuName = "Events/Transform Event Channel", order = 0)]
public class TransformEventChannelSO : DescriptionBaseSO
{
    public UnityAction<Transform> OnEventRaised;

#if UNITY_EDITOR
    [SerializeField, ReadOnly] private Transform lastValue;

    public Transform LastValue
    {
        get { return lastValue; }
    }
#endif

    public void RaiseEvent(Transform value)
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