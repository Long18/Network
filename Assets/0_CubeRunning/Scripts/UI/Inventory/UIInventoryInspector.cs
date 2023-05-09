using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryInspector : MonoBehaviour
{
    [SerializeField] private UIInspectorDescription inspectorDescription = default;

    public void FillInspector(ItemSO item, bool[] availabilityArray = null)
    {
        bool isForShopping = (item.ItemType.ActionType == ItemInventoryActionType.Shop);

        inspectorDescription.FillDescription(item);
    }
}