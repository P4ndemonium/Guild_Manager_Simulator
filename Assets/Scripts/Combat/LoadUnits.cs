using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using System.Linq;

public class LoadUnits : MonoBehaviour
{
    public GameObject unitPanel;
    public RectTransform parent;


    void Start()
    {
        if (parent.name == "AdventurerTeam") LoadAdventurers();
        else if (parent.name == "EnemyTeam") LoadEnemies();
    }

    public void LoadAdventurers()
    {
        SaveManager.Instance.OnLoadButtonPressed();

        foreach (UnitSaveData unitData in SaveManager.Instance.saveFile.hiredAdventurers)
        {
            GameObject newPanel = Instantiate(unitPanel, parent);

            if (newPanel.GetComponent<AdventurerCombatUI>() != null)
            {
                newPanel.GetComponent<AdventurerCombatUI>().LoadFromData(unitData);
            }
        }
    }

    public void LoadEnemies()
    {
        int roll = Random.Range(3, 6); // 3 - 5 inclusive
        for (int i = 0; i < roll; i++) SpawnRandomEnemy(QuestManager.Instance.encounter);
    }

    public void SpawnRandomEnemy(string encounterType)
    {
        float roll = Random.Range(0f, 100f);
        if (encounterType == "Slime")
        {
            if (roll <= 60f) FindAndInstantiateEnemy("enemy_blue_slime");
            else if (roll <= 90f) FindAndInstantiateEnemy("enemy_green_slime");
            else FindAndInstantiateEnemy("enemy_red_slime");
        }
        else if (encounterType == "Goblin")
        {

        }
    }

    public void FindAndInstantiateEnemy(string targetName)
    {
        UnitSaveData foundEnemy = EnemyDatabase.Instance.enemyDatabase.FirstOrDefault(item => item.unitID == targetName);

        GameObject newPanel = Instantiate(unitPanel, parent);
        if (newPanel.GetComponent<EnemyCombatUI>() != null)
        {
            newPanel.GetComponent<EnemyCombatUI>().LoadFromData(foundEnemy);
            newPanel.GetComponent<EnemyCombatUI>().RandomizeStats();
        }
    }
}
