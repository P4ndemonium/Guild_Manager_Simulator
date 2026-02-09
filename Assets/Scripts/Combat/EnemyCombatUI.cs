using DG.Tweening;
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
        //image.sprite = library.allPossibleSprites[spriteID]; // No sprites for enemy yet

        if (nameText != null && statsTextLeft != null && statsTextRight != null)
        {
            nameText.text = unitName;

            // 1. ANIMATE THE HEALTH BAR
            float duration = 0.5f;
            float targetFill = currentHealth / maxHealth;

            // Kill any previous health animation to prevent flickering
            healthBar.DOKill();

            // Use DOTween to fill the bar
            DOTween.To(() => healthBar.fillAmount, x => healthBar.fillAmount = x, targetFill, duration).SetEase(Ease.OutQuad);

            healthBar.transform.DOShakePosition(0.2f, 10f, 10);

            // 2. ANIMATE THE HP TEXT (Counting numbers)
            // We'll parse the current text to find where to start counting from
            healthText.DOKill();
            float startHP = currentHealth; // Or use a variable to store previous health

            DOTween.To(() => startHP, x => {
                healthText.text = $"HP: {Mathf.CeilToInt(x)} / {maxHealth}";
            }, currentHealth, duration);

            // 3. OPTIONAL: CHANGE BAR COLOR BASED ON HP %
            if (targetFill < 0.25f)
                healthBar.DOColor(Color.red, duration);
            else
                healthBar.DOColor(Color.green, duration);

            statsTextLeft.text = $"STR: {EffectiveSTR}\n" +
                                 $"DEX: {EffectiveDEX}\n" +
                                 $"VIT: {EffectiveVIT}\n" +
                                 $"SPI: {EffectiveSPI}";

            statsTextRight.text = $"INT: {EffectiveINT}\n" +
                                  $"WIS: {EffectiveWIS}\n" +
                                  $"END: {EffectiveEND}\n" +
                                  $"AGI: {EffectiveAGI}";
        }
    }
}
