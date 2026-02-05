using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventoryContainer stash;
    public InventoryContainer adventurerBag;
    public GameObject itemPrefab;

    public SelectorUI currentSelector;

    void Awake() => Instance = this;

    void Start()
    {
        // Wait a frame or ensure ItemDatabaseManager Awake has run
        PrepopulateStash(5);
    }

    public void SwitchAdventurer()
    {
        //Debug.Log($"Switching! Selector: {currentSelector}, Bag: {adventurerBag}");
        if (currentSelector == null) return;
        if (adventurerBag == null) return;

        adventurerBag.Clear();
        adventurerBag.owner = currentSelector;

        foreach (var itemData in currentSelector.inventory)
        {
            SpawnItemInContainer(itemData, adventurerBag);
        }

        Debug.Log($"Loaded {currentSelector.inventory.Count} items for {currentSelector.UnitName}");
    }

    public void PrepopulateStash(int count)
    {
        // 1. Get all available blueprints
        List<ItemData> allBlueprints = new List<ItemData>(ItemDatabase.AllBlueprints.Values);
        if (allBlueprints.Count == 0) return;

        for (int i = 0; i < count; i++)
        {
            // 2. Pick a random blueprint
            ItemData randomBP = allBlueprints[Random.Range(0, allBlueprints.Count)];

            // 3. Roll a random rarity
            Rarity randomRarity = (Rarity)Random.Range(0, System.Enum.GetValues(typeof(Rarity)).Length);

            // 4. Use YOUR constructor to generate stats/substats
            ItemSaveData newData = new ItemSaveData(randomBP, randomRarity);

            // 5. Spawn the UI element
            SpawnItemInContainer(newData, stash);
        }
    }

    public void SpawnItemInContainer(ItemSaveData data, InventoryContainer container)
    {
        if (itemPrefab == null) return;

        // Instantiate with the container as the parent immediately
        GameObject go = Instantiate(itemPrefab, container.transform);

        ItemUI ui = go.GetComponent<ItemUI>();
        ui.data = data;

        // Standard Visual Setup
        ItemData bp = ItemDatabase.GetBlueprint(data.blueprintID);
        go.GetComponent<UnityEngine.UI.Image>().sprite = bp.icon;

        // Ensure scale is correct for UI
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;

        // Set a random position within the NEW parent's bounds
        rt.anchoredPosition = new Vector2(Random.Range(-100f, 100f), Random.Range(-100f, 100f));

        container.AddItem(ui);
    }
}
