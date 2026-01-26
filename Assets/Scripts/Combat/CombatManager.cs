using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;

public class CombatManager : MonoBehaviour
{
    public int turnNum;
    public List<Unit> adventurers = new List<Unit>();
    public List<Unit> enemies = new List<Unit>();
    public List<Unit> allUnitsInBattle = new List<Unit>();

    void Start() // Edit > Project Settings > Script Execution Order : SET TO LAST
    {
        adventurers.AddRange(GameObject.Find("AdventurerTeam").GetComponentsInChildren<Unit>());
        enemies.AddRange(GameObject.Find("EnemyTeam").GetComponentsInChildren<Unit>());

        StartCoroutine(BattleRoutine());
    }

    void CreateInitiativeOrder()
    {
        allUnitsInBattle.Clear();
        allUnitsInBattle.AddRange(adventurers);
        allUnitsInBattle.AddRange(enemies);

        // Optional: Sort by a "Speed" variable if you have one
        allUnitsInBattle = allUnitsInBattle.OrderByDescending(u => u.Speed).ToList();
    }

    IEnumerator BattleRoutine()
    {
        while (adventurers.Count > 0 && enemies.Count > 0)
        {
            turnNum++;
            Debug.Log($"<color=yellow>--- Starting Round {turnNum} ---</color>");

            // Determine who goes in what order this round
            CreateInitiativeOrder();

            foreach (Unit unit in allUnitsInBattle)
            {
                if (adventurers.Count == 0 || enemies.Count == 0) break; // Re-check: Did the battle end mid-round?
                if (unit == null || unit.IsDead) continue;               // Skip dead units

                if (unit.UnitTeam == Team.Adventurer)
                {
                    yield return StartCoroutine(HandleUnitTurn(unit, enemies, adventurers));
                }
                else
                {
                    yield return StartCoroutine(HandleUnitTurn(unit, adventurers, enemies));
                }
            }

            yield return new WaitForSeconds(0.5f); // Brief pause between rounds
        }
    }

    IEnumerator HandleUnitTurn(Unit attacker, List<Unit> targets, List<Unit> allies) // allies for skills that target allies later on
    {
        Unit target = GetWeightedRandomTarget(targets);

        if (target != null)
        {
            attacker.Attack(target);
            Debug.Log($"<color=green>--- {attacker.UnitName} ({attacker.UnitTeam}) attacks {target.UnitName} ({target.UnitTeam}) for {attacker.OutgoingDamage} damage ---</color>");

            if (target.IsDead)
            {
                Debug.Log($"<color=red>--- {target.UnitName} ({target.UnitTeam}) has died ---</color>");
                targets.Remove(target);
                // We don't necessarily Destroy immediately if you want a death animation
                Destroy(target.gameObject, 0.5f);
            }
        }

        UpdateAllUnitStats();

        yield return new WaitForSeconds(2f); // Time for the player to see the action
    }

    public Unit GetWeightedRandomTarget(List<Unit> potentialTargets)
    {
        if (potentialTargets == null || potentialTargets.Count == 0) return null;

        // 1. Calculate the total sum of all aggro weights
        float totalAggro = potentialTargets.Sum(u => u.AggroWeight);

        // 2. Pick a random number between 0 and the total sum
        float randomRoll = Random.Range(0f, totalAggro);

        // 3. Iterate through targets and subtract their weight from the roll
        // The first one to drop the roll below zero is the winner
        float currentWeightSum = 0;
        foreach (Unit unit in potentialTargets)
        {
            currentWeightSum += unit.AggroWeight;
            if (randomRoll <= currentWeightSum)
            {
                return unit;
            }
        }

        return potentialTargets[0]; // Fallback
    }

    public void UpdateAllUnitStats()
    {
        foreach (Unit unit in allUnitsInBattle)
        {
            if (!unit.IsDead)
            {
                if (unit.UnitTeam == Team.Adventurer) unit.gameObject.GetComponent<AdventurerCombatUI>().DisplayStats();
                else unit.gameObject.GetComponent<EnemyCombatUI>().DisplayStats();
            }
        }
    }
}
