using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    private static SceneLoader instance;

    private void Awake()
    {
        ManageSingletonInstance();
    }

    public static SceneLoader Instance
    {
        get { return instance; }
    }

    // Ensuring only one instance of SceneLoader exists
    private void ManageSingletonInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionToCongratsScreen()
    {
        StartCoroutine(TransitionToScene("CongratsScreen"));
    }

    // Handles transitions between scenes
    IEnumerator TransitionToScene(string sceneName)
    {
        // Trigger the transition animation
        transition.SetTrigger("Start");

        // Wait for the specified transition time
        yield return new WaitForSeconds(transitionTime);

        // Load the specified scene
        LoadScene(sceneName);
    }

    // Loads specified scene
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
