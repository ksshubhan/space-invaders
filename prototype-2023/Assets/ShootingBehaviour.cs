using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// When the space key is pressed the player's ship
// is able to shoot bullets at the enemy ships 
// </summary>
 
public class ShootingBehaviour : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawnPoint.up * bulletSpeed;
        }
    }
}
