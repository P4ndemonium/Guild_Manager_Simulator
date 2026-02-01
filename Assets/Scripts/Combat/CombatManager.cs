using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public TurnOrderUI turnOrderUI;
    public GameObject endPanel;
    public TextMeshProUGUI endText;

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

    // ================================================================
    // TURN LOGIC
    // ================================================================

    IEnumerator BattleRoutine()
    {
        yield return new WaitForEndOfFrame();

        // 1. Initial Setup: Set everyone's first turn time based on their cost
        allUnitsInBattle.Clear();
        allUnitsInBattle.AddRange(adventurers);
        allUnitsInBattle.AddRange(enemies);

        foreach (Unit u in allUnitsInBattle)
        {
            u.nextActionTime = u.ActionCost;
        }

        while (adventurers.Count > 0 && enemies.Count > 0)
        {
            // 2. Find the unit whose nextActionTime is the SMALLEST
            // We filter out nulls or dead units just in case
            Unit nextUnit = allUnitsInBattle
                .Where(u => u != null && !u.IsDead)
                .OrderBy(u => u.nextActionTime)
                .FirstOrDefault();

            if (nextUnit == null) break;

            // 3. Let that unit take their turn
            if (nextUnit.UnitTeam == Team.Adventurer)
                yield return StartCoroutine(HandleUnitTurn(nextUnit, enemies, adventurers));
            else
                yield return StartCoroutine(HandleUnitTurn(nextUnit, adventurers, enemies));

            // 4. Update the unit's next action time based on their cost
            // This pushes them further down the timeline
            nextUnit.nextActionTime += nextUnit.ActionCost;

            // Re-update UI after the action in case someone died
            // turnOrderUI.UpdateDisplay(allUnitsInBattle);

            // Optional: If the numbers get too high (e.g. 1,000,000), 
            // you can subtract the lowest timestamp from everyone to keep them small.
        }
    }

    IEnumerator HandleUnitTurn(Unit attacker, List<Unit> targets, List<Unit> allies) // allies for skills that target allies later on
    {
        Unit target = GetWeightedRandomTarget(targets);

        if (target != null)
        {
            yield return StartCoroutine(attacker.Attack(target));
            Debug.Log($"<color=green>--- {attacker.UnitName} ({attacker.UnitTeam}) attacks {target.UnitName} ({target.UnitTeam}) for {attacker.OutgoingDamage} damage ---</color>");
            //UpdateAllUnitStats();

            if (target.IsDead)
            {
                Debug.Log($"<color=red>--- {target.UnitName} ({target.UnitTeam}) has died ---</color>");
                targets.Remove(target);
                // We don't necessarily Destroy immediately if you want a death animation
                Destroy(target.gameObject, 0.5f);

                if (target.UnitTeam == Team.Adventurer)
                {
                    SaveManager.Instance.DeleteUnitFromSave(target.UnitID);
                }
            }

            if (CheckForBattleEnd())
            {
                StopAllCoroutines(); // Stop the battle loop immediately
                yield break;
            }
        }

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

    // ================================================================
    // END OF COMBAT
    // ================================================================

    private bool CheckForBattleEnd()
    {
        // Check if all adventurers are dead or the list is empty
        bool adventurersWiped = adventurers.Count == 0 || adventurers.All(u => u.IsDead);

        // Check if all enemies are dead or the list is empty
        bool enemiesWiped = enemies.Count == 0 || enemies.All(u => u.IsDead);

        if (adventurersWiped)
        {
            Debug.Log("<color=red>DEFEAT! All adventurers have fallen.</color>");
            EndBattle(Team.Enemy);

            return true;
        }

        if (enemiesWiped)
        {
            Debug.Log("<color=green>VICTORY! All enemies defeated.</color>");
            EndBattle(Team.Adventurer);

            return true;
        }

        return false;
    }

    void EndBattle(Team winner)
    {
        endPanel.SetActive(true);
        if (winner == Team.Adventurer)
        {
            endText.text = $"VICTORY!!! Your party WON!!!\n\n" +
                $"Gold received: {QuestManager.Instance.questReward}\n\n" +
                $"Survivng members: {GetSurvivorList()}";
            ProgressManager.Instance.gold += QuestManager.Instance.questReward;
            ProgressManager.Instance.rating -= 1f;
            ProgressManager.Instance.week += 1;
        }
        else
        {
            endText.text = $"Your party was wiped out...";
            ProgressManager.Instance.rating += 0.2f;
            ProgressManager.Instance.week += 1;
        }
    }

    public string GetSurvivorList()
    {
        var survivorStrings = adventurers
            .Where(u => u != null && !u.IsDead)
            .Select(u => $"{u.UnitName}");

        return string.Join(", ", survivorStrings);
    }
}
