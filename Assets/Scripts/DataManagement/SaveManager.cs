using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

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
        if (saveFile == null) saveFile = new GameSaveFile();

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
                ProgressManager.Instance.gold = 1000; // Your starting amount
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
}
