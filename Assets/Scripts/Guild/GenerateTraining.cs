using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GenerateTraining : MonoBehaviour
{
    public static GenerateTraining Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public List<TrainingOption> allAvailableTraining; // The "Container" of variables
    public Button[] selectionButtons; // Drag your 3 buttons here
    public GameObject trainingPanel;

    private List<TrainingOption> selectedOptions = new List<TrainingOption>();
    public SelectorUI currentSelector;

    void Start()
    {
        GenerateWeightedOptions();
    }

    public void GenerateWeightedOptions()
    {
        foreach (Button btn in selectionButtons)
        {
            btn.gameObject.SetActive(true);
        }

        List<TrainingOption> tempPool = new List<TrainingOption>(allAvailableTraining);

        for (int i = 0; i < selectionButtons.Length; i++)
        {
            int totalWeight = 0;
            foreach (var p in tempPool) totalWeight += p.rarityWeight;

            int roll = Random.Range(0, totalWeight);
            int currentWeightSum = 0;

            for (int j = 0; j < tempPool.Count; j++)
            {
                currentWeightSum += tempPool[j].rarityWeight;
                if (roll < currentWeightSum)
                {
                    TrainingOption chosen = tempPool[j];

                    // CHANGE HERE: Create the result wrapper before setting up the button
                    TrainingResult rolledResult = new TrainingResult(chosen);

                    SetupButton(selectionButtons[i], rolledResult);

                    tempPool.RemoveAt(j);
                    break;
                }
            }
        }
    }

    void SetupButton(Button btn, TrainingResult result)
    {
        string descriptionText = $"<b>{result.originalOption.trainingName}</b>\n";

        // 1. Show the knowns
        foreach (var change in result.visibleResults)
        {
            // Determine the sign (+ or -)
            string sign = change.minAmount >= 0 ? "+" : "";
            // Pick a color: Green for bonus, Red for penalty
            string colorHex = change.minAmount >= 0 ? "#55FF55" : "#FF5555";

            if (change.stat == StatType.ReshuffleAll)
            {
                descriptionText += "<color=#FF00FF>Randomly redistribute all stat points</color>\n";
            }
            else if (change.stat == StatType.AllOtherStats)
            {
                descriptionText += $"All Other Stats: {sign}{change.minAmount}\n";
            }
            else
            {
                descriptionText += $"{change.stat}: {sign}{change.minAmount}\n";
            }
        }

        // 2. Show the mysteries (without revealing the stat or the number)
        foreach (var hidden in result.hiddenResults)
        {
            // Determine the sign (+ or -)
            string sign = hidden.minAmount >= 0 ? "+" : "";
            // Pick a color: Green for bonus, Red for penalty
            string colorHex = hidden.minAmount >= 0 ? "#55FF55" : "#FF5555";

            descriptionText += $"<color={colorHex}>Random Stat: {sign}{hidden.minAmount}</color>\n";
        }

        btn.GetComponentInChildren<TextMeshProUGUI>().text = descriptionText;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => {
            currentSelector.ApplyHiddenResult(result);
            trainingPanel.SetActive(false);
            btn.gameObject.SetActive(false);
            AdventurerInfoPanel.Instance.DisplayAdventurerInformation(currentSelector);
            AdventurerInfoPanel.Instance.SaveUnit();
        });
    }

    //void OnOptionSelected(TrainingOption selected, SelectorUI selectorData)
    //{
    //    Debug.Log("You picked: " + selected.trainingName);

    //    selectorData.ApplyTraining(selected);

    //    // Close the menu
    //    trainingPanel.SetActive(false);
    //}
}
