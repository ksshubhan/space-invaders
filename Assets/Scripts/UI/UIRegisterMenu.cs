using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRegisterMenu : MonoBehaviour
{
    [SerializeField] Button _startMenu;

    private void Start()
    {
        InitialiseUI();
    }

    // Checking button is present in scene
    private void InitialiseUI()
    {
        if (_startMenu != null)
        {
            _startMenu.onClick.AddListener(LoadStartScreen);
        }
        else
        {
            Debug.LogWarning(_startMenu + " is not assigned in the inspector.");
        }
    }

    // Loading the Start screen
    private void LoadStartScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StartScreen);
    }
}
