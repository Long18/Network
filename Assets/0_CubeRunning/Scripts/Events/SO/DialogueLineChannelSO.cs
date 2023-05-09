using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Dialogue Line", menuName = "Events/Dialogue Line Event", order = 0)]
public class DialogueLineChannelSO : DescriptionBaseSO
{
    public event UnityAction<LocalizedString, ActorSO> OnEventRaised;

    public void RaiseEvent(LocalizedString line, ActorSO actor)
    {
        if (OnEventRaised == null)
        {
            Debug.LogWarning($"No one listen to this event {name}");
            return;
        }

        OnEventRaised.Invoke(line, actor);
    }
}