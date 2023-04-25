using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Dialogue Choice", menuName = "Events/Dialogue Choice Event", order = 0)]
public class DialogueChoiceChannelSO : DescriptionBaseSO
{
    public event UnityAction<Choice> OnEventRaised;

    public void RaiseEvent(Choice choice)
    {
        if (OnEventRaised == null)
        {
            Debug.LogWarning($"No one listen to this event {name}");
            return;
        }

        OnEventRaised.Invoke(choice);
    }
}