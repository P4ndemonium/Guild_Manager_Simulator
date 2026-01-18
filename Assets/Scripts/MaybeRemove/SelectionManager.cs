using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectionManager 
{
    // This is the button currently 'waiting' for a unit to be picked
    public static AdventurerSquadUI pendingButton;

    public static void SelectUnit(UnitSaveData data)
    {
        Debug.Log("SelectionManager received unit: " + data.unitName);

        if (pendingButton != null)
        {
            Debug.Log("Passing data to pending button...");
            pendingButton.ReceiveUnitData(data);
        }
        else
        {
            Debug.LogError("No button was waiting for data! (pendingButton is null)");
        }
    }
}
