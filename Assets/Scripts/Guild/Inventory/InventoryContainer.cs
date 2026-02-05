using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryContainer : MonoBehaviour
{
    public Unit owner; // Set in Inspector for Bag, leave null for Stash
    public List<ItemUI> itemsInContainer = new List<ItemUI>();

    public void AddItem(ItemUI item)
    {
        if (!itemsInContainer.Contains(item))
        {
            itemsInContainer.Add(item);

            if (gameObject.name != "Stash")
            {
                owner = InventoryManager.Instance.currentSelector;
                if (owner != null && !owner.inventory.Contains(item.data))
                {
                    owner.inventory.Add(item.data);
                }
            }
            else
            {
                // If it IS the stash, we clear the owner 
                // so the item is no longer tied to an adventurer
                owner = null;
            }

            // Always save after any move to ensure the file matches the UI
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.OnSaveButtonPressedNOLOAD();
            }
        }
    }

    public void RemoveItem(ItemUI item)
    {
        if (itemsInContainer.Contains(item))
        {
            itemsInContainer.Remove(item);

            if (owner != null)
            {
                // Remove the data from the unit so they don't keep the item
                owner.inventory.Remove(item.data);

                // Save the fact that the unit no longer has this item
                if (SaveManager.Instance != null)
                {
                    SaveManager.Instance.OnSaveButtonPressedNOLOAD();
                }

                Debug.Log($"Removed {item.data.blueprintID} from {owner.UnitName}");
            }
        }
    }

    public void Clear()
    {
        // Loop backwards when destroying objects to avoid reference errors
        for (int i = itemsInContainer.Count - 1; i >= 0; i--)
        {
            if (itemsInContainer[i] != null)
            {
                Destroy(itemsInContainer[i].gameObject);
            }
        }
        itemsInContainer.Clear();
    }
}