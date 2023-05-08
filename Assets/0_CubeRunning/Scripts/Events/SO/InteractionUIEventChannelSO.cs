using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// This class is used for Events to toggle the interaction UI.
/// Example: Dispaly or hide the interaction UI via a bool and the interaction type from the enum via int
/// </summary>
[CreateAssetMenu(fileName = "InteractionUI", menuName = "Events/Toogle Interaction UI Event Channel", order = 0)]
public class InteractionUIEventChannelSO : DescriptionBaseSO
{
    public UnityAction<bool, InteractionType> OnEventRaised;

#if UNITY_EDITOR
    [SerializeField, ReadOnly] private InteractionType lastTypeValue;
    [SerializeField, ReadOnly] private bool lastBoolValue;

    public InteractionType LastTypeValue
    {
        get { return lastTypeValue; }
    }

    public bool LastBoolValue
    {
        get { return lastBoolValue; }
    }
#endif

    public void RaiseEvent(bool state, InteractionType interactionType)
    {
        if (OnEventRaised == null)
        {
            Debug.LogWarning($"No one listen to this event {name}");
            return;
        }

        OnEventRaised.Invoke(state, interactionType);
#if UNITY_EDITOR
        lastTypeValue = interactionType;
        lastBoolValue = state;
#endif
    }
}