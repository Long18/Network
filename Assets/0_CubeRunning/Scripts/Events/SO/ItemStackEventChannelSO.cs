using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Item interaction events.
/// Example: Pick up an item passed as paramater
/// </summary>
[CreateAssetMenu(menuName = "Events/UI/Item stack Event Channel")]
public class ItemStackEventChannelSO : DescriptionBaseSO
{
    public UnityAction<ItemStack> OnEventRaised;

#if UNITY_EDITOR
    [SerializeField] private ItemStack lastValue;

    public ItemStack LastValue
    {
        get { return lastValue; }
    }
#endif
    public void RaiseEvent(ItemStack value)
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