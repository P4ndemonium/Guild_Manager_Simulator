using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSaveData
{
    public Team unitTeam;
    public string unitID;
    public string unitName;
    public int STR;
    public int INT;
    public int DEX;
    public int WIS;
    public int VIT;
    public int END;
    public int SPI;
    public int AGI;
    public int GRO;
    public int age;
    public int spriteID;
    public int partyNum;
    public List<ItemSaveData> inventory = new List<ItemSaveData>();
}
