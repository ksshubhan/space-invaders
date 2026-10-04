using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISettingsMenu : MonoBehaviour
{
    [SerializeField] private Button backButton;

    private void Start()
    {
        InitialiseUI();
    }

    // Checking button is present in scene
    private void InitialiseUI()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(LoadOptionsScreen);
        }
        else
        {
            Debug.LogWarning("BackButton is not assigned in the inspector.");
        }
    }

    // Loading start screen
    private void LoadOptionsScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.OptionsScreen);
    }
}
