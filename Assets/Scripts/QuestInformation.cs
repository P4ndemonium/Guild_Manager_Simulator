using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInformation : MonoBehaviour
{
    public enum QuestType { Slime, Goblin, Undead }

    [Header("Quest settings")]
    public QuestType questType;

    public void SetEncounter()
    {
        // To use it as a string:
        QuestManager.Instance.encounter = questType.ToString();
    }
}
