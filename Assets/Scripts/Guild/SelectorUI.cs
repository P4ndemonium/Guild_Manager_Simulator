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
    public int PartyNum => partyNum;

    void Start()
    {
        DisplaySelector();
    }

    public void DisplaySelector()
    {
        sImage.sprite = library.allPossibleSprites[spriteID];
        sNameText.text = unitName;
        sPartyNumText.text = partyNum.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AdventurerInfoPanel.Instance.DisplayAdventurerInformation(this);

        GenerateTraining.Instance.currentSelector = this;
    }
}
