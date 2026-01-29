using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommandBridge : MonoBehaviour
{
    public void ClickUnloadCurrentScene() { GameSceneManager.Instance.UnloadCurrentScene(); }
    public void ClickEnterListings() { GameSceneManager.Instance.EnterListings(); }
    public void ClickEnterCombat() { GameSceneManager.Instance.EnterCombat(); }
    public void ClickOnSaveButtonPressed() { SaveManager.Instance.OnSaveButtonPressed(); }
    public void ClickResetSaveData() { SaveManager.Instance.ResetSaveData(); }
    public void ClickIncSelectedParty() { QuestManager.Instance.IncSelectedParty(); }
    public void ClickDecSelectedParty() { QuestManager.Instance.DecSelectedParty(); }
}
