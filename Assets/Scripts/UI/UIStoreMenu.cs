using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIStoreMenu : MonoBehaviour
{
    [SerializeField] Button _backButton;

    private void Start()
    {
        InitialiseUI();
    }
    // Checking button is present in scene 
    private void InitialiseUI()
    {
        if (_backButton != null)
        {
            _backButton.onClick.AddListener(LoadOptionsScreen);
        }
        else
        {
            Debug.LogWarning("BackButton is not assigned in the inspector.");
        }
    }
    
    // Loading Options screen 
    private void LoadOptionsScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.OptionsScreen);
    }
}
