using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectorUI : Adventurer, IPointerClickHandler
{
    [Header("UI References")]
    [SerializeField] protected Image sImage;
    [SerializeField] protected TextMeshProUGUI sNameText;
    [SerializeField] protected TextMeshProUGUI sPartyNumText;
    [SerializeField] protected Image ConditionBar;
    [SerializeField] protected TextMeshProUGUI ConditionText;

    public Image SImage => sImage;
    public TextMeshProUGUI SNameText => sNameText;

    public int pSTR => STR;
    public int pINT => INT;
    public int pDEX => DEX;
    public int pWIS => WIS;
    public int pVIT => VIT;
    public int pEND => END;
    public int pSPI => SPI;
    public int pAGI => AGI;
    public int pGRO => GRO;

    public int Age => age;
    //public int PartyNum => partyNum;

    void Start()
    {
        DisplaySelector();
    }

    public void DisplaySelector()
    {
        sImage.sprite = library.allPossibleSprites[spriteID];
        sNameText.text = unitName;
        sPartyNumText.text = partyNum.ToString();

        float duration = 0.5f;
        float targetFill = condition / 100;

        ConditionBar.DOKill();
        DOTween.To(() => ConditionBar.fillAmount, x => ConditionBar.fillAmount = x, targetFill, duration).SetEase(Ease.OutQuad);

        ConditionBar.DOKill();
        float startC = condition;

        DOTween.To(() => startC, x => {
            ConditionText.text = $"{Mathf.CeilToInt(x)}%";
        }, condition, duration);

        if (targetFill < 0.25f)
            ConditionBar.DOColor(Color.red, duration);
        else
            ConditionBar.DOColor(Color.green, duration);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AdventurerInfoPanel.Instance.DisplayAdventurerInformation(this);
        GenerateTraining.Instance.currentSelector = this;
        InventoryManager.Instance.currentSelector = this;
        InventoryManager.Instance.SwitchAdventurer();
    }

    public void SetPartyNum(int num)
    {
        partyNum = num;
    }
}
