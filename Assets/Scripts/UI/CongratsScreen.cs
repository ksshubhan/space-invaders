using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CongratsScreen : MonoBehaviour
{
    [SerializeField] private Text congratsText;
    [SerializeField] private float textSpeed = 0.1f;

    private void Start()
    {
        StartCoroutine(DisplayCongratsText());
    }

    // Obtaining information for congrats message
    private IEnumerator DisplayCongratsText()
    {
        int userID = Main.Instance.Web.GetLoggedInUserId();

        yield return StartCoroutine(Main.Instance.Web.GetUserFirstName(userID, HandleUserFirstNameResponse));

        string congratsMessage = $"Well done {Main.Instance.Web.GetUserFirstName()}! You have beaten Space Invaders";

        yield return DisplayTextWithSpeed(congratsMessage, textSpeed);

        yield return new WaitForSeconds(1f);
    }

    // Callback method to handle the user's first name response
    private void HandleUserFirstNameResponse(string firstName)
    {
        if (Main.Instance.Web != null)
        {
            Main.Instance.Web.SetUserFirstName(firstName);
        }
        else
        {
            Debug.LogError("Web reference is null. Ensure Main.Instance.Web is properly initialized.");
        }
    }

    // Displays text using progressive disclosure 
    private IEnumerator DisplayTextWithSpeed(string fullText, float speed)
    {
        string currentText = "";

        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            congratsText.text = currentText;

            yield return new WaitForSeconds(speed);
        }
    }
}
