using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Events/Dialogue Data Channel", order = 0)]
public class DialogueDataChannelSO : DescriptionBaseSO
{
    public event UnityAction<DialogueDataSO> OnEventRaised;

    public void RaiseEvent(DialogueDataSO value)
    {
        if (OnEventRaised == null)
        {
            Debug.LogWarning($"No one listen to this event {name}");
            return;
        }

        OnEventRaised.Invoke(value);
    }
}