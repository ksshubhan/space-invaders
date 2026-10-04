using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject instructionsView;
    [SerializeField] private Button instructionsButton;

    private void Start()
    {
        InitializeInstructionsMenu();
    }

    // Initialises the Instructions menu
    private void InitializeInstructionsMenu()
    {

        if (instructionsView != null)
        {
            instructionsView.SetActive(false);
        }
        else
        {
            Debug.LogWarning("InstructionsView is not assigned in the inspector. Please assign it to ensure proper functionality.");
        }

        if (instructionsButton != null)
        {
            instructionsButton.onClick.AddListener(ToggleInstructions);
        }
        else
        {
            Debug.LogWarning("InstructionsButton is not assigned in the inspector. Please assign it to ensure proper functionality.");
        }
    }

    // Allows user to toggle the Instructions menu's visibility
    public void ToggleInstructions()
    {
        if (instructionsView != null)
        {
            instructionsView.SetActive(!instructionsView.activeSelf);
        }
        else
        {
            Debug.LogError("InstructionsView reference is null. Ensure InstructionsView is present in the scene.");
        }
    }
}
