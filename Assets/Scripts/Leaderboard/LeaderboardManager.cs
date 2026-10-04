using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public string player;
    public int points;
}

[System.Serializable]
public class PlayerDataArray
{
    public List<PlayerData> items;
}

public class LeaderboardManager : MonoBehaviour
{
    public Text leaderboardText;

    void Start()
    {
        // Fetching and displaying leaderboard when the script starts
        StartCoroutine(GetLeaderboard());
    }

    // Fetching leadboard data from the server
    IEnumerator GetLeaderboard()
    {
        // Send a UnityWebRequest to the server
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/SpaceInvaders/getLeaderboardF.php"))
        {
            yield return www.SendWebRequest();

            // Check for errors during the request
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // Parse JSON data and display in UI
                string json = www.downloadHandler.text;
                Debug.Log("Received JSON: " + json);

                // Attempt to parse JSON data directly as an array
                try
                {
                    // Deserialize JSON array into PlayerDataArray object
                    PlayerDataArray leaderboardDataArray = JsonUtility.FromJson<PlayerDataArray>("{\"items\":" + json + "}");

                    // Call method to display the leaderboard
                    DisplayLeaderboard(leaderboardDataArray.items);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error parsing JSON: " + e.Message);
                }
            }
        }
    }

    // Handles displaying leaderboad in scene 
    void DisplayLeaderboard(List<PlayerData> leaderboardData)
    {
        leaderboardText.text = "LEADERBOARD\n\n";

        // Calculate the maximum length of player names
        int maxNameLength = leaderboardData.Max(player => player.player.Length);

        // Loop through each player data and format the leaderboard entry
        for (int i = 0; i < leaderboardData.Count; i++)
        {
            string positionWord = GetPositionWord(i + 1);
            PlayerData player = leaderboardData[i];

            // Calculate padding based on the maximum name length and adding some extra space
            int padding = maxNameLength - player.player.Length + 4;

            // Use string formatting for aligned columns
            string leaderboardEntry = $"{positionWord,-4}   {player.player.PadRight(maxNameLength + 4)}   {player.points,4}";

            leaderboardText.text += leaderboardEntry + "\n";
        }
    }

    // Getting the word representation of the position (e.g., 1st, 2nd, 3rd)
    string GetPositionWord(int position)
    {
        switch (position)
        {
            case 1: return "1ST";
            case 2: return "2ND";
            case 3: return "3RD";
            case 4: return "4TH";
            case 5: return "5TH";
            default: return position.ToString(); 
        }
    }
}
