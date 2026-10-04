using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerCountDisplay : MonoBehaviour
{
    public Text playerCountText;

    private string apiUrl = "http://localhost/SpaceInvaders/getPlayerCountF.php";

    void Start()
    {
        StartCoroutine(GetPlayerCount());
    }

    // Getting number of players from server 
    IEnumerator GetPlayerCount()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            try
            {
                HandleResponse(www);
            }
            catch (Exception e)
            {
                HandleErrorResponse("Exception during HTTP request: " + e.Message);
            }
        }
    }

    // Handling HTTP response
    private void HandleResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            HandleSuccessResponse(www.downloadHandler.text);
        }
        else
        {
            HandleErrorResponse("HTTP request failed. Status Code: " + www.responseCode);
        }
    }

    // If JSON response successful return number of players
    private void HandleSuccessResponse(string jsonResponse)
    {
        try
        {
            PlayerCountData data = JsonUtility.FromJson<PlayerCountData>(jsonResponse);

            if (playerCountText != null)
            {
                playerCountText.text = "Number of players: " + data.playerCount;
            }
            Debug.Log("Number of players: " + data.playerCount);
        }
        catch (Exception e)
        {
            HandleErrorResponse("Exception during JSON parsing: " + e.Message);
        }
    }

    // If JSON response not successful report error
    private void HandleErrorResponse(string errorMessage)
    {
        Debug.LogError("Error fetching player count: " + errorMessage);
    }

    // Serilizable class to hold JSON data
    [System.Serializable]
    public class PlayerCountData
    {
        public int playerCount;
    }
}
