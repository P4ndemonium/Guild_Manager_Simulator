using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item Blueprint")]
public class ItemData : ScriptableObject
{
    public string itemID; // Unique ID to link SaveData back to this SO
    public string baseName;
    public Sprite icon;

    [Tooltip("This stat will always appear on this item type")]
    public StatType forcedMainStat;

    [Tooltip("The unique effect if this item rolls as a Unique rarity")]
    public string uniqueEffectDescription;
}
