using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;
    public enum GuildRank { S, A, B, C, D, E, F }
    public GuildRank guildRank;
    public int guildRankProgress;
    public int year;
    public int month;

    // ================================================================
    // AUTO UPDATES UI IF CHANGES ANYWHERE
    // Gold
    private int _gold;
    public int gold
    {
        get => _gold;
        set
        {
            _gold = value;
            // Every time gold is changed (e.g. gold -= 10), 
            // it automatically tries to update the UI
            FindFirstObjectByType<ProgressUI>()?.UpdateProgressText();
        }
    }
    // Week
    private int _week;
    public int week
    {
        get => _week;
        set
        {
            _week = value;
            FindFirstObjectByType<ProgressUI>()?.UpdateProgressText();
            if (GenerateTraining.Instance != null) GenerateTraining.Instance.GenerateWeightedOptions();
        }
    }
    // Rating
    private float _rating;
    public float rating
    {
        get => _rating;
        set
        {
            _rating = value;
            FindFirstObjectByType<ProgressUI>()?.UpdateProgressText();
        }
    }
    // ================================================================

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        guildRank = GuildRank.F;
    }
}
