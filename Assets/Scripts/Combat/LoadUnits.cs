using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using System.Linq;
using UnityEngine.UI;

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
            if (unitData.partyNum == QuestManager.Instance.selectedPartyNum)
            {
                GameObject newPanel = Instantiate(unitPanel, parent, false);

                if (newPanel.GetComponent<AdventurerCombatUI>() != null)
                {
                    newPanel.GetComponent<AdventurerCombatUI>().LoadFromData(unitData);
                }
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent); // for bug where unit spawns off screen/layout
    }

    public void LoadEnemies()
    {
        int roll = Random.Range(3, 6); // 3 - 5 inclusive
        for (int i = 0; i < roll; i++) SpawnRandomEnemy(QuestManager.Instance.encounter);
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent); // for bug where unit spawns off screen/layout
    }

    public void SpawnRandomEnemy(string encounterType)
    {
        float roll = Random.Range(0f, 100f);
        if (encounterType == "Slime")
        {
            if (roll <= 60f) FindAndInstantiateEnemy("enemy_blue_slime");       // 60%
            else if (roll <= 90f) FindAndInstantiateEnemy("enemy_green_slime"); // 30%
            else FindAndInstantiateEnemy("enemy_red_slime");                    // 10%
        }
        else if (encounterType == "Goblin")
        {
            if (roll <= 30f) FindAndInstantiateEnemy("enemy_goblin");              // 30%
            else if (roll <= 55f) FindAndInstantiateEnemy("enemy_goblin_warrior"); // 25%
            else if (roll <= 80f) FindAndInstantiateEnemy("enemy_goblin_shaman");  // 25%
            else if (roll <= 99f) FindAndInstantiateEnemy("enemy_hobgoblin");      // 19%
            else FindAndInstantiateEnemy("enemy_goblin_king");                     // 1%
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
