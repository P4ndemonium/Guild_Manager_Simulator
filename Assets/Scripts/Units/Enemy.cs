using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public LootTable dropTable;

    // Start is called before the first frame update
    void Start()
    {
        unitTeam = Team.Enemy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        base.Die();
        if (dropTable != null)
        {
            ItemSaveData droppedItem = dropTable.GetRandomDrop();
            // Logic to spawn a physical chest or add directly to player
        }
    }

    public override void RandomizeStats()
    {
        // We calculate the min as (Current - 100), but cap it at 1.
        // The max is (Current + 1), because Random.Range(int, int) is exclusive of the max.

        STR = Random.Range(Mathf.Max(1, STR - 100), STR + 1);
        INT = Random.Range(Mathf.Max(1, INT - 100), INT + 1);
        DEX = Random.Range(Mathf.Max(1, DEX - 100), DEX + 1);
        WIS = Random.Range(Mathf.Max(1, WIS - 100), WIS + 1);
        VIT = Random.Range(Mathf.Max(1, VIT - 100), VIT + 1);
        END = Random.Range(Mathf.Max(1, END - 100), END + 1);
        SPI = Random.Range(Mathf.Max(1, SPI - 100), SPI + 1);
        AGI = Random.Range(Mathf.Max(1, AGI - 100), AGI + 1);

        CalculateStats();
    }
}
