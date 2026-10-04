using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine.Windows;

public class PasswordManager : MonoBehaviour
{
    public static string HashPassword(string password)
    {
        try
        {
            // Checking if the password is null or empty
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            // Initialising hash value
            int hash = 0;

            // Iterating through each character in the password
            for (int i = 0; i < password.Length; i++)
            {
                char currentChar = password[i];

                // Simple hash algorithm
                hash = (hash << 5) - hash + currentChar;
                hash = hash & hash;
            }

            // Return the hashed password as a string
            return hash.ToString();
        }
        catch (Exception ex)
        {
            // Logs an error message if an exception occurs during password hashing
            Debug.LogError("Error hashing password: " + ex.Message);
            return null; 
        }
    }
}
