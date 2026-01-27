using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerListingUI : Adventurer
{
    [Header("UI References")]
    [SerializeField] protected Image image;
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected TextMeshProUGUI statsTextLeft;
    [SerializeField] protected TextMeshProUGUI statsTextRight;

    // Start is called before the first frame update
    void Start()
    {
        RandomName();
        RandomizeStats();
        DisplayStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayStats()
    {
        image.sprite = library.allPossibleSprites[spriteID];

        if (nameText != null && statsTextLeft != null && statsTextRight != null)
        {
            nameText.text = unitName;

            statsTextLeft.text = $"STR: {STR}\n" +
                                 $"DEX: {DEX}\n" +
                                 $"VIT: {VIT}\n" +
                                 $"SPI: {SPI}\n" +
                                 $"GRO: {GRO}";

            statsTextRight.text = $"INT: {INT}\n" +
                                  $"WIS: {WIS}\n" +
                                  $"END: {END}\n" +
                                  $"AGI: {AGI}\n" +
                                  $"age: {age}";
        }
    }
}
