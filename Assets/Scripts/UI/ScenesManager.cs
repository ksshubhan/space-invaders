using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Enumeration of different scenes in the game
    public enum Scene
    {
        StartScreen,
        RegisterScreen,
        LoginScreen,
        SpaceInvaders,
        GameModeScreen,
        HighscoresScreen,
        StatsandScoresScreen,
        MazeScreen,
        MultiplayerScreen,
        GlobalStatsScreen,
        SettingsScreen,
        StatsScreen,
        PlayerStatsScreen,
        OptionsScreen,
        StoreScreen,
        CongratsScreen
    }

    // Loads the specified sreen
    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    // Loads the Start screen 
    public void LoadStartScreen()
    {
        SceneManager.LoadScene(Scene.StartScreen.ToString());
    }

    // Load the next screen in the build order
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
