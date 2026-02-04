using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingResult
{
    public TrainingOption originalOption;
    public List<StatChange> visibleResults = new List<StatChange>();
    public List<StatChange> hiddenResults = new List<StatChange>();

    public TrainingResult(TrainingOption option)
    {
        originalOption = option;
        foreach (var change in option.changes)
        {
            int rolledValue = Random.Range(change.minAmount, change.maxAmount + 1);
            StatChange rolledChange = new StatChange {
                stat = change.stat,
                minAmount = rolledValue,
                maxAmount = rolledValue 
            };

            // RandomStat is hidden, but AllOtherStats is usually visible
            if (change.stat == StatType.RandomStat) {
                hiddenResults.Add(rolledChange);
            } else {
                visibleResults.Add(rolledChange);
            }
        }
    }
}
