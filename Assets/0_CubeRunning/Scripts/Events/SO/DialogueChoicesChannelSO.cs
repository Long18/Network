using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue Choices", menuName = "Events/Dialogue Choices Event", order = 0)]
public class DialogueChoicesChannelSO : DescriptionBaseSO
{
    public event UnityAction<List<Choice>> OnEventRaised;

    public void RaiseEvent(List<Choice> values)
    {
        if (OnEventRaised == null)
        {
            Debug.LogWarning($"No one listen to this event {name}");
            return;
        }

        OnEventRaised.Invoke(values);
    }
}