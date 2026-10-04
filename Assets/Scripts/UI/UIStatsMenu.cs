using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatsMenu : MonoBehaviour
{
    [SerializeField] Button GlobalStatsButton;
    [SerializeField] Button PlayerStatsButton;
    [SerializeField] Button BackButton;

    private void Start()
    {
        AttachButtonClickHandlers();
    }

    // Check buttons are present in scene 
    private void AttachButtonClickHandlers()
    {
        if (GlobalStatsButton != null)
        {
            GlobalStatsButton.onClick.AddListener(LoadGlobalStatsScreen);
        }
        else
        {
            HandleButtonNotAssigned("GlobalStatsButton");
        }

        if (PlayerStatsButton != null)
        {
            PlayerStatsButton.onClick.AddListener(LoadPlayerStatsScreen);
        }
        else
        {
            HandleButtonNotAssigned("PlayerStatsButton");
        }

        if (BackButton != null)
        {
            BackButton.onClick.AddListener(LoadStatsandScoresScreen);
        }
        else
        {
            HandleButtonNotAssigned("BackButton");
        }
    }

    // Loading Global Stats screen 
    private void LoadGlobalStatsScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.GlobalStatsScreen);
    }

    // Loading Player Stats Screen scene
    private void LoadPlayerStatsScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.PlayerStatsScreen);
    }

    // Loading Stats and Scores screen 
    private void LoadStatsandScoresScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StatsandScoresScreen);
    }

    // Handling case where button is not assigned
    private void HandleButtonNotAssigned(string buttonName)
    {
        Debug.LogWarning(buttonName + " is not assigned in the inspector.");
    }
}
