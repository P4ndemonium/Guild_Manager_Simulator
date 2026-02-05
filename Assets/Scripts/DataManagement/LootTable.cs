using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootEntry
{
    public ItemData blueprint;
    public int weight; // Higher weight = more common
    public Rarity minRarity = Rarity.Common;
    public Rarity maxRarity = Rarity.Rare;
}

[CreateAssetMenu(menuName = "Items/Loot Table")]
public class LootTable : ScriptableObject
{
    public List<LootEntry> possibleDrops;

    public ItemSaveData GetRandomDrop()
    {
        // 1. Calculate total weight
        int totalWeight = 0;
        possibleDrops.ForEach(x => totalWeight += x.weight);

        // 2. Roll a random number
        int roll = Random.Range(0, totalWeight);
        int currentWeight = 0;

        foreach (var entry in possibleDrops)
        {
            currentWeight += entry.weight;
            if (roll < currentWeight)
            {
                // 3. Roll a random rarity for this specific drop
                Rarity rolledRarity = (Rarity)Random.Range((int)entry.minRarity, (int)entry.maxRarity + 1);
                return new ItemSaveData(entry.blueprint, rolledRarity);
            }
        }
        return null;
    }
}
