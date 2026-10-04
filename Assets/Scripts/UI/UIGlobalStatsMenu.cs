using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGlobalStatsMenu : MonoBehaviour
{
    [SerializeField] private Button backButton;

    private void Start()
    {
        InitialiseUI();
    }

    // Checking if back button is present in scene
    private void InitialiseUI()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(LoadStatsAndScoresScreen);
        }
        else
        {
            Debug.LogWarning("BackButton is not assigned in the inspector.");
        }
    }

    // Loading StatsandScores screen 
    private void LoadStatsAndScoresScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StatsandScoresScreen);
    }
}
