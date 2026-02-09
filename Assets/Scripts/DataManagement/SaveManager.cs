using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; } 
    public GameSaveFile saveFile;
    public int currentActiveSlot = 1;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Call this to save the current state to a file
    public void OnSaveButtonPressed()
    {
        OnLoadButtonPressed();

        Unit[] units = FindObjectsOfType<Unit>();

        foreach (Unit u in units)
        {
            if (u.IsHired)
            {
                UnitSaveData packedData = u.SaveToData();

                // Check if this specific unit already exists in the loaded list
                int index = saveFile.hiredAdventurers.FindIndex(d => d.unitID == packedData.unitID);

                if (index == -1)
                {
                    // If it's a brand new unit, add it to the existing list
                    saveFile.hiredAdventurers.Add(packedData);
                }
                else
                {
                    // If it already existed in the file, update its data (e.g. new position/stats)
                    saveFile.hiredAdventurers[index] = packedData;
                }
            }
        }
        SaveToFile();

        Debug.Log("Adventurers Saved! Adventurer Count: " + saveFile.hiredAdventurers.Count);
        Debug.Log("Save created at: " + saveFile.saveTimestamp);
    }

    public void OnSaveButtonPressedNOLOAD()
    {
        // REMOVED: OnLoadButtonPressed(); <-- This was overwriting your current progress!

        // 1. Ensure the currentActiveSlot data is in memory
        if (saveFile == null) saveFile = new GameSaveFile();

        // 2. Find all active units (this includes SelectorUI since it inherits from Unit)
        Unit[] units = FindObjectsOfType<Unit>();

        foreach (Unit u in units)
        {
            if (u.IsHired)
            {
                // Unit.SaveToData() MUST include the inventory list!
                UnitSaveData packedData = u.SaveToData();

                int index = saveFile.hiredAdventurers.FindIndex(d => d.unitID == packedData.unitID);

                if (index == -1)
                    saveFile.hiredAdventurers.Add(packedData);
                else
                    saveFile.hiredAdventurers[index] = packedData;
            }
        }

        SaveToFile();
    }

    // Call this to load data back into the scene
    public void OnLoadButtonPressed()
    {
        string path = GetPath(currentActiveSlot);
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        saveFile = JsonUtility.FromJson<GameSaveFile>(json);

        // Logic here depends on if you want to update existing units 
        // or clear the scene and re-spawn them.


        Debug.Log("Data Loaded!");
    }

    public List<UnitSaveData> GetAllUnitsFromSave()
    {
        string path = GetPath(currentActiveSlot);

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found at " + path);
            return new List<UnitSaveData>(); // Return empty list instead of null to avoid errors
        }

        string json = File.ReadAllText(path);
        GameSaveFile tempFile = JsonUtility.FromJson<GameSaveFile>(json);

        return tempFile.hiredAdventurers ?? new List<UnitSaveData>();
    }

    // Reset all data
    public void ResetSaveData()
    {
        // Create a brand new, empty data object
        GameSaveFile emptyFile = new GameSaveFile();

        // Convert it to JSON
        string json = JsonUtility.ToJson(emptyFile, true);

        // Overwrite the existing file
        string path = GetPath(currentActiveSlot);
        File.WriteAllText(path, json);

        Debug.Log("Save data has been reset to empty.");
    }

    public void UpdateUnitInSave(UnitSaveData newData)
    {
        // FIX: Always ensure we have the LATEST data from the disk before modifying it
        // If we don't do this, we are editing an empty list and overwriting the file
        string path = GetPath(currentActiveSlot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveFile = JsonUtility.FromJson<GameSaveFile>(json);
        }

        // Safety check if file didn't exist or was empty
        if (saveFile == null) saveFile = new GameSaveFile();
        if (saveFile.hiredAdventurers == null) saveFile.hiredAdventurers = new List<UnitSaveData>();

        int existingIndex = saveFile.hiredAdventurers.FindIndex(data => data.unitID == newData.unitID);

        if (existingIndex != -1)
        {
            saveFile.hiredAdventurers[existingIndex] = newData;
        }
        else
        {
            saveFile.hiredAdventurers.Add(newData);
        }

        SaveToFile();
    }

    public void ClearCurrentSaveData()
    {
        if (saveFile == null)
        {
            saveFile = new GameSaveFile();
            return;
        }

        // 1. Clear the list of adventurers
        saveFile.hiredAdventurers.Clear();

        // 2. Reset other data points
        saveFile.saveTimestamp = "No Save Data";

        // 3. Reset any other variables you have (e.g., Gold, Level, etc.)
        // saveFile.playerGold = 0;

        Debug.Log("Save file data cleared in memory.");
    }

    // Helper to get the correct path based on the slot
    private string GetPath(int slot) => Application.persistentDataPath + "/save_" + slot + ".json";

    // Helper method to handle the actual writing to disk
    public void SaveToFile()
    {
        // Fix 1: Ensure saveFile actually exists in memory
        if (saveFile == null)
        {
            saveFile = new GameSaveFile();
        }

        saveFile.saveTimestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Fix 2: Safety check for ProgressManager
        if (ProgressManager.Instance != null)
        {
            saveFile.gold = ProgressManager.Instance.gold;
            saveFile.week = ProgressManager.Instance.week;
            saveFile.rating = ProgressManager.Instance.rating;
        }
        else
        {
            Debug.LogError("SaveManager: ProgressManager.Instance is null! Cannot save gold.");
        }

        // NEW: Save the shared Stash items
        if (InventoryManager.Instance != null && InventoryManager.Instance.stash != null)
        {
            saveFile.stashItems = new List<ItemSaveData>();
            foreach (ItemUI ui in InventoryManager.Instance.stash.itemsInContainer)
            {
                saveFile.stashItems.Add(ui.data);
            }
        }

        string json = JsonUtility.ToJson(saveFile, true);
        File.WriteAllText(GetPath(currentActiveSlot), json);
    }

    public void SetSaveSlot(int slot)
    {
        currentActiveSlot = slot;
    }

    public void LoadProgress() // Currently only gold loaded, add guildrank, guildrankprogress, etc. later
    {
        string path = GetPath(currentActiveSlot);

        if (File.Exists(path))
        {
            // 1. Read the file
            string json = File.ReadAllText(path);

            // 2. Parse it into a temporary object
            GameSaveFile tempSave = JsonUtility.FromJson<GameSaveFile>(json);

            // 3. Only apply the gold value to your game manager
            if (ProgressManager.Instance != null)
            {
                ProgressManager.Instance.gold = tempSave.gold;
                ProgressManager.Instance.week = tempSave.week;
                ProgressManager.Instance.rating = tempSave.rating;

                ProgressUI ui = FindFirstObjectByType<ProgressUI>();
                if (ui != null) ui.UpdateProgressText();
            }
        }
        else
        {
            // If no file exists, set the default starting gold
            if (ProgressManager.Instance != null)
            {
                ProgressManager.Instance.gold = 500; // Your starting amount
                ProgressManager.Instance.week = 0;
                ProgressManager.Instance.rating = 3f;
                Debug.Log("No save found.");
            }
        }
    }

    public void DeleteUnitFromSave(string unitID)
    {
        string path = GetPath(currentActiveSlot);

        if (!File.Exists(path)) return;

        // 1. Load the data into a temporary local variable
        string json = File.ReadAllText(path);
        GameSaveFile tempSave = JsonUtility.FromJson<GameSaveFile>(json);

        if (tempSave.hiredAdventurers != null)
        {
            // 2. Remove the specific unit from the local list
            int initialCount = tempSave.hiredAdventurers.Count;
            tempSave.hiredAdventurers.RemoveAll(u => u.unitID == unitID);

            // 3. Only save back to disk if something was actually removed
            if (tempSave.hiredAdventurers.Count < initialCount)
            {
                string updatedJson = JsonUtility.ToJson(tempSave, true);
                File.WriteAllText(path, updatedJson);
                Debug.Log($"Unit {unitID} removed and file updated.");
            }
        }
    }

    public void DeleteSaveFile(int slot)
    {
        string path = GetPath(slot);

        if (File.Exists(path))
        {
            File.Delete(path);

            // Optional: If the deleted file was the one currently loaded in memory, 
            // reset the local saveFile object so you don't accidentally save old data back out.
            if (slot == currentActiveSlot)
            {
                saveFile = new GameSaveFile();
            }

            Debug.Log($"Save file in slot {slot} has been deleted.");
        }
        else
        {
            Debug.LogWarning($"Attempted to delete save in slot {slot}, but no file exists at: {path}");
        }
    }

    public bool IsPartyFull(int partyId, int limit)
    {
        // 0 usually means "No Party", so we don't limit that
        if (partyId == 0) return false;

        // Load the latest data if necessary, or use the existing saveFile
        if (saveFile == null || saveFile.hiredAdventurers == null) return false;

        // Count how many units have this party ID
        int count = saveFile.hiredAdventurers.Count(u => u.partyNum == partyId);

        return count >= limit;
    }

    public void ProcessGuildRest()
    {
        // 1. Ensure we have the most recent data from the file
        string path = GetPath(currentActiveSlot);
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        saveFile = JsonUtility.FromJson<GameSaveFile>(json);

        if (saveFile.hiredAdventurers == null || saveFile.hiredAdventurers.Count == 0) return;

        // 2. Iterate through the DATA (not GameObjects)
        foreach (UnitSaveData data in saveFile.hiredAdventurers)
        {
            // Since the logic is inside the Unit class, we mimic it here.
            // We calculate the gain and update the data object directly.
            data.condition = CalculateConditionGain(data.condition);
        }

        // 3. Save the modified list back to the disk
        SaveToFile();
        Debug.Log($"Guild Rest Complete: {saveFile.hiredAdventurers.Count} adventurers recovered condition.");
    }

    public float CalculateConditionGain(float condition)
    {
        float gain = UnityEngine.Random.Range(70, 100);
        return Mathf.Min(condition + gain, 100);
    }

    public bool IsAdventurerConditionZero()
    {
        if (saveFile == null || saveFile.hiredAdventurers == null)
        {
            Debug.LogWarning("SaveManager: No data to check condition.");
            return false;
        }

        bool hasExhaustedMember = saveFile.hiredAdventurers
            .Where(u => u.partyNum == QuestManager.Instance.selectedPartyNum)
            .Any(u => u.condition <= 0);

        return hasExhaustedMember;
    }
}
