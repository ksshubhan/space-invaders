using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeProjectileU : MazeProjectile
{
    public void Start()
    {
        direction = Vector3.up;
        speed = 20f;
        transform.position = GameObject.Find("UpLaserPos").transform.position;
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        Debug.Log("Upward projectile collided!");
    }
}
