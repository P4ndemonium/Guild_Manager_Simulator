using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AdventurerInfoPanel : MonoBehaviour
{
    public static AdventurerInfoPanel Instance;

    public Image image;
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI STRText;
    public Image STRBar;
    public TextMeshProUGUI INTText;
    public Image INTBar;
    public TextMeshProUGUI DEXText;
    public Image DEXBar;
    public TextMeshProUGUI WISText;
    public Image WISBar;
    public TextMeshProUGUI VITText;
    public Image VITBar;
    public TextMeshProUGUI ENDText;
    public Image ENDBar;
    public TextMeshProUGUI SPIText;
    public Image SPIBar;
    public TextMeshProUGUI AGIText;
    public Image AGIBar;
    public TextMeshProUGUI GROText;
    public Image GROBar;

    public TextMeshProUGUI ageText;

    public GameObject panelRoot; // The actual UI Panel object to show/hide
    public float maxValue = 250f;
    public TextMeshProUGUI partyNumText;
    private SelectorUI currentSelector;

    void Awake()
    {
        Instance = this;
        panelRoot.SetActive(false); // Hide on start
    }

    public void DisplayAdventurerInformation(SelectorUI data)
    {
        // 1. Unsubscribe from the OLD unit first (to prevent memory leaks)
        if (currentSelector != null)
            currentSelector.OnStatsChanged -= RefreshUI;

        currentSelector = data;

        // 2. Subscribe to the NEW unit
        currentSelector.OnStatsChanged += RefreshUI;

        panelRoot.SetActive(true);
        RefreshUI(); // Initial update
    }

    // This runs every time the unit "screams" that its stats changed
    private void RefreshUI()
    {
        if (currentSelector == null) return;

        panelRoot.SetActive(true);

        image.sprite = currentSelector.SImage.sprite;
        nameText.text = currentSelector.SNameText.text;
        ageText.text = "Age: " + currentSelector.Age.ToString("F0");
        partyNumText.text = currentSelector.PartyNum.ToString("F0");

        // 1. Set the global animation settings
        float duration = 0.6f;
        Ease easeType = Ease.OutQuad;

        // 2. Animate each stat using a helper (defined below)
        AnimateStat(currentSelector.pSTR, STRBar, STRText, duration, easeType);
        AnimateStat(currentSelector.pINT, INTBar, INTText, duration, easeType);
        AnimateStat(currentSelector.pDEX, DEXBar, DEXText, duration, easeType);
        AnimateStat(currentSelector.pWIS, WISBar, WISText, duration, easeType);
        AnimateStat(currentSelector.pVIT, VITBar, VITText, duration, easeType);
        AnimateStat(currentSelector.pEND, ENDBar, ENDText, duration, easeType);
        AnimateStat(currentSelector.pSPI, SPIBar, SPIText, duration, easeType);
        AnimateStat(currentSelector.pAGI, AGIBar, AGIText, duration, easeType);
        AnimateStat(currentSelector.pGRO, GROBar, GROText, duration, easeType);
    }

    private void AnimateStat(float targetValue, Image bar, TextMeshProUGUI text, float duration, Ease ease)
    {
        bar.DOKill();
        text.DOKill();

        // 1. Determine if it's a buff or a debuff
        float startValue = bar.fillAmount * maxValue;
        Color feedbackColor = Color.white; // Default color

        if (targetValue > startValue + 0.1f) // Small threshold to avoid micro-flickers
            feedbackColor = Color.green;
        else if (targetValue < startValue - 0.1f)
            feedbackColor = Color.red;

        // 2. Animate the Color Punch
        // Change to green/red immediately, then fade back to white over the duration
        text.color = feedbackColor;
        text.DOColor(Color.white, duration).SetEase(Ease.InQuad);

        // 3. Animate the Fill Bar
        DOTween.To(() => bar.fillAmount, x => bar.fillAmount = x, targetValue / maxValue, duration).SetEase(ease);

        // 4. Animate the Text Number
        DOTween.To(() => startValue, x => {
            text.text = x.ToString("F0");
        }, targetValue, duration).SetEase(ease);

        // 5. Optional: Subtle "Punch" Scale (makes the text pop out slightly)
        if (feedbackColor != Color.white)
        {
            text.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f);
        }
    }

    public void UpParty()
    {
        currentSelector.IncParty();
        SaveUnit();
    }
    public void DownParty()
    {
        currentSelector.DecParty();
        SaveUnit();
    }

    public void SaveUnit()
    {
        partyNumText.text = currentSelector.PartyNum.ToString("F0");
        currentSelector.DisplaySelector();
        UnitSaveData freshData = currentSelector.SaveToData();
        SaveManager.Instance.UpdateUnitInSave(freshData);
    }
}
