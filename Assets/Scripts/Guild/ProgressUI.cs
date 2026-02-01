using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI weekText;
    [SerializeField] private TextMeshProUGUI ratingText;

    private void Start()
    {
        UpdateProgressText();
    }

    public void UpdateProgressText()
    {
        if (goldText != null) goldText.text = $"Gold: {ProgressManager.Instance.gold.ToString("N0")}"; // N0 adds commas to numbers
        if (weekText != null) weekText.text = $"Week: {ProgressManager.Instance.week.ToString("N0")}";
        if (ratingText != null) ratingText.text = $"Rating: {ProgressManager.Instance.rating.ToString()}";
    }
}
