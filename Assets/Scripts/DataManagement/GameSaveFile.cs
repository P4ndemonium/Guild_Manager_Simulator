using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveFile
{
    public string saveTimestamp; // New field
    public int gold;
    public int month;
    public float rating;
    public Rank guildRank;
    public List<ItemSaveData> stashItems = new List<ItemSaveData>();
    public List<UnitSaveData> hiredAdventurers = new List<UnitSaveData>();
}
