using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public static GameObject enemyPanel;
    public string encounter;

    public void loadQuest()
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
