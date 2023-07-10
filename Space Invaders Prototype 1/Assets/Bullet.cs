using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// When the bullet collides with the enemy ships
// The enemy ship is removed from the scene
// <summary>

public class Bullet : MonoBehaviour
{
    public float life = 3;

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}