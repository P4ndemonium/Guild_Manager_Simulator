using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSelector : MonoBehaviour
{
    // Prefab
    public GameObject adventurerSelector;
    public RectTransform content;

    public void LoadAdventurers()
    {
        List<UnitSaveData> loadedUnits = SaveManager.Instance.GetAllUnitsFromSave();

        foreach (UnitSaveData unitData in loadedUnits)
        {
            GameObject newPanel = Instantiate(adventurerSelector, content);

            if (newPanel.GetComponent<SelectorUI>() != null)
            {
                newPanel.GetComponent<SelectorUI>().LoadFromData(unitData);
            }

            //newPanel.GetComponent<SelectorUI>().Setup(unitData);          // Previous selector functionality
        }
    }

    public void DeleteAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
