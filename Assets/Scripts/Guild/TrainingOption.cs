using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatChange
{
    public StatType stat;
    public int minAmount; // Use this as the base/fixed value
    public int maxAmount; // Use this for the upper range

    // Helper to check if it's a range or fixed
    public bool IsRandom => maxAmount > minAmount;
}

[CreateAssetMenu(fileName = "Training", menuName = "Training")]
public class TrainingOption : ScriptableObject
{
    public string trainingName;
    public string description;
    public int rarityWeight; // Example: Common = 100, Rare = 20, Legendary = 5

    public List<StatChange> changes;
}
