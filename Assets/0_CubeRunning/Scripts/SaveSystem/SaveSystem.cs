using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public class SaveSystem : ScriptableObject
{
    [SerializeField] private VoidEventChannelSO saveSettingsEvent = default;
    [SerializeField] private LoadEventChannelSO loadLocation = default;
    [SerializeField] private InventorySO playerInventory = default;

    [SerializeField] private SettingsSO currentSettings = default;
    // [SerializeField] private QuestManagerSO _questManagerSO = default;

    public string saveFilename = "save.william";
    public string backupSaveFilename = "save.william.bak";
    public Save saveData = new Save();


    void OnEnable()
    {
        saveSettingsEvent.OnEventRaised += SaveSettings;
        loadLocation.OnLoadingRequested += CacheLoadLocations;
    }

    void OnDisable()
    {
        saveSettingsEvent.OnEventRaised -= SaveSettings;
        loadLocation.OnLoadingRequested -= CacheLoadLocations;
    }

    private void CacheLoadLocations(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        LocationSO locationSO = locationToLoad as LocationSO;
        if (locationSO)
        {
            saveData.LocationId = locationSO.Guid;
        }

        SaveDataToDisk();
    }

    public bool LoadSaveDataFromDisk()
    {
        if (FileManager.LoadFromFile(saveFilename, out var json))
        {
            saveData.LoadFromJson(json);
            return true;
        }

        Debug.Log($"[SaveSystem] No save data found at {saveFilename}");
        return false;
    }

    public bool CheckMultiplayer()
    {
        if (saveData.IsMultiplay) return true;
        else return false;
    }

    public IEnumerator LoadSavedInventory()
    {
        playerInventory.Items.Clear();
        foreach (var serializedItemStack in saveData.ItemStacks)
        {
            var loadItemOperationHandle = Addressables.LoadAssetAsync<ItemSO>(serializedItemStack.itemGuid);
            yield return loadItemOperationHandle;
            if (loadItemOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                var itemSO = loadItemOperationHandle.Result;
                playerInventory.Add(itemSO, serializedItemStack.amount);
            }
        }
    }

    public void LoadSavedQuestlineStatus()
    {
        // _questManagerSO.SetFinishedQuestlineItemsFromSave(saveData._finishedQuestlineItemsGUIds);
    }

    public void SaveDataToDisk()
    {
        saveData.ItemStacks.Clear();
        foreach (var itemStack in playerInventory.Items)
        {
            saveData.ItemStacks.Add(new SerializedItemStack(itemStack.Item.Guid, itemStack.Amount));
        }

        saveData.FinishedQuestlineItemsGUIds.Clear();

        // foreach (var item in _questManagerSO.GetFinishedQuestlineItemsGUIds())
        // {
        // 	saveData._finishedQuestlineItemsGUIds.Add(item);
        //
        // }
        if (FileManager.MoveFile(saveFilename, backupSaveFilename))
        {
            if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
            {
                Debug.Log("Save successful " + saveFilename);
            }
        }
    }

    public void WriteEmptySaveFile()
    {
        FileManager.WriteToFile(saveFilename, "");
    }

    public void SetNewGameData()
    {
        FileManager.WriteToFile(saveFilename, "");
        playerInventory.Init();
        // _questManagerSO.ResetQuestlines();

        SaveDataToDisk();
    }

    void SaveSettings()
    {
        saveData.SaveSettings(currentSettings);
    }

    public void ResetSettings()
    {
        saveData.ResetSettings();
        if (FileManager.DeleteFile(backupSaveFilename))
        {
            Debug.Log($"<color=red>[SaveSystem]</color> Backup save deleted");
            if (FileManager.DeleteFile(saveFilename))
            {
                Debug.Log($"<color=red>[SaveSystem]</color> Save deleted");
            }
        }
    }
}