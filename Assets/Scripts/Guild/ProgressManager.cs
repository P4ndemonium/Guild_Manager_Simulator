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

    // ================================================================
    // AUTO UPDATES UI IF CHANGES ANYWHERE
    // Gold
    [SerializeField] private int _gold;
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
    [SerializeField] private int _month;
    public int month
    {
        get => _month;
        set
        {
            _month = value;
            FindFirstObjectByType<ProgressUI>()?.UpdateProgressText();
            if (GenerateTraining.Instance != null) GenerateTraining.Instance.GenerateWeightedOptions();
        }
    }
    // Rating
    [SerializeField] private float _rating;
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

    public void NextMonth()
    {
        month += 1;
        SaveManager.Instance.ProcessGuildRest();

        if (month % 3 == 0) // every quarter
        {
            SaveManager.Instance.ProcessAdvanceQuarter();
        }

        if (month % 12 == 0) // every year
        {
            SaveManager.Instance.ProcessAdvanceYear();
        }
    }
}
