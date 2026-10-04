using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Web : MonoBehaviour
{
    [Serializable]
    public class TitleInfoWrapper
    {
        public List<TitleInfo> titles;
    }

    [Serializable]
    public class UsernameExistsResponse
    {
        public bool usernameExists;
    }

    [Serializable]
    public class TitleInfo
    {
        public string ID;
        public string title_name;
        public string price;
    }

    [Serializable]
    public class MostExpensiveTitleResponse
    {
        public string title_name;
    }

    [Serializable]
    public class FirstNameResponse
    {
        public string firstName;
    }

    [Serializable]
    public class SurnameResponse
    {
        public string surname;
    }

    // Fields to store server responses and user data
    public string id;
    public string titles;
    private string loginResponse;
    private string registrationStatus;
    private string mostExpensiveTitle;
    private string userSurname;
    private string userFirstName;
    
    private SceneLoader sceneLoader;




    // Sets the most expensive title
    public void SetMostExpensiveTitle(string title)
    {
        mostExpensiveTitle = title;
    }

    // Gets the most expensive title
    public string GetMostExpensiveTitle()
    {
        return mostExpensiveTitle;
    }

    // Sets the user's surname
    public void SetUserFirstName(string firstname)
    {
        userFirstName = firstname;
    }

    // Gets the user's surname
    public string GetUserFirstName()
    {
        return userFirstName;
    }

    // Sets the user's surname
    public void SetUserSurname(string surname)
    {
        userSurname = surname;
    }

    // Gets the user's surname
    public string GetUserSurname()
    {
        return userSurname;
    }


   // Handling user login 
   public IEnumerator LoginUser(string username, string password)
   {
      string fetchedSalt = null;

      // Fetching salt from the server
      yield return StartCoroutine(FetchSaltCoroutine(username, (salt) =>
      {
          fetchedSalt = salt;
      }));

      if (fetchedSalt != null)
      {
          // Combining password and salt and hash them
          string hashedPassword = PasswordManager.HashPassword(password +      fetchedSalt);
          Debug.Log(fetchedSalt);
          Debug.Log(hashedPassword);

          // Sending hashed password to the server for authentication
          WWWForm form = new WWWForm();
          form.AddField("username", username);
          form.AddField("password", hashedPassword);

          using (UnityWebRequest www =   UnityWebRequest.Post("http://localhost/SpaceInvaders/LoginFP5.php", form))
           {
              yield return www.SendWebRequest();

              if (www.result != UnityWebRequest.Result.Success)
              {
                Debug.LogError(www.error);
              }
              else
              {
                  // Save login response and proceed if login is successful
                  loginResponse = www.downloadHandler.text;
                  if (loginResponse.Equals("Login successful"))
                  {
                    yield return GetUserIDFromServer(username);
                    PlayerPrefs.SetInt("PlayerID", GetLoggedInUserId());
                  }
              }
          }
      }
      else
      {
          Debug.LogError("Failed to fetch salt for user: " + username);
      }
  }




   // Fetching salt from the database
   private IEnumerator FetchSaltCoroutine(string username, Action<string> callback)
    {
  yield return FetchSaltFromDatabase(username, callback);
    }

    // Fetching login response
    public string GetLoginResponse()
    {
        return loginResponse;
    }

    // Fetching registration response
    public string GetRegisterReponse()
    {
        return registrationStatus;
    }

    // Getting titles for a specific user 
    public IEnumerator GetTitles(int userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", userID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SpaceInvaders/getTitlesF.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Save titles and update PlayerPrefs
                titles = www.downloadHandler.text;
                PlayerPrefs.SetString("PlayerTitles", titles);
            }
            else
            {
                Debug.LogError("Error fetching titles: " + www.error);
            }
        }
    }


    // Handling login and subsequent fetching of titles 
    public IEnumerator FetchTitles(string username, string password)
    {
        yield return LoginUser(username, password);

        if (!string.IsNullOrEmpty(id))
        {
            int userID = Convert.ToInt32(id);
            yield return GetTitles(userID);
        }
    }


    // Handling user registration 
    public IEnumerator RegisterUser(string name, string surname, string username,                     string password, Action<string> callback)
{
    // Generate a random salt
    string salt = GenerateSalt();
    Debug.Log(salt);

    // Combine salt and password
    string saltedPassword = password + salt;

    // Hash the salted password
    string hashedPassword = PasswordManager.HashPassword(saltedPassword);


    WWWForm form = new WWWForm();
    form.AddField("name", name);
    form.AddField("surname", surname);
    form.AddField("username", username);
    form.AddField("password", hashedPassword);
    form.AddField("salt", salt);

    using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SpaceInvaders/RegisterFP4.php", form))
    {
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            callback?.Invoke("Registration failed. Please try again later.");
        }
        else
        {
            // Save registration response and invoke the callback
            registrationStatus = www.downloadHandler.text;
            if (registrationStatus.Equals("Registration successful"))
            {
                callback?.Invoke("Registration successful");
                yield return new WaitForSeconds(2.0f);
                SceneManager.LoadScene("StartScreen");
            }
            else
            {
                callback?.Invoke("Registration failed. Please check your inputs and try again.");
            }
        }
    }
}

private string GenerateSalt()
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    char[] saltChars = new char[16];
    for (int i = 0; i < saltChars.Length; i++)
    {
        saltChars[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
    }
    return new string(saltChars);
}

