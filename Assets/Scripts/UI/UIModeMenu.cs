using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModeMenu : MonoBehaviour
{
    [SerializeField] Button _startNormal;
    [SerializeField] Button _startMaze;
    [SerializeField] Button _backButton;

    private void Start()
    {
        InitialiseUI();
    }

    // Checking buttons are present in scene
    private void InitialiseUI()
    {
        if (_startNormal != null)
        {
            _startNormal.onClick.AddListener(LoadSpaceInvaders);
        }
        else
        {
            Debug.LogWarning(_startNormal + " is not assigned in the inspector.");
        }

        if (_startMaze != null)
        {
            _startMaze.onClick.AddListener(LoadMazeMode);
        }
        else
        {
            Debug.LogWarning(_startMaze + " is not assigned in the inspector.");
        }

        if (_backButton != null)
        {
            _backButton.onClick.AddListener(LoadOptionsScreen);
        }
        else
        {
            Debug.LogWarning(_backButton + " is not assigned in the inspector.");
        }
    }

    // Loading Space Invaders screen
    private void LoadSpaceInvaders()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.SpaceInvaders);
    }

    // Loading Maze Mode screen
    private void LoadMazeMode()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.MazeScreen);
    }

    // Loading Options screen
    private void LoadOptionsScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.OptionsScreen);
    }
}
