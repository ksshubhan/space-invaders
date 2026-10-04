using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField NameInput;
    public InputField SurnameInput;
    public InputField UsernameInput;
    public InputField PasswordInput;
    public Button RegisterButton;
    public Text RegistrationStatus;

    private void Start()
    {
        RegisterButton.onClick.AddListener(() =>
        {
            StartCoroutine(CheckUsernameAndRegister());
        });
    }

    // Handles registration process 
    IEnumerator CheckUsernameAndRegister()
    {
        // Validating inputs
        if (string.IsNullOrEmpty(NameInput.text) || string.IsNullOrEmpty(SurnameInput.text) ||
            string.IsNullOrEmpty(UsernameInput.text) || string.IsNullOrEmpty(PasswordInput.text))
        {
            // Display an error message if any field is empty
            SetRegistrationStatus("Please fill in all fields.");
            yield break;
        }

        // Checking if the username already exists
        yield return Main.Instance.Web.CheckUsernameExists(UsernameInput.text, (usernameExists) =>
        {
            if (usernameExists)
            {
                // Displaying error message if username already exists
                SetRegistrationStatus("Username already exists. Please choose a different one.");
            }
            else
            {
                // If username is unique, proceed with registration
                StartCoroutine(Main.Instance.Web.RegisterUser(NameInput.text, SurnameInput.text, UsernameInput.text, PasswordInput.text, HandleRegistrationResult));
            }
        });
    }

    // Displays if registration was success or failure
    void HandleRegistrationResult(string result)
    {
        Debug.Log(result);
        SetRegistrationStatus(result);
    }

    // Setting registration status text
    public void SetRegistrationStatus(string status)
    {
        if (RegistrationStatus != null)
        {
            RegistrationStatus.text = status;
        }
        else
        {
            Debug.LogWarning("Feedback text element not assigned in the inspector.");
        }
    }
}
