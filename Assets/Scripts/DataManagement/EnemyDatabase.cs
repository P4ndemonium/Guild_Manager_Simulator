using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyDatabase : MonoBehaviour
{
    public List<UnitSaveData> enemyDatabase = new List<UnitSaveData>();

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(enemyDatabase, true); // 'true' makes it readable
        string path = Path.Combine(Application.dataPath, "EnemyData.json");

        File.WriteAllText(path, json);
        Debug.Log($"<color=green>Enemy Database auto-saved to:</color> {path}");

        // Refresh the Project window so the file appears immediately
        #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
        #endif
    }
}
