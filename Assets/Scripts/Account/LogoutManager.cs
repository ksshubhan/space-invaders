using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LogoutManager : MonoBehaviour
{
    [SerializeField] private Button logoutButton;

    private const string logoutURL = "http://localhost/SpaceInvaders/LogoutF.php";

    private void Start()
    {
        // Set up UI listeners and handle potential errors
        InitialiseUI();
    }

    // Handles UI
    private void InitialiseUI()
    {
        if (logoutButton != null)
        {
            logoutButton.onClick.AddListener(Logout);
        }
        else
        {
            Debug.LogError("LogoutButton not assigned in the inspector.");
        }
    }
    
    // Handles loguout process
    private void Logout()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.StartScreen);

        StartCoroutine(LogoutRequest());
    }

    // Handles logout functionality
    private IEnumerator LogoutRequest()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(logoutURL))
        {
            yield return www.SendWebRequest();

            WebRequestResult(www);
        }
    }

    // Checking result of request to server
    private void WebRequestResult(UnityWebRequest www)
    {
        if (www.result != UnityWebRequest.Result.Success)
        {
            // Log an error if the request fails
            Debug.LogError("Logout request failed: " + www.error);
        }
        else
        {
            PhpResponse(www.downloadHandler.text);
        }
    }

    // Handling the response from the PHP script
    private void PhpResponse(string response)
    {
        if (response.Contains("Logout successful"))
        {
            // Log a success message
            Debug.Log("Logout successful");
        }
        else
        {
            // Log a warning if the response indicates a failed logout
            Debug.LogWarning("Logout failed: " + response);
        }
    }
}
