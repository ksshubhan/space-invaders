using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Text feedbackText;

    private void Start()
    {
        if (loginButton != null)
        {
            loginButton.onClick.AddListener(AttemptLogin);
        }
        else
        {
            Debug.LogWarning("LoginButton is not assigned in the inspector.");
        }
    }

    // Handles the user's login attempt
    private void AttemptLogin()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        // Checking if username or password is empty
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            SetFeedbackText("Username and password cannot be empty.");
            return;
        }

        StartCoroutine(LoginUserCoroutine(username, password));
    }

    // Handles login process 
    private IEnumerator LoginUserCoroutine(string username, string password)
    {
        yield return StartCoroutine(Main.Instance.Web.LoginUser(username, password));

        // Fetching login response from the Web class
        string loginResponse = Main.Instance.Web.GetLoginResponse();

        // Checking result and providing feedback to the user
        if (loginResponse.Equals("Login successful"))
        {
            SetFeedbackText("Login successful!");

            Debug.Log("Login successful!");

            Debug.Log("User ID : " + Main.Instance.Web.GetLoggedInUserId());

            yield return StartCoroutine(Main.Instance.Web.FetchTitles(username, password));

            int userID = Main.Instance.Web.GetLoggedInUserId();

            yield return StartCoroutine(Main.Instance.Web.GetMostExpensiveTitle(userID, HandleMostExpensiveTitleResponse));

            yield return StartCoroutine(Main.Instance.Web.GetUserFirstName(userID, HandleUserFirstNameResponse));

            yield return StartCoroutine(Main.Instance.Web.GetUserSurname(userID, HandleUserSurnameResponse));

            // Loading Options screen after successful login
            SceneManager.LoadScene("WelcomeScreen");
        }
        else
        {
            SetFeedbackText(loginResponse);

            Debug.Log(loginResponse);
        }
    }

    // Callback method to handle the most expensive title response
    private void HandleMostExpensiveTitleResponse(Web.MostExpensiveTitleResponse mostExpensiveTitle)
    {
        Debug.Log("Most Expensive Title: " + mostExpensiveTitle.title_name);
    }

    // Callback method to handle the user's first name response
    private void HandleUserFirstNameResponse(string firstName)
    {
        Debug.Log("User First Name: " + firstName);
    }

    // Callback method to handle the user's surname response
    private void HandleUserSurnameResponse(string surname)
    {

        Debug.Log("User Surname: " + surname);
    }

    // Setting feedback text onto the UI
    private void SetFeedbackText(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
        }
        else
        {
            Debug.LogWarning("Feedback text element not assigned in the inspector.");
        }
    }
}
