using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeScreen : MonoBehaviour
{
    public Text welcomeText;
    public float textSpeed = 0.1f;
    public string nextSceneName = "OptionsScreen";

    private SceneLoader sceneLoader;

    private void Start()
    {
        StartCoroutine(DisplayWelcomeText());
    }

    // Obtains information for welcome message 
    private IEnumerator DisplayWelcomeText()
    {
        // Fetching the most expensive title and user's surname
        int userID = Main.Instance.Web.GetLoggedInUserId();

        yield return StartCoroutine(Main.Instance.Web.GetMostExpensiveTitle(userID, HandleMostExpensiveTitleResponse));
        yield return StartCoroutine(Main.Instance.Web.GetUserSurname(userID, HandleUserSurnameResponse));

        string welcomeMessage = $"Welcome back {Main.Instance.Web.GetMostExpensiveTitle()} {Main.Instance.Web.GetUserSurname()}";

        yield return DisplayTextWithSpeed(welcomeMessage, textSpeed);

        yield return new WaitForSeconds(1f);

        TriggerOptionsScreen();
    }

    // Callback method to handle the most expensive title response
    private void HandleMostExpensiveTitleResponse(Web.MostExpensiveTitleResponse mostExpensiveTitle)
    {
        Main.Instance.Web.SetMostExpensiveTitle(mostExpensiveTitle.title_name);
    }

    // Callback method to handle the user surname response
    private void HandleUserSurnameResponse(string surname)
    {
        Main.Instance.Web.SetUserSurname(surname);
    }

    // Displays text with progressive disclosure 
    private IEnumerator DisplayTextWithSpeed(string fullText, float speed)
    {
        string currentText = "";

        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            welcomeText.text = currentText;

            yield return new WaitForSeconds(speed);
        }
    }

    // Transitions to Options screen
    private void TriggerOptionsScreen()
    {
        GetSceneLoaderInstance();

        if (sceneLoader != null)
        {
            sceneLoader.TransitionToOptionsScreen();
        }
        else
        {
            Debug.LogError("SceneLoader reference is null. Ensure SceneLoader is present in the scene.");
        }
    }

    private void GetSceneLoaderInstance()
    {
        if (sceneLoader == null)
        {
            sceneLoader = SceneLoader.Instance;
        }
    }
}
