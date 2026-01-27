using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommandBridge : MonoBehaviour
{
    public void ClickUnloadCurrentScene()
    {
        // Because of 'static Instance', any scene can see this!
        GameSceneManager.Instance.UnloadCurrentScene();
    }

    public void ClickEnterListings()
    {
        // Because of 'static Instance', any scene can see this!
        GameSceneManager.Instance.EnterListings();
    }

    public void ClickEnterCombat()
    {
        // Because of 'static Instance', any scene can see this!
        GameSceneManager.Instance.EnterCombat();
    }

    public void ClickOnSaveButtonPressed()
    {
        SaveManager.Instance.OnSaveButtonPressed();
    }

    public void ClickResetSaveData()
    {
        SaveManager.Instance.ResetSaveData();
    }
}
