using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIInstructionsMenu : MonoBehaviour
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
            backButton.onClick.AddListener(LoadStartScreen);
        }
        else
        {
            Debug.LogWarning("BackButton is not assigned in the inspector.");
        }
    }

    // Loading options screen
    private void LoadStartScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StartScreen);
    }
}
