using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;
    public string guildRank;
    public int guildRankProgress;

    // ================================================================
    // AUTO UPDATES UI IF GOLD CHANGES ANYWHERE
    private int _gold;
    public int gold
    {
        get => _gold;
        set
        {
            _gold = value;
            // Every time gold is changed (e.g. gold -= 10), 
            // it automatically tries to update the UI
            FindFirstObjectByType<GoldUI>()?.UpdateGoldText();
        }
    }
    // ================================================================

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
}
