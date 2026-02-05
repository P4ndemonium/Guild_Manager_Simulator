using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Keep the static class for easy global access
public static class ItemDatabase
{
    public static Dictionary<string, ItemData> AllBlueprints = new Dictionary<string, ItemData>();

    public static ItemData GetBlueprint(string id)
    {
        if (AllBlueprints.TryGetValue(id, out ItemData data)) return data;
        return null;
    }
}
