using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity { Common, Uncommon, Rare, Legendary, Unique }

[System.Serializable]
public class ItemSaveData
{
    public string blueprintID; // Used to find the ScriptableObject to get the Sprite/Name
    public Rarity rarity;
    public StatModifier mainStat;
    public List<StatModifier> subStats = new List<StatModifier>();
    public string uniqueEffect;

    // This constructor generates a random item based on a blueprint
    public ItemSaveData(ItemData blueprint, Rarity r)
    {
        blueprintID = blueprint.itemID;
        rarity = r;

        // 1. Roll Main Stat (Flat)
        mainStat = new StatModifier(blueprint.forcedMainStat, UnityEngine.Random.Range(5, 16), false);

        // 2. Roll Substats
        int subStatCount = GetCountByRarity(r);
        for (int i = 0; i < subStatCount; i++)
        {
            StatType randomType = GetRandomCoreStat();
            bool isPercent = UnityEngine.Random.value > 0.8f;
            float val = isPercent ? UnityEngine.Random.Range(0.05f, 0.15f) : UnityEngine.Random.Range(1, 6);

            subStats.Add(new StatModifier(randomType, val, isPercent));
        }

        // 3. Handle Unique Logic
        if (r == Rarity.Unique)
        {
            uniqueEffect = blueprint.uniqueEffectDescription;
        }
    }

    private int GetCountByRarity(Rarity r) => r switch
    {
        Rarity.Common => 1,
        Rarity.Uncommon => 2,
        Rarity.Rare => 3,
        Rarity.Legendary => 4,
        Rarity.Unique => 4,
        _ => 0
    };

    private StatType GetRandomCoreStat()
    {
        // Using the 8 core stats you defined earlier
        StatType[] core = { StatType.STR, StatType.INT, StatType.DEX, StatType.WIS,
                            StatType.VIT, StatType.END, StatType.SPI, StatType.AGI };
        return core[UnityEngine.Random.Range(0, core.Length)];
    }
}
