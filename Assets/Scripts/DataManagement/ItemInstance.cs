using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RolledStat
{
    public StatType type;
    public int value;
}

[System.Serializable]
public class ItemInstance
{
    public ItemData data;
    public List<RolledStat> stats = new List<RolledStat>();

    public ItemInstance(ItemData blueprint)
    {
        data = blueprint;
        RollStats();
    }

    private void RollStats()
    {
        // 1. Create a copy of the possible stats pool
        List<StatRange> pool = new List<StatRange>(data.possibleStats);

        // 2. Pick 3 unique stats
        for (int i = 0; i < 3; i++)
        {
            if (pool.Count == 0) break; // Safety check

            int randomIndex = Random.Range(0, pool.Count);
            StatRange chosenRange = pool[randomIndex];

            // 3. Roll the value and add to this item
            stats.Add(new RolledStat
            {
                type = chosenRange.statType,
                value = Random.Range(chosenRange.minValue, chosenRange.maxValue + 1)
            });

            // 4. Remove from temporary pool so we don't pick the same stat twice
            pool.RemoveAt(randomIndex);
        }
    }
}
