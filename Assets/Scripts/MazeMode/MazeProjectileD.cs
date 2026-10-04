using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeProjectileD : MazeProjectile
{
    public void Start()
    {
        direction = Vector3.down;
        speed = 20f;
        transform.position = GameObject.Find("DownLaserPos").transform.position;
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
    Debug.Log("Downward projectile collided!");
    }
}
