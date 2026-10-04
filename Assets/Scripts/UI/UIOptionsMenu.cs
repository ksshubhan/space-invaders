using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOptionsMenu : MonoBehaviour
{
    [SerializeField] Button _startStatsandScores;
    [SerializeField] Button _startSettings;
    [SerializeField] Button _startGames;
    [SerializeField] Button _startStore;
    [SerializeField] Button _backButton;

    void Start()
    {
        InitialiseUI();
    }

    // Checking buttons are present in scene
    private void InitialiseUI()
    {
        if (_startStatsandScores != null)
        {
            _startStatsandScores.onClick.AddListener(StartStatsandScores);
        }
        else
        {
            ButtonNotAssigned("_startStatsandScores");
        }

        if (_startSettings != null)
        {
            _startSettings.onClick.AddListener(StartSettings);
        }
        else
        {
            ButtonNotAssigned("_startSettings");
        }

        if (_startGames != null)
        {
            _startGames.onClick.AddListener(StartGames);
        }
        else
        {
            ButtonNotAssigned("_startGames");
        }

        if (_startStore != null)
        {
            _startStore.onClick.AddListener(StartStore);
        }
        else
        {
            ButtonNotAssigned("_startStore");
        }
    }

    // Loading the Stats and Scores screen 
    private void StartStatsandScores()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StatsandScoresScreen);
    }

    // Loading the Settings screen 
    private void StartSettings()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.SettingsScreen);
    }

    // Loading the Game Mode screen 
    private void StartGames()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.GameModeScreen);
    }

    // Loading the Start screen 
    private void StartScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StartScreen);
    }

    // Loading the Store screen 
    private void StartStore()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StoreScreen);
    }

    // Handling case where button is not assigned
    private void ButtonNotAssigned(string buttonName)
    {
        Debug.LogWarning(buttonName + " is not assigned in the inspector.");
    }
}
