using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    [Header("UI Canvases")]
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject guildCanvas;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string guildScene = "Guild";
    public string combatScene = "Combat";
    public string listingScene = "Listings";

    public string currentScene = "MainMenu";

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void EnterGuild()
    {
        mainMenuCanvas.SetActive(false);
        currentScene = guildScene;
        SceneManager.LoadScene(guildScene, LoadSceneMode.Additive);
    }

    public void EnterListings() // Also in UICommandBridge
    {
        if (guildCanvas != null) guildCanvas.SetActive(false);
        currentScene = listingScene;
        SceneManager.LoadScene(listingScene, LoadSceneMode.Additive);
    }

    public void EnterCombat() // Also in UICommandBridge
    {
        if (guildCanvas != null) guildCanvas.SetActive(false);
        currentScene = combatScene;
        SceneManager.LoadScene(combatScene, LoadSceneMode.Additive);
    }

    public void UnloadCurrentScene()
    {
        if (currentScene == guildScene)
        {
            guildCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
            currentScene = mainMenuScene;
            SceneManager.UnloadSceneAsync(guildScene);
        }
        else if (currentScene == listingScene)
        {
            guildCanvas.SetActive(true);
            currentScene = guildScene;
            SceneManager.UnloadSceneAsync(listingScene);
        }
        else if (currentScene == combatScene)
        {
            guildCanvas.SetActive(true);
            currentScene = guildScene;
            SceneManager.UnloadSceneAsync(combatScene);
        }
    }

    public void RegisterGuildCanvas(GameObject canvas) => guildCanvas = canvas;
}
