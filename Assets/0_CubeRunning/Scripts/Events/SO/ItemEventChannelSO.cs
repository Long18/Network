using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Item interaction events.
/// Example: Pick up an item passed as paramater
/// </summary>
[CreateAssetMenu(menuName = "Events/UI/Item Event Channel")]
public class ItemEventChannelSO : DescriptionBaseSO
{
    public UnityAction<ItemSO> OnEventRaised;


#if UNITY_EDITOR
    [SerializeField, ReadOnly] private ItemSO lastValue;

    public ItemSO LastValue
    {
        get { return lastValue; }
    }
#endif
    public void RaiseEvent(ItemSO value)
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