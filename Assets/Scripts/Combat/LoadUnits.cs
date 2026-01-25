using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUnits : MonoBehaviour
{
    public GameObject unitPanel;
    public RectTransform parent;


    void Start()
    {
        LoadAdventurers();
    }

    public void LoadAdventurers()
    {
        SaveManager.Instance.OnLoadButtonPressed();
        Debug.Log("1");

        foreach (UnitSaveData unitData in SaveManager.Instance.saveFile.hiredAdventurers)
        {
            Debug.Log("2");
            GameObject newPanel = Instantiate(unitPanel, parent);
            Debug.Log("3");

            if (newPanel.GetComponent<AdventurerCombatUI>() != null)
            {
                newPanel.GetComponent<AdventurerCombatUI>().LoadFromData(unitData);
                Debug.Log("4");
            }
        }
    }
}
