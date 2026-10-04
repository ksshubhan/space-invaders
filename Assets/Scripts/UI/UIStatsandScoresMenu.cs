using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatsandScoresMenu : MonoBehaviour
{
    [SerializeField] Button StatsButton;
    [SerializeField] Button ScoresButton;
    [SerializeField] Button BackButton;

    private void Start()
    {
        InitialiseUI();
    }

    // Check buttons are present in scene 
    private void InitialiseUI()
    {
        if (StatsButton != null)
        {
            StatsButton.onClick.AddListener(LoadStatsScreen);
        }
        else
        {
            ButtonNotAssigned("StatsButton");
        }

        if (ScoresButton != null)
        {
            ScoresButton.onClick.AddListener(LoadHighscoresScreen);
        }
        else
        {
            ButtonNotAssigned("ScoresButton");
        }

        if (BackButton != null)
        {
            BackButton.onClick.AddListener(LoadStartScreen);
        }
        else
        {
            ButtonNotAssigned("BackButton");
        }
    }

    // Loading Stats screen
    private void LoadStatsScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StatsScreen);
    }

    // Loading Highscores screen 
    private void LoadHighscoresScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.HighscoresScreen);
    }

    // Loading Options screen
    private void LoadStartScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.OptionsScreen);
    }

    // Handling case where button is not assigned
    private void ButtonNotAssigned(string buttonName)
    {
        Debug.LogWarning(buttonName + " is not assigned in the inspector.");
    }
}
