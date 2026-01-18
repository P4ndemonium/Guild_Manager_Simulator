using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerSelectorUI : Adventurer
{
    [Header("UI References")]
    [SerializeField] protected Image image;
    [SerializeField] protected TextMeshProUGUI nameText;

    private UnitSaveData _myData;

    void Start()
    {
        DisplaySelector();
    }

    public void DisplaySelector()
    {
        image.sprite = library.allPossibleSprites[spriteID];
        nameText.text = unitName;
    }

    public void Setup(UnitSaveData data)
    {
        _myData = data;
    }

    // Link this to the Button component's OnClick in the Prefab
    public void OnClickSelectThisUnit()
    {
        SelectionManager.SelectUnit(_myData);
    }
}
