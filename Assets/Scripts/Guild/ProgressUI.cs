using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI monthText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TextMeshProUGUI guildRankText;

    private void Start()
    {
        UpdateProgressText();
    }

    public void UpdateProgressText()
    {
        if (goldText != null) goldText.text = $"Gold: {ProgressManager.Instance.gold.ToString("N0")}"; // N0 adds commas to numbers
        if (monthText != null) monthText.text = $"Month: {ProgressManager.Instance.month.ToString("N0")}";
        if (ratingText != null) ratingText.text = $"Rating: {ProgressManager.Instance.rating.ToString()}";
        if (guildRankText != null) guildRankText.text = $"Guild Rank: {ProgressManager.Instance.guildRank.ToString()}";
    }
}
