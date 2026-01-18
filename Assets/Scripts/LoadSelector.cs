using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSelector : MonoBehaviour
{
    // Prefab
    public GameObject adventurerSelector;

    public RectTransform content;


    void Awake()
    {
        foreach (UnitSaveData unitData in SaveManager.Instance.saveFile.hiredAdventurers)
        {
            GameObject newPanel = Instantiate(adventurerSelector, content);
            newPanel.GetComponent<AdventurerSelectorUI>().LoadFromData(unitData);
            newPanel.GetComponent<AdventurerSelectorUI>().Setup(unitData);
        }
    }
}
