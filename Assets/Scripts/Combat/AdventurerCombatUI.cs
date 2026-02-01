using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerCombatUI : Adventurer
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI statsTextLeft;
    [SerializeField] private TextMeshProUGUI statsTextRight;

    void Start()
    {
        DisplayStats();
    }

    public void DisplayStats()
    {
        image.sprite = library.allPossibleSprites[spriteID];

        if (nameText != null && statsTextLeft != null && statsTextRight != null)
        {
            nameText.text = unitName;

            healthBar.fillAmount = currentHealth / maxHealth;
            healthText.text = $"HP: {Mathf.CeilToInt(currentHealth)} / {maxHealth}";

            statsTextLeft.text = $"STR: {STR}\n" +
                                 $"DEX: {DEX}\n" +
                                 $"VIT: {VIT}\n" +
                                 $"SPI: {SPI}";

            statsTextRight.text = $"INT: {INT}\n" +
                                  $"WIS: {WIS}\n" +
                                  $"END: {END}\n" +
                                  $"AGI: {AGI}";
        }
    }
}
