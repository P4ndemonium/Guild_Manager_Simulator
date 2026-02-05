using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Text;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private CanvasGroup canvasGroup;

    private RectTransform rectTransform;

    void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup.alpha = 0; // Start hidden
    }

    void Update()
    {
        // Follow mouse position
        Vector2 mousePos = Input.mousePosition;
        // Offset slightly so it's not directly under the cursor
        rectTransform.position = mousePos + new Vector2(15f, -15f);
    }

    public void Show(ItemSaveData data)
    {
        Debug.Log($"Showing Tooltip for {data.blueprintID}");

        ItemData bp = ItemDatabase.GetBlueprint(data.blueprintID);
        if (bp == null) return;

        // 1. Title and Rarity Color
        titleText.text = bp.baseName;
        titleText.color = GetRarityColor(data.rarity);

        // 2. Build the Stat String
        StringBuilder sb = new StringBuilder();

        // Rarity Label
        sb.AppendLine($"<size=80%>{data.rarity.ToString().ToUpper()}</size>");
        sb.AppendLine();

        // Main Stat (Always shown)
        sb.AppendLine($"<b>{FormatStat(data.mainStat)}</b> (Main)");
        sb.AppendLine("<color=#777777>-------------------------</color>");

        // Substats
        foreach (var sub in data.subStats)
        {
            sb.AppendLine(FormatStat(sub));
        }

        // Unique Effect
        if (data.rarity == Rarity.Unique && !string.IsNullOrEmpty(data.uniqueEffect))
        {
            sb.AppendLine();
            sb.AppendLine($"<color=#FFD700><i>{data.uniqueEffect}</i></color>");
        }

        statsText.text = sb.ToString();

        // 3. DOTween Animation
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, 0.15f).SetUpdate(true); // SetUpdate(true) works even if game is paused
    }

    // Helper to format StatModifier into "STR +5" or "INT +10%"
    private string FormatStat(StatModifier mod)
    {
        // Use 'type' instead of 'stat' to match your class definition
        string statName = mod.type.ToString();

        // Determine if we need a + or - sign
        string prefix = mod.value >= 0 ? "+" : "";

        // Determine the suffix (% or empty)
        string suffix = mod.isPercent ? "%" : "";

        // Calculation: 
        // Flat: just the value (e.g., 5)
        // Percent: value * 100 (e.g., 0.12 becomes 12)
        float displayVal = mod.isPercent ? mod.value * 100f : mod.value;

        // :F0 removes decimals (e.g., 12.00 becomes 12)
        // :F1 would keep one decimal if you want more precision for percentages
        return $"{statName} {prefix}{displayVal:F0}{suffix}";
    }

    // Color Palette for Rarity
    private Color GetRarityColor(Rarity r)
    {
        return r switch
        {
            Rarity.Common => Color.white,
            Rarity.Uncommon => new Color(0.12f, 0.8f, 0.12f), // Green
            Rarity.Rare => new Color(0.2f, 0.6f, 1f),        // Blue
            Rarity.Legendary => new Color(1f, 0.5f, 0f),     // Orange
            Rarity.Unique => new Color(1f, 0.84f, 0f),       // Gold
            _ => Color.white
        };
    }

    public void Hide()
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(0f, 0.1f);
    }
}
