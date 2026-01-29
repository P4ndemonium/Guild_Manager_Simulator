using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public int selectedPartyNum = 1;
    public TextMeshProUGUI selectedPartyNumText;
    public string encounter;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public void IncSelectedParty()
    {
        selectedPartyNum++;
        selectedPartyNumText.text = selectedPartyNum.ToString();
    }

    public void DecSelectedParty()
    {
        if (selectedPartyNum > 1)
        {
            selectedPartyNum--;
            selectedPartyNumText.text = selectedPartyNum.ToString();
        }
    }
}
