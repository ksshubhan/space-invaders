using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartMenu : MonoBehaviour
{
    [SerializeField] Button _startLogin;
    [SerializeField] Button _startRegister;

    void Start()
    {
        InitialiseUI();
    }

    // Checking buttons are present in scene
    private void InitialiseUI()
    {
        if (_startLogin != null)
        {
            _startLogin.onClick.AddListener(StartLogin);
        }
        else
        {
            ButtonNotAssigned("_startLogin");
        }

        if (_startRegister != null)
        {
            _startRegister.onClick.AddListener(StartRegister);
        }
        else
        {
            ButtonNotAssigned("_startRegister");
        }
    }

    // Loading Login screen 
    private void StartLogin()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.LoginScreen);
    }

    // Loading Register screen 
    private void StartRegister()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.RegisterScreen);
    }

    // Handling case where button is not assigned
    private void ButtonNotAssigned(string buttonName)
    {
        Debug.LogWarning(buttonName + " is not assigned in the inspector.");
    }
}
