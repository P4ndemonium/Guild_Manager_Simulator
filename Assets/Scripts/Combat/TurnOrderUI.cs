using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TurnOrderUI : MonoBehaviour
{
    public GameObject iconPrefab; // Drag your Prefab here
    public Transform container;   // The Vertical Layout Group transform

    public void UpdateDisplay(List<Unit> allUnits, int previewCount = 6)
    {
        // 1. Clear old icons
        foreach (Transform child in container) Destroy(child.gameObject);

        // 2. Create a temporary list of "Simulated" turn data
        var simulation = allUnits
            .Where(u => u != null && !u.IsDead)
            .Select(u => new { Unit = u, Time = u.nextActionTime })
            .ToList();

        List<Unit> displayOrder = new List<Unit>();

        // 3. Predict the next X turns
        for (int i = 0; i < previewCount; i++)
        {
            // Find who moves next in our simulation
            var next = simulation.OrderBy(s => s.Time).First();
            displayOrder.Add(next.Unit);

            // Update the simulated time for this unit (don't touch the real unit!)
            int index = simulation.FindIndex(s => s.Unit == next.Unit);
            simulation[index] = new { next.Unit, Time = next.Time + next.Unit.ActionCost };
        }

        // 4. Instantiate the icons
        foreach (Unit u in displayOrder)
        {
            GameObject icon = Instantiate(iconPrefab, container);
            // Set icon image or text here
            Image portrait = icon.GetComponentInChildren<Image>();
            if (portrait != null && u.Library != null)
            {
                portrait.sprite = u.Library.allPossibleSprites[u.SpriteID];
            }

            icon.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = u.UnitName;
        }
    }
}
