using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeProjectileL : MazeProjectile
{
    public void Start()
    {
        direction = Vector3.left;
        speed = 20f;
        transform.position = GameObject.Find("LeftLaserPos").transform.position;
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        Debug.Log("Leftward projectile collided!");
    }
}
