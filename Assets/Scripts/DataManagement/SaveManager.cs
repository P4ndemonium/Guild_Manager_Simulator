using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; } // The global "hook"
    public GameSaveFile saveFile;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Call this to save the current state to a file
    public void OnSaveButtonPressed()
    {
        //GameSaveFile saveFile = new GameSaveFile();
        Unit[] units = FindObjectsOfType<Unit>();

        foreach (Unit u in units)
        {
            // ONLY save if the unit is hired
            if (u.IsHired)
            {
                UnitSaveData packedData = u.SaveToData();
                saveFile.hiredAdventurers.Add(packedData);
            }
        }
        SaveToFile();

        Debug.Log("Adventurers Saved! Adventurer Count: " + saveFile.hiredAdventurers.Count);
        Debug.Log("Save created at: " + saveFile.saveTimestamp);
    }

    // Call this to load data back into the scene
    public void OnLoadButtonPressed()
    {
        string path = Application.persistentDataPath + "/units.json";
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        saveFile = JsonUtility.FromJson<GameSaveFile>(json);

        // Logic here depends on if you want to update existing units 
        // or clear the scene and re-spawn them.


        Debug.Log("Data Loaded!");
    }

    // Reset all data
    public void ResetSaveData()
    {
        // Create a brand new, empty data object
        GameSaveFile emptyFile = new GameSaveFile();

        // Convert it to JSON
        string json = JsonUtility.ToJson(emptyFile, true);

        // Overwrite the existing file
        string path = Application.persistentDataPath + "/units.json";
        File.WriteAllText(path, json);

        Debug.Log("Save data has been reset to empty.");
    }

    public void UpdateUnitInSave(Unit u)
    {
        if (saveFile == null) saveFile = new GameSaveFile();

        UnitSaveData newData = u.SaveToData();

        // Find if this unit already exists in the list
        int existingIndex = saveFile.hiredAdventurers.FindIndex(data => data.unitID == newData.unitID);

        if (existingIndex != -1)
        {
            // Overwrite existing data
            saveFile.hiredAdventurers[existingIndex] = newData;
            Debug.Log($"Updated existing unit: {newData.unitID}");
        }
        else
        {
            // Add as new entry
            saveFile.hiredAdventurers.Add(newData);
            Debug.Log($"Added new unit to save: {newData.unitID}");
        }

        // Optional: Auto-save to disk immediately after the update
        SaveToFile();
    }

    // Helper method to handle the actual writing to disk
    private void SaveToFile()
    {
        saveFile.saveTimestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string json = JsonUtility.ToJson(saveFile, true);
        File.WriteAllText(Application.persistentDataPath + "/units.json", json);
        Debug.Log("File saved to disk.");
    }
}
