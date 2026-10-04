using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginMenu : MonoBehaviour
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
            backButton.onClick.AddListener(LoadStartScreen);
        }
        else
        {
            Debug.LogWarning("BackButton is not assigned in the inspector.");
        }
    }

    // Loading Start screen
    private void LoadStartScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StartScreen);
    }
}
