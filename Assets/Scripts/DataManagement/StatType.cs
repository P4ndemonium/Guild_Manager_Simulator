using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { STR, INT, DEX, WIS, VIT, END, SPI, AGI, RandomStat, AllOtherStats, ReshuffleAll }

public static class StatHelpers
{
    // A static reference to only the stats allowed on items
    public static readonly StatType[] CoreStats = {
        StatType.STR, StatType.INT, StatType.DEX, StatType.WIS,
        StatType.VIT, StatType.END, StatType.SPI, StatType.AGI
    };
}

[System.Serializable]
public class StatModifier
{
    public StatType type;
    public float value;
    public bool isPercent;

    public StatModifier(StatType t, float v, bool p) { type = t; value = v; isPercent = p; }
}

[System.Serializable]
public class Stat
{
    public float BaseValue;
    private List<StatModifier> modifiers = new List<StatModifier>();

    public float GetValue()
    {
        float flatSum = 0;
        float percentSum = 0;

        foreach (var mod in modifiers)
        {
            if (mod.isPercent) percentSum += mod.value;
            else flatSum += mod.value;
        }

        return (BaseValue + flatSum) * (1 + percentSum);
    }

    public void AddModifier(StatModifier mod) => modifiers.Add(mod);
    public void RemoveModifier(StatModifier mod) => modifiers.Remove(mod);
}
