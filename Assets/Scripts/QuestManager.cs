using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public TextMeshProUGUI selectedSquadNum;
    public string encounter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadQuest()
    {
        if (encounter == "Slime")
        {
            float roll = Random.Range(0f, 100f);

            if (roll <= 60f)
            {
                // Spawn blue
            }
            else if (roll <= 90f) 
            {
                // Spawn green
            }
            else
            {
                // Spawn red
            }
        }
        else if (encounter == "Goblin")
        {

        }
    }
}
