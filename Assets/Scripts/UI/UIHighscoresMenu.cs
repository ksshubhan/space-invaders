using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHighscoresMenu : MonoBehaviour
{
    [SerializeField] Button backButton;

    private void Start()
    {
        InitialiseUI();
    }

    // Checking if back button is present in scene
    private void InitialiseUI()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(LoadStatsandScoresScreen);
        }
        else
        {
            Debug.LogWarning("BackButton is not assigned in the inspector.");
        }
    }

    // Loading StatsandScores screen
    private void LoadStatsandScoresScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StatsandScoresScreen);
    }
}
