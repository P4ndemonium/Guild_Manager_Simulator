using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class EnemyListWrapper
{
    public List<UnitSaveData> enemies;
}

public class EnemyDatabase : MonoBehaviour
{
    public static EnemyDatabase Instance;
    public List<UnitSaveData> enemyDatabase = new List<UnitSaveData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveToJson()
    {
        EnemyListWrapper wrapper = new EnemyListWrapper();
        wrapper.enemies = enemyDatabase;
        string json = JsonUtility.ToJson(wrapper, true);
        string path = Path.Combine(Application.persistentDataPath, "EnemyData.json");

        File.WriteAllText(path, json);
        Debug.Log($"<color=green>Enemy Database auto-saved to:</color> {path}");

        // Refresh the Project window so the file appears immediately
        #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
        #endif
    }
}
