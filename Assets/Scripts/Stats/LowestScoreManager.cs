using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LowestScoreManager : MonoBehaviour
{
    public Text lowestScoreText;

    void Start()
    {
        StartCoroutine(GetLowestScore());
    }

    // Retrieving lowest score from server
    IEnumerator GetLowestScore()
    {
        string phpScriptUrl = "http://localhost/SpaceInvaders/getLowestscoreF.php";

        using (UnityWebRequest www = UnityWebRequest.Get(phpScriptUrl))
        {
            yield return www.SendWebRequest();

            // Check if the request was success or failure
            if (www.result == UnityWebRequest.Result.Success)
            {
                // Log the lowest score to the console
                Debug.Log("Lowest Score: " + www.downloadHandler.text);

                if (lowestScoreText != null)
                {
                    // Update the Text component with the lowest score
                    lowestScoreText.text = www.downloadHandler.text;
                }
            }
            else
            {
                // Log an error if the request fails
                Debug.LogError("Error: " + www.error);
            }
        }
    }
}
