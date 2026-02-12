using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestInformation : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI questText;
    public enum QuestType { 
        Slime, Goblin,    // F
        Undead, Roper,    // E
        Bandit, Orc,      // D
        Dungeon, Pirate,  // C
        Skeleton, Snow,   // B
        Desert, Mythical, // A
        Demon, Angel,     // S
        Unknown
        }

    private int lowestReward = 25;
    private int highestReward = 76;
    [SerializeField] private int reward;

    [Header("Quest settings")]
    public QuestType questType;
    public Rank questRank;

    private void Start()
    {
        questType = GetWeightedQuest();
        UpdateQuestText();
    }

    public void SetEncounter()
    {
        QuestManager.Instance.encounter = questType.ToString();
        QuestManager.Instance.questReward = reward;
    }

    public QuestType GetWeightedQuest()
    {
        int roll = Random.Range(0, 100);

        switch (ProgressManager.Instance.guildRank)
        {
            case Rank.S:
                reward = Random.Range(lowestReward + 150, highestReward + 150);
                return QuestType.Undead;
            case Rank.A:
                reward = Random.Range(lowestReward + 125, highestReward + 125);
                return QuestType.Undead;
            case Rank.B:
                reward = Random.Range(lowestReward + 100, highestReward + 100);
                return QuestType.Undead;
            case Rank.C:
                reward = Random.Range(lowestReward + 75, highestReward + 75);
                return QuestType.Undead;
            case Rank.D:
                reward = Random.Range(lowestReward + 50, highestReward + 50);
                return QuestType.Undead;
            case Rank.E:
                reward = Random.Range(lowestReward + 25, highestReward + 25);

                return roll switch
                {
                    < 10 => GetRandomQuest(QuestType.Slime, QuestType.Roper),
                    _ => GetRandomQuest(QuestType.Slime, QuestType.Goblin)
                };
            case Rank.F:
                reward = Random.Range(lowestReward, highestReward);

                return roll switch
                {
                    < 50 => QuestType.Slime, // 50% chance
                    _ => QuestType.Goblin // 50% chance
                };

            default:
                Debug.LogWarning("Rank not found, defaulting to Slime!");
                return QuestType.Slime;
        }
    }

    public void UpdateQuestText()
    {
        switch (questType)
        {
            case QuestType.Slime:
                questText.text = $"Kill slimes in the forest\n\nReward: {reward}";
                questRank = Rank.F;
                break;
            case QuestType.Goblin:
                questText.text = $"Clear out a goblin infestation\n\nReward: {reward}";
                questRank = Rank.F;
                break;
            case QuestType.Undead:
                questText.text = $"Cleanse the undeads' souls\n\nReward: {reward}";
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        QuestConfirmation.Instance.confirmation.SetActive(true);
        FindFirstObjectByType<ConfirmationMemory>().SetObjectToRemember(gameObject);
    }

    public QuestType GetRandomQuest(QuestType start, QuestType end)
    {
        int min = (int)start;
        int max = (int)end;

        int randomValue = UnityEngine.Random.Range(min, max + 1);

        return (QuestType)randomValue;
    }
}
