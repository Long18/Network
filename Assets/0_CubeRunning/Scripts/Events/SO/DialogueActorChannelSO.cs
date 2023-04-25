using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue Actor", menuName = "Events/Dialogue Actor Channel", order = 0)]
public class DialogueActorChannelSO : DescriptionBaseSO
{
    public event UnityAction<ActorSO> OnEventRaised;

    public void RaiseEvent(ActorSO value)
    {
        if (OnEventRaised == null)
        {
            Debug.LogWarning($"No one listen to this event {name}");
            return;
        }

        OnEventRaised.Invoke(value);
    }
}