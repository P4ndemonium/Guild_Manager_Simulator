using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] protected AllSpritesLibrary library;

    // Start is called before the first frame update
    void Start()
    {
        unitTeam = Team.Enemy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void RandomizeStats()
    {
        STR = Random.Range(1, STR + 1);
        INT = Random.Range(1, INT + 1);
        DEX = Random.Range(1, DEX + 1);
        WIS = Random.Range(1, WIS + 1);
        VIT = Random.Range(1, VIT + 1);
        END = Random.Range(1, END + 1);
        SPI = Random.Range(1, SPI + 1);
        AGI = Random.Range(1, AGI + 1);

        CalculateStats();
    }
}
