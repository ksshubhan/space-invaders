using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsScreen : MonoBehaviour
{
    public Text titlesText;

    void Start()
    {
        // Retrieving titles from PlayerPrefs
        string titlesString = PlayerPrefs.GetString("PlayerTitles", "No titles available");

        // Converting the titles string to a List<string>
        List<string> ownedTitles = new List<string>(titlesString.Split(','));

        DisplayTitles(ownedTitles);
    }

    // Display titles in the UI Text component
    private void DisplayTitles(List<string> ownedTitles)
    {
        titlesText.text = "Player Titles:\n";

        foreach (string title in ownedTitles)
        {
            string formattedTitle = RemoveUnwantedCharacters(title);

            titlesText.text += formattedTitle + "\n";
        }
    }

    // Remove square brackets and quotations from the title
    private string RemoveUnwantedCharacters(string title)
    {
        return title.Replace("[", "").Replace("]", "").Replace("\"", "");
    }
}
