using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PointsUpdater : MonoBehaviour
{
    private string phpScriptURL = "http://localhost/SpaceInvaders/updatePointsF.php";

    // Updating points for the logged-in user
    public void UpdatePoints(int points)
    {
        // Getting logged-in user's ID
        int userId = Main.Instance?.Web?.GetLoggedInUserId() ?? -1;

        // Checking if valid user ID obtained
        if (userId == -1)
        {
            Debug.LogError("Failed to get user ID for updating points. Aborting.");
            return;
        }

        StartCoroutine(SendUpdateRequest(userId, points));
    }

    // Coroutine to send the update request to the server
    public IEnumerator SendUpdateRequest(int userId, int newPoints)
    {
        // Validateing input parameters
        if (userId < 0 || newPoints < 0)
        {
            Debug.LogError("Invalid input parameters for updating points. Aborting.");
            yield break;
        }

        // Creating a form to send data to the server
        WWWForm form = new WWWForm();
        form.AddField("id", userId);
        form.AddField("points", newPoints);

        // Sending the UnityWebRequest to update points
        using (UnityWebRequest www = UnityWebRequest.Post(phpScriptURL, form))
        {
            yield return www.SendWebRequest();

            // Checking for errors in the request
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error updating points: " + www.error);
            }
            else
            {
                Debug.Log("Points updated successfully");
            }
        }
    }
}
