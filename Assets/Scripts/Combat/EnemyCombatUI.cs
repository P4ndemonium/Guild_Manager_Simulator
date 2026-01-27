using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombatUI : Enemy
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
        //image.sprite = library.allPossibleSprites[spriteID]; // undo when enemy sprites are done

        if (nameText != null && statsTextLeft != null && statsTextRight != null)
        {
            nameText.text = unitName;

            healthBar.fillAmount = currentHealth / maxHealth;
            healthText.text = $"HP: {(int)currentHealth} / {maxHealth}";

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
