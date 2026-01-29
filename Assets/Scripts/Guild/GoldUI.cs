using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        goldText.text = $"Gold: {ProgressManager.Instance.gold.ToString("N0")}"; // N0 adds commas to numbers
    }
}
