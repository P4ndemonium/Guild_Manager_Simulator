using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInformation : MonoBehaviour
{
    public enum QuestType { Slime, Goblin, Undead }

    [Header("Select from Dropdown")]
    public QuestType questType;

    void Start()
    {
        // To use it as a string:
        QuestManager.Instance.encounter = questType.ToString();
    }
}
