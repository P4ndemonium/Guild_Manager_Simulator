using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestInformation : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI questText;
    public enum QuestType { Slime, Goblin, Undead }

    private int lowestReward = 25;
    private int highestReward = 76;
    [SerializeField] private int reward;

    [Header("Quest settings")]
    public QuestType questType;

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
        switch (ProgressManager.Instance.guildRank)
        {
            case ProgressManager.GuildRank.S:
                reward = Random.Range(lowestReward + 150, highestReward + 150);
                return QuestType.Undead;
            case ProgressManager.GuildRank.A:
                reward = Random.Range(lowestReward + 125, highestReward + 125);
                return QuestType.Undead;
            case ProgressManager.GuildRank.B:
                reward = Random.Range(lowestReward + 100, highestReward + 100);
                return QuestType.Undead;
            case ProgressManager.GuildRank.C:
                reward = Random.Range(lowestReward + 75, highestReward + 75);
                return QuestType.Undead;
            case ProgressManager.GuildRank.D:
                reward = Random.Range(lowestReward + 50, highestReward + 50);
                return QuestType.Undead;
            case ProgressManager.GuildRank.E:
                reward = Random.Range(lowestReward + 25, highestReward + 25);
                return QuestType.Undead;
            case ProgressManager.GuildRank.F:
                reward = Random.Range(lowestReward, highestReward);
                int roll = Random.Range(0, 100);

                return roll switch
                {
                    < 60 => QuestType.Slime, // 50% chance
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
                break;
            case QuestType.Goblin:
                questText.text = $"Clear out a goblin infestation\n\nReward: {reward}";
                break;
            case QuestType.Undead:
                questText.text = $"Cleanse the undeads' souls\n\nReward: {reward}";
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        QuestConfirmation.Instance.confirmation.SetActive(true);
        FindFirstObjectByType<ConfirmationMemory>().SetObjectToRemember(this.gameObject);
    }
}
