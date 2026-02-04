using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { STR, INT, DEX, WIS, VIT, END, SPI, AGI, RandomStat, AllOtherStats, ReshuffleAll }

[System.Serializable]
public class StatRange
{
    public StatType statType;
    public int minValue;
    public int maxValue;
}
