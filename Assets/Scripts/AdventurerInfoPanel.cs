using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public float maxValue = 200f;
    public TextMeshProUGUI squadNum;
    private SelectorUI currentSelector;

    void Awake()
    {
        Instance = this;
        panelRoot.SetActive(false); // Hide on start
    }

    public void DisplayAdventurerInformation(SelectorUI data)
    {
        currentSelector = data;
        panelRoot.SetActive(true);

        image.sprite = data.SImage.sprite;
        nameText.text = data.SNameText.text;

        STRText.text = data.pSTR.ToString("F0"); // "F0" removes decimals
        STRBar.fillAmount = data.pSTR / maxValue;
        INTText.text = data.pINT.ToString("F0");
        INTBar.fillAmount = data.pINT / maxValue;
        DEXText.text = data.pDEX.ToString("F0");
        DEXBar.fillAmount = data.pDEX / maxValue;
        WISText.text = data.pWIS.ToString("F0");
        WISBar.fillAmount = data.pWIS / maxValue;
        VITText.text = data.pVIT.ToString("F0");
        VITBar.fillAmount = data.pVIT / maxValue;
        ENDText.text = data.pEND.ToString("F0");
        ENDBar.fillAmount = data.pEND / maxValue;
        SPIText.text = data.pSPI.ToString("F0");
        SPIBar.fillAmount = data.pSPI / maxValue;
        AGIText.text = data.pAGI.ToString("F0");
        AGIBar.fillAmount = data.pAGI / maxValue;
        GROText.text = data.pGRO.ToString("F0");
        GROBar.fillAmount = data.pGRO / maxValue;

        ageText.text = "Age: " + data.Age.ToString("F0");

        squadNum.text = data.SquadNum.ToString("F0");
    }

    public void UpSquad()
    {
        currentSelector.IncSquad();
        squadNum.text = currentSelector.SquadNum.ToString("F0");
        currentSelector.SaveToData();
        SaveManager.Instance.UpdateUnitInSave(currentSelector);
    }
    public void DownSquad()
    {
        currentSelector.DecSquad();
        squadNum.text = currentSelector.SquadNum.ToString("F0");
        currentSelector.SaveToData();
        SaveManager.Instance.UpdateUnitInSave(currentSelector);
    }
}
