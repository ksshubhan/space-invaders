using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStatsMenu : MonoBehaviour
{
    [SerializeField] Button backButton;

    private void Start()
    {
        InitialiseUI(); 
    }

    // Checking button is present in scene 
    private void InitialiseUI()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(LoadStatsScreen);
        }
        else
        {
            Debug.LogWarning("BackButton is not assigned in the inspector.");
        }
    }

    // Loading Stats screen
    private void LoadStatsScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StatsScreen);
    }
}
