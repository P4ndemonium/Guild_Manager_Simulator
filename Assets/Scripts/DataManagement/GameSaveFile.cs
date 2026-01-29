using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveFile
{
    public string saveTimestamp; // New field
    public int gold;
    public List<UnitSaveData> hiredAdventurers = new List<UnitSaveData>();
}