// Fetching salt from the database
public IEnumerator FetchSaltFromDatabase(string username, Action<string> callback)
{
    WWWForm form = new WWWForm();
    form.AddField("username", username);

    using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SpaceInvaders/fetchsaltfromdatabase.php", form))
    {
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch salt: " + www.error);
            callback?.Invoke(null); 
        }
        else
        {
            string salt = www.downloadHandler.text;
            callback?.Invoke(salt); 
        }
    }
}



    // Checking if a username already exists
    public IEnumerator CheckUsernameExists(string username, Action<bool> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SpaceInvaders/CheckUsernameExistsF.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Parsing the server response 
                string response = www.downloadHandler.text;
                bool usernameExists = JsonUtility.FromJson<UsernameExistsResponse>(response).usernameExists;
                callback?.Invoke(usernameExists);
            }
            else
            {
                Debug.LogError("Error checking username existence: " + www.error);
                callback?.Invoke(false); // Assume username doesn't exist in case of an error
            }
        }
    }


    // Getting user ID from the server
    public IEnumerator GetUserIDFromServer(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SpaceInvaders/getUserIDF.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Save user ID and update PlayerPrefs
                id = www.downloadHandler.text;
                PlayerPrefs.SetInt("PlayerID", GetLoggedInUserId());
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }




    // Fetching user's first name from the server
    public IEnumerator GetUserFirstName(int userID, Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SpaceInvaders/getUserFirstNameF.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Parsing the server response 
                string response = www.downloadHandler.text;
                try
                {
                    FirstNameResponse firstNameResponse = JsonUtility.FromJson<FirstNameResponse>(response);

                    if (firstNameResponse != null)
                    {
                        callback?.Invoke(firstNameResponse.firstName);
                    }
                    else
                    {
                        Debug.LogError("Error deserializing JSON: FirstNameResponse is null.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error deserializing JSON: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError("Error fetching user first name: " + www.error);
            }
        }
    }





    // Fetching user's surname from the server
    public IEnumerator GetUserSurname(int userID, Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SpaceInvaders/getUserSurnameF.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Parsing the server response 
                string response = www.downloadHandler.text;
                try
                {
                    SurnameResponse surnameResponse = JsonUtility.FromJson<SurnameResponse>(response);

                    if (surnameResponse != null)
                    {
                        callback?.Invoke(surnameResponse.surname);
                    }
                    else
                    {
                        Debug.LogError("Error deserializing JSON: SurnameResponse is null.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error deserializing JSON: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError("Error fetching user surname: " + www.error);
            }
        }
    }



    // Fetching titles and prices from the server
    public IEnumerator GetTitlesAndPrices(Action<List<TitleInfo>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/SpaceInvaders/getTitlesandPricesF.php"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Parsing the server response 
                string response = www.downloadHandler.text;
                try
                {
                    TitleInfoWrapper wrapper = JsonUtility.FromJson<TitleInfoWrapper>($"{{\"titles\":{response}}}");

                    if (wrapper != null && wrapper.titles != null)
                    {
                        List<TitleInfo> titles = wrapper.titles;
                        callback?.Invoke(titles);
                    }
                    else
                    {
                        Debug.LogError("Error deserializing JSON: 'titles' property is null.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error deserializing JSON: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError("Error fetching titles and prices: " + www.error);
            }
        }
    }

    
    // Handles buying titles functionality
    public IEnumerator BuyTitle(int userID, int titleID, Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        form.AddField("titleID", titleID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SpaceInvaders/buyTitleF.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Parse the server response and invoke the callback
                string response = www.downloadHandler.text;
                callback?.Invoke(response);

                // Checking if the user owns all the titles after purchase
                yield return CheckAllTitlesOwned(userID);
            }
            else
            {
                Debug.LogError("Error purchasing title: " + www.error);
            }
        }
    }

    
    // Checking if the user owns all titles
    private IEnumerator CheckAllTitlesOwned(int userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SpaceInvaders/checkAllTitlesOwnedF.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                if (response.Equals("AllTitlesOwned"))
                {
                    // Trigger the Congrats screen
                    TriggerCongratsScreen();
                }
            }
            else
            {
                Debug.LogError("Error checking all titles owned: " + www.error);
            }
        }
    }


    // Fetching the user's most expensive title 
    public IEnumerator GetMostExpensiveTitle(int userID, Action<MostExpensiveTitleResponse> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SpaceInvaders/getMostExpensiveTitleF.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {

                // Print the raw response for debugging
                Debug.Log("Raw Server Response: " + www.downloadHandler.text);

                // Parsing the server response 
                string response = www.downloadHandler.text;
                try
                {
                    MostExpensiveTitleResponse mostExpensiveTitle = JsonUtility.FromJson<MostExpensiveTitleResponse>(response);

                    if (mostExpensiveTitle != null)
                    {
                        callback?.Invoke(mostExpensiveTitle);
                    }
                    else
                    {
                        Debug.LogError("Error deserializing JSON: MostExpensiveTitleResponse is null.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error deserializing JSON: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError("Error fetching most expensive title: " + www.error);
            }
        }
    }


    // Triggers transition to Congrats screen 
    private void TriggerCongratsScreen()
    {
        if (sceneLoader == null)
        {
            sceneLoader = SceneLoader.Instance;
        }

        if (sceneLoader != null)
        {

            sceneLoader.TransitionToCongratsScreen();
        }
        else
        {
            Debug.LogError("SceneLoader reference is null. Ensure SceneLoader is present in the scene.");
        }
    }

    // Method for getting user ID
    public int GetLoggedInUserId()
    {
        return Convert.ToInt32(id);
    }
}
