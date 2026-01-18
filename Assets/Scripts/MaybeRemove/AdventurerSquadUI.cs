using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerSquadUI : Adventurer
{
    public GameObject selectionPanel;

    [Header("UI References")]
    [SerializeField] protected Sprite defaultImage;
    [SerializeField] protected Image image;

    private void Awake()
    {
        image.sprite = defaultImage;
    }

    public void DisplaySquadUI()
    {
        image.sprite = library.allPossibleSprites[spriteID];
    }

    // 1. Called when you click this button to open the list
    public void OpenSelectionPanel()
    {
        SelectionManager.pendingButton = this;
        Debug.Log("Pending button has been set to: " + gameObject.name);
        selectionPanel.SetActive(true);
    }

    // 2. Called automatically by the SelectionSystem after a choice is made
    public void ReceiveUnitData(UnitSaveData data)
    {
        Debug.Log("Main button successfully received data for: " + data.unitName);

        LoadFromData(data);
        DisplaySquadUI();

        selectionPanel.SetActive(false);
    }

    public void RemoveAdventurerFromSquad()
    {
        image.sprite = defaultImage;
        ClearData();
    }

    public override void RandomizeStats()
    {
        // Nothing
    }

    public override void RandomName()
    {
        // Nothing also :)
    }
}
