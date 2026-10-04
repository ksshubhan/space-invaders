using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    // Prefabs and UI elements
    public Text titlePrefab;
    public Text pricePrefab;
    public Button buyButtonPrefab;
    public Transform contentPanel;
    public Text purchaseResponse;

    // List to store title information
    private List<Web.TitleInfo> titlesList;

    void Start()
    {
        StartCoroutine(Main.Instance.Web.GetTitlesAndPrices(HandleTitlesAndPrices));
    }

    // Callback to handle the fetched titles and prices
    private void HandleTitlesAndPrices(List<Web.TitleInfo> titles)
    {
        titlesList = titles;

        UpdateUI();
    }

    // Updates UI with titles and prices
    private void UpdateUI()
    {
        ClearUI();

        // Set spacing between rows and elements
        float rowSpacing = 350f; 
        float elementSpacing = 10f; 

        // Displaying each title and price
        for (int i = 0; i < titlesList.Count; i++)
        {
            // Calculate position for the current row
            float rowY = (i * (titlePrefab.rectTransform.sizeDelta.y + rowSpacing));
           
            // Instantiates title Text element
            Text titleText = Instantiate(titlePrefab, contentPanel);
            titleText.text = "Title: " + titlesList[i].title_name;
            titleText.rectTransform.anchoredPosition = new Vector2(0f, rowY);

            // Instantiates price Text element
            Text priceText = Instantiate(pricePrefab, contentPanel);
            priceText.text = "Price: " + titlesList[i].price + " points";
            // Adjust the x-position of the priceText to appear next to the titleText
            priceText.rectTransform.anchoredPosition = new Vector2(titleText.rectTransform.sizeDelta.x + elementSpacing - 100f, rowY);

            // Instantiates buyButton
            Button buyButton = Instantiate(buyButtonPrefab, contentPanel);
            buyButton.GetComponentInChildren<Text>().text = "Buy";

            RectTransform buyButtonRectTransform = buyButton.gameObject.GetComponent<RectTransform>();
            buyButtonRectTransform.anchoredPosition = new Vector2(priceText.rectTransform.anchoredPosition.x - priceText.rectTransform.sizeDelta.x - 250f + elementSpacing + 180f, rowY);

            int titleID = Convert.ToInt32(titlesList[i].ID);
            buyButton.onClick.AddListener(() => OnTitleButtonClick(titleID));

            // Debug information
            Debug.Log($"Instantiated: Title = {titlesList[i].title_name}, Price = {titlesList[i].price}");
        }
    }

    // Clears existing UI elements
    private void ClearUI()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    // Handles buying titles functionality
    public void OnTitleButtonClick(int titleID)
    {
        // Allows specific user to purchase specific title depending on titleID (unique identifier)
        StartCoroutine(Main.Instance.Web.BuyTitle(PlayerPrefs.GetInt("PlayerID"), titleID, HandlePurchaseResponse));
    }

    // Handling the purchase response
    private void HandlePurchaseResponse(string response)
    { 
        Debug.Log("Purchase Response: " + response);

        if (response.Contains("Purchase successful"))
        {
            SetPurchaseResponse("Purchase successful");
        }
        else if (response == "Not enough points.")
        {
            SetPurchaseResponse(response);
        }
        else if (response == "You already own this title.")
        {
            SetPurchaseResponse(response);
        }
    }

    // Setting the purchase response text in the UI
    void SetPurchaseResponse(string message)
    {
        if (purchaseResponse != null)
        {
            // Clears the previous text before setting the new response
            purchaseResponse.text = "";

            // Sets the new response
            purchaseResponse.text = message;
        }
        else
        {
            Debug.LogWarning("Feedback text element not assigned in the inspector.");
        }
    }
}
