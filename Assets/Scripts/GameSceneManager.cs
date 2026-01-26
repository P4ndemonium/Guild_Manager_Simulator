using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    [SerializeField] private GameObject guildCanvas; // Reference to your Guild UI
    private string currentLoadedScene;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string combatScene = "Combat";
    public string listingScene = "Listings";

    private void Awake()
    {
        Instance = this;
        // Start by showing the Main Menu over the Guild
        //LoadSceneAdditive(mainMenuScene);                  <-- ADD THIS LATER
    }

    public void EnterCombat()
    {
        // 1. Hide the Guild UI so we don't see it behind the combat
        guildCanvas.SetActive(false);

        // 2. Load Combat
        LoadSceneAdditive(combatScene);
    }

    public void EnterListings()
    {
        // 1. Hide the Guild UI so we don't see it behind the combat
        guildCanvas.SetActive(false);

        // 2. Load Combat
        LoadSceneAdditive(listingScene);
    }

    public void ReturnToGuild()
    {
        // 1. Unload Combat
        UnloadCurrentScene();

        // 2. Show the Guild UI again
        guildCanvas.SetActive(true);
    }

    private void LoadSceneAdditive(string sceneName)
    {
        currentLoadedScene = sceneName;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    private void UnloadCurrentScene()
    {
        if (!string.IsNullOrEmpty(currentLoadedScene))
        {
            SceneManager.UnloadSceneAsync(currentLoadedScene);
        }
    }
}
