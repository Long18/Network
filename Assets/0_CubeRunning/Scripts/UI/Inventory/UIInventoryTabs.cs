using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryTabs : MonoBehaviour
{
    [SerializeField] private List<UIInventoryTab> listTabs = default;

    public event UnityAction<InventoryTabSO> TabChanged;

    private bool canDisableLayout = false;

    private void OnEnable()
    {
        if (gameObject.GetComponent<VerticalLayoutGroup>() != null && canDisableLayout)
        {
            gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
            canDisableLayout = false;
        }
    }

    public void SetTabs(List<InventoryTabSO> typesList, InventoryTabSO selectedType)
    {
        if (listTabs == null)
        {
            listTabs = new();
        }

        if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
        {
            gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;
        }

        int maxCount = Mathf.Max(typesList.Count, listTabs.Count);

        for (int i = 0; i < maxCount; i++)
        {
            if (i < typesList.Count)
            {
                if (i >= listTabs.Count)
                {
                    Debug.LogError("Maximum tabs reached");
                }

                bool isSelected = typesList[i] == selectedType;
                //fill
                listTabs[i].SetTab(typesList[i], isSelected);
                listTabs[i].gameObject.SetActive(true);
                listTabs[i].TabClicked += ChangeTab;
            }
            else if (i < listTabs.Count)
            {
                //Desactive
                listTabs[i].gameObject.SetActive(false);
            }
        }

        if (isActiveAndEnabled) // check if the game object is active and enabled so that we could start the coroutine. 
        {
            StartCoroutine(waitBeforeDesactiveLayout());
        }
        else // if the game object is inactive, disabling the layout will happen on onEnable 
        {
            canDisableLayout = true;
        }
    }

    private IEnumerator waitBeforeDesactiveLayout()
    {
        yield return new WaitForSeconds(1);
        //disable layout group after layout calculation
        if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
        {
            gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
        }
    }

    public void ChangeTabSelection(InventoryTabSO selectedType)
    {
        for (int i = 0; i < listTabs.Count; i++)
        {
            bool isSelected = listTabs[i].currentTabType == selectedType;

            listTabs[i].UpdateState(isSelected);
        }
    }

    private void ChangeTab(InventoryTabSO selectedType) => TabChanged?.Invoke(selectedType);
}