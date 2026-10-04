using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICongratsMenu : MonoBehaviour
{
    [SerializeField] private Button HomeButton;

    private void Start()
    {
        InitialiseUI();
    }

    // Checking button is present in scene
    private void InitialiseUI()
    {
        if (HomeButton != null)
        {
            HomeButton.onClick.AddListener(LoadStartScreen);
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
