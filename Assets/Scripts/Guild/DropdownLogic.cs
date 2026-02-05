using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class DropdownLogic : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public void OnDropdownChanged(int index)
    {
        // 1. If no unit is selected, ignore the event
        if (AdventurerInfoPanel.Instance.currentSelector == null) return;

        // 2. If the dropdown index is ALREADY the unit's party, ignore it.
        // This stops the "forcing" behavior during RefreshUI.
        if (index == AdventurerInfoPanel.Instance.currentSelector.PartyNum) return;

        // 3. Check for full party
        if (index > 0 && SaveManager.Instance.IsPartyFull(index, QuestManager.Instance.partyLimit))
        {
            UnityEngine.Debug.LogWarning("Party Full!");
            // Revert UI without triggering this function again
            dropdown.SetValueWithoutNotify(AdventurerInfoPanel.Instance.currentSelector.PartyNum);
            dropdown.RefreshShownValue();
            return;
        }

        // 4. Finally, update and save
        QuestManager.Instance.selectedPartyNum = index;
        AdventurerInfoPanel.Instance.SaveUnit();
    }
}
