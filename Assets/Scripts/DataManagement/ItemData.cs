using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    // The pool of 8 potential stats for this specific weapon type
    public List<StatRange> possibleStats = new List<StatRange>();
}
