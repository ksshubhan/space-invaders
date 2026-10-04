using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private static bool GameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    private bool hasLoggedIn = false;

    private void Start()
    {
        SetInitialGameState();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Initial game state is set when a new scene is loaded
    private void SetInitialGameState()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    // When new scene is loaded game state is reset 
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetInitialGameState();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        HandlePauseInput();

        CheckLoginStatus();
    }

    // Allows player to toggle between pause states 
    private void HandlePauseInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Resume the game by deactivating the pause menu
    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    // Handles pause functionality
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    // Checking logging status based on reponse from Web
    private void CheckLoginStatus()
    {
        string loginResponse = Main.Instance.Web.GetLoginResponse();
        if (loginResponse != null && loginResponse.Contains("Login successful"))
        {
            hasLoggedIn = true;
        }
    }

    // Loads Options screen but only if player has logged in first
    public void LoadOptionsScreen()
    {
        if (hasLoggedIn)
        {
            Time.timeScale = 1f;
            StartOptionsScreen();
        }
        else
        {
            Debug.Log("Please log in first.");
        }
    }

    // Quit the game (for a build application)
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    // Loads Options screen
    private void StartOptionsScreen()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.OptionsScreen);
    }
}
