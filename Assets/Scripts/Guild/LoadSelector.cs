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
        SaveManager.Instance.OnLoadButtonPressed();

        foreach (UnitSaveData unitData in SaveManager.Instance.saveFile.hiredAdventurers)
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
