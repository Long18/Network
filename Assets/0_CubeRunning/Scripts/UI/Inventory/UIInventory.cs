using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UIInventory : MonoBehaviour
{
    public UnityAction Closed;

    [SerializeField] private InputReaderSO inputReader = default;
    [SerializeField] private InventorySO currentInventory = default;
    [SerializeField] private UIInventoryItem itemPrefab = default;
    [SerializeField] private GameObject contentParent = default;
    [SerializeField] private GameObject errorPotMessage = default;
    [SerializeField] private UIInventoryInspector inspectorPanel = default;
    [SerializeField] private List<InventoryTabSO> tabTypesList = new List<InventoryTabSO>();
    [SerializeField] private List<UIInventoryItem> availableItemSlots = default;

    [Header("Listening to")] [SerializeField]
    private UIInventoryTabs tabsPanel = default;

    [SerializeField] private UIActionButton actionButton = default;
    [SerializeField] private VoidEventChannelSO onInteractionEndedEvent = default;

    [Header("Broadcasting on")] [SerializeField]
    private ItemEventChannelSO useItemEvent = default;

    [SerializeField] private IntEventChannelSO restoreHealth = default;
    [SerializeField] private ItemEventChannelSO equipItemEvent = default;
    [SerializeField] private ItemEventChannelSO cookRecipeEvent = default;

    private InventoryTabSO selectedTab = default;
    private bool isNearShop = false;
    private int selectedItemId = -1;

    private void OnEnable()
    {
        actionButton.Clicked += OnActionButtonClicked;
        tabsPanel.TabChanged += OnChangeTab;
        onInteractionEndedEvent.OnEventRaised += InteractionEnded;

        for (int i = 0; i < availableItemSlots.Count; i++)
        {
            availableItemSlots[i].ItemSelected += InspectItem;
        }

        inputReader.TabSwitchedEvent += OnSwitchTab;
    }

    private void OnDisable()
    {
        actionButton.Clicked -= OnActionButtonClicked;
        tabsPanel.TabChanged -= OnChangeTab;
        onInteractionEndedEvent.OnEventRaised -= InteractionEnded;

        for (int i = 0; i < availableItemSlots.Count; i++)
        {
            availableItemSlots[i].ItemSelected -= InspectItem;
        }

        inputReader.TabSwitchedEvent -= OnSwitchTab;
    }

    private void OnSwitchTab(float orientation)
    {
        if (orientation == 0) return;

        bool isLeft = orientation < 0;
        int initalIndex = tabTypesList.FindIndex(i => i == selectedTab);

        if (initalIndex == -1) return;

        if (isLeft) initalIndex--;
        else initalIndex++;

        initalIndex = Mathf.Clamp(initalIndex, 0, tabTypesList.Count - 1);

        OnChangeTab(tabTypesList[initalIndex]);
    }

    public void FillInventory(InventoryTabType selectedTabType = InventoryTabType.Sword, bool _isNearShop = false)
    {
        isNearShop = _isNearShop;

        if (tabTypesList.Exists(i => i.TabType == selectedTabType))
        {
            selectedTab = tabTypesList.Find(i => i.TabType == selectedTabType);
        }
        else
        {
            if (tabTypesList == null) return;

            if (tabTypesList.Count > 0)
            {
                selectedTab = tabTypesList[0];
            }
        }

        if (selectedTab != null)
        {
            SetTabs(tabTypesList, selectedTab);
            List<ItemStack> listItem = new();
            listItem = currentInventory.Items.FindAll(i => i.Item.ItemType.TabType == selectedTab);

            FillInventoryItems(listItem);
        }
        else
        {
            Debug.LogError("No tab found");
        }
    }

    private void InteractionEnded() => isNearShop = false;

    private void SetTabs(List<InventoryTabSO> typesList, InventoryTabSO selectedType) =>
        tabsPanel.SetTabs(typesList, selectedType);

    private void FillInventoryItems(List<ItemStack> listItem)
    {
        if (availableItemSlots == null) availableItemSlots = new();

        int maxCount = Math.Max(listItem.Count, availableItemSlots.Count);

        for (int i = 0; i < maxCount; i++)
        {
            if (i < listItem.Count)
            {
                bool isSelected = selectedItemId == i;
                availableItemSlots[i].SetItem(listItem[i], isSelected);
            }
            else if (i < availableItemSlots.Count)
            {
                availableItemSlots[i].SetInactiveItem();
            }
        }

        HideItemInformation();

        if (selectedItemId >= 0)
        {
            UnselectItem(selectedItemId);
            selectedItemId = -1;
        }

        if (availableItemSlots.Count > 0)
        {
            availableItemSlots[0].SelectFirstElement();
        }
    }

    private void UpdateItemInInventory(ItemStack itemToUpdate, bool removeItem)
    {
        if (availableItemSlots == null)
            availableItemSlots = new List<UIInventoryItem>();

        if (removeItem)
        {
            if (availableItemSlots.Exists(o => o.currentItem == itemToUpdate))
            {
                int index = availableItemSlots.FindIndex(o => o.currentItem == itemToUpdate);
                availableItemSlots[index].SetInactiveItem();
            }
        }
        else
        {
            int index = 0;

            //if the item has already been created
            if (availableItemSlots.Exists(o => o.currentItem == itemToUpdate))
            {
                index = availableItemSlots.FindIndex(o => o.currentItem == itemToUpdate);
            }
            //if the item needs to be created
            else
            {
                //if the new item needs to be instantiated
                if (currentInventory.Items.Count > availableItemSlots.Count)
                {
                    UIInventoryItem instantiatedPrefab =
                        Instantiate(itemPrefab, contentParent.transform) as UIInventoryItem;
                    availableItemSlots.Add(instantiatedPrefab);
                }

                //find the last instantiated game object not used
                index = currentInventory.Items.Count;
            }

            bool isSelected = selectedItemId == index;
            availableItemSlots[index].SetItem(itemToUpdate, isSelected);
        }
    }

    public void InspectItem(ItemSO itemToInspect)
    {
        if (availableItemSlots.Exists(o => o.currentItem.Item == itemToInspect))
        {
            int itemIndex = availableItemSlots.FindIndex(o => o.currentItem.Item == itemToInspect);

            //unselect selected Item
            if (selectedItemId >= 0 && selectedItemId != itemIndex)
                UnselectItem(selectedItemId);

            //change Selected ID 
            selectedItemId = itemIndex;

            //show Information
            ShowItemInformation(itemToInspect);

            //check if interactable
            bool isInteractable = true;
            actionButton.gameObject.SetActive(true);
            errorPotMessage.SetActive(false);
            if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.Shop)
            {
                isInteractable = currentInventory.hasIngredients(itemToInspect.IngredientsList) && isNearShop;
                errorPotMessage.SetActive(!isNearShop);
            }
            else if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.DoNothing)
            {
                isInteractable = false;
                actionButton.gameObject.SetActive(false);
            }

            //set button
            actionButton.FillInventoryButton(itemToInspect.ItemType, isInteractable);
        }
    }

    void ShowItemInformation(ItemSO item)
    {
        bool[] availabilityArray = currentInventory.IngredientsAvailability(item.IngredientsList);

        inspectorPanel.FillInspector(item, availabilityArray);
        inspectorPanel.gameObject.SetActive(true);
    }

    void HideItemInformation()
    {
        actionButton.gameObject.SetActive(false);
        inspectorPanel.gameObject.SetActive(false);
    }

    void UnselectItem(int itemIndex)
    {
        if (availableItemSlots.Count > itemIndex)
        {
            availableItemSlots[itemIndex].UnSelectItem();
        }
    }

    void UpdateInventory()
    {
        FillInventory(selectedTab.TabType, isNearShop);
    }

    void OnActionButtonClicked()
    {
        //find the selected Item
        if (availableItemSlots.Count > selectedItemId
            && selectedItemId > -1)
        {
            ItemSO itemToActOn = ScriptableObject.CreateInstance<ItemSO>();
            itemToActOn = availableItemSlots[selectedItemId].currentItem.Item;

            //check the selected Item type
            //call action function depending on the itemType
            switch (itemToActOn.ItemType.ActionType)
            {
                case ItemInventoryActionType.Shop:
                    CookRecipe(itemToActOn);
                    break;
                case ItemInventoryActionType.Use:
                    UseItem(itemToActOn);
                    break;
                case ItemInventoryActionType.Equip:
                    EquipItem(itemToActOn);
                    break;
                default:

                    break;
            }
        }
    }

    void UseItem(ItemSO itemToUse)
    {
        if (itemToUse.Amount > 0)
        {
            restoreHealth.RaiseEvent(itemToUse.Amount);
        }

        useItemEvent.RaiseEvent(itemToUse);
        UpdateInventory();
    }

    void EquipItem(ItemSO itemToUse)
    {
        Debug.Log("Equip ITEM " + itemToUse.name);
        equipItemEvent.RaiseEvent(itemToUse);
    }

    void CookRecipe(ItemSO recipeToCook)
    {
        cookRecipeEvent.RaiseEvent(recipeToCook);

        //update inspector
        InspectItem(recipeToCook);

        //update inventory
        UpdateInventory();
    }

    void OnChangeTab(InventoryTabSO tabType)
    {
        FillInventory(tabType.TabType, isNearShop);
    }

    public void CloseInventory()
    {
        Closed.Invoke();
    }
}