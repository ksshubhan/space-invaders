using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : MonoBehaviour
{

    [SerializeField] SpriteRenderer Powerpellets;
    public int score = 10;

    // Checking for collision with player
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            CollectPellet();
        }
    }

    // Allows player to collect pellet 
    private void CollectPellet()
    {
        MazeManager.Instance.OnPelletCollected(this);
    }
}
