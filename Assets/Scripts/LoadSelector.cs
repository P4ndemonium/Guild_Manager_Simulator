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
        foreach (UnitSaveData unitData in SaveManager.Instance.saveFile.hiredAdventurers)
        {
            GameObject newPanel = Instantiate(adventurerSelector, content);
            newPanel.GetComponent<SelectorUI>().LoadFromData(unitData);
            //newPanel.GetComponent<SelectorUI>().Setup(unitData);          // Previous selector funcitonality
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
