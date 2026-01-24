using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestEncounter : MonoBehaviour
{
    public TextMeshProUGUI encounterText;

    // Start is called before the first frame update
    void Start()
    {
        encounterText.text = QuestManager.Instance.encounter;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
