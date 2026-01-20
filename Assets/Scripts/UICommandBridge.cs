using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommandBridge : MonoBehaviour
{
    public void ClickReturnToGuild()
    {
        // Because of 'static Instance', any scene can see this!
        GameSceneManager.Instance.ReturnToGuild();
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
