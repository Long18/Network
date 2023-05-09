using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NPC Movement", menuName = "Events/NPC Movement Event Channel", order = 0)]
public class NPCMovementEventChannelSO : DescriptionBaseSO
{
    public event UnityAction<NPCMovementConfigSO> OnEventRaised;

    public void RaiseEvent(NPCMovementConfigSO value)
    {
        if (OnEventRaised == null)
        {
            Debug.LogWarning($"No one listen to this event {name}");
            return;
        }

        OnEventRaised.Invoke(value);
    }
}