using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighestScoreManager : MonoBehaviour
{
    public Text HighestScoreText;

    void Start()
    {
        StartCoroutine(GetHighScore());
    }
    
    // Fetching result for highest score from server
    IEnumerator GetHighScore()
    {
        // Sending a UnityWebRequest to the server
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/SpaceInvaders/getHighestscoreF.php"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error: " + www.error);
            }
            else
            {
                // Parse the response and update the UI text with the highest score
                if (HighestScoreText != null)
                {
                    HighestScoreText.text = www.downloadHandler.text;
                }
                Debug.Log("High Score: " + www.downloadHandler.text);
            }
        }
    }
}
