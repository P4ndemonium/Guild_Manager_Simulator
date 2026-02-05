using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabaseManager : MonoBehaviour
{
    [Header("Drop all ItemData ScriptableObjects here")]
    [SerializeField] private List<ItemData> masterItemList;

    void Awake()
    {
        // Clear the static dictionary to prevent duplicates on scene reload
        ItemDatabase.AllBlueprints.Clear();

        foreach (ItemData item in masterItemList)
        {
            if (item != null && !string.IsNullOrEmpty(item.itemID))
            {
                ItemDatabase.AllBlueprints[item.itemID] = item;
            }
            else
            {
                Debug.LogWarning($"Item {item?.name} is missing an itemID!");
            }
        }
    }
}
