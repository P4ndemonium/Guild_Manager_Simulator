using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommandBridge : MonoBehaviour
{
    public void ClickUnloadCurrentScene() { GameSceneManager.Instance.UnloadCurrentScene(); }
    public void ClickEnterListings() { GameSceneManager.Instance.EnterListings(); }
    public void ClickEnterCombat()
    {
        if (!SaveManager.Instance.IsAdventurerConditionZero())
        { 
            ConfirmationMemory.Instance.questConfirmationPanel.SetActive(false);
            ConfirmationMemory.Instance.DestroyRememberedObject();
            GameSceneManager.Instance.EnterCombat();
        }
        else
        {
            Debug.Log("Party has an adventurer at 0% condition");
        }
    }
    public void ClickOnSaveButtonPressed() { SaveManager.Instance.OnSaveButtonPressed(); }
    public void ClickResetSaveData() { SaveManager.Instance.ResetSaveData(); }
    public void ClickIncSelectedParty() { QuestManager.Instance.IncSelectedParty(); }
    public void ClickDecSelectedParty() { QuestManager.Instance.DecSelectedParty(); }
    public void ClickResetSelectedPartyNum() { QuestManager.Instance.ResetSelectedPartyNum(); }
    public void ClickNextWeek() { ProgressManager.Instance.NextWeek(); }
}
