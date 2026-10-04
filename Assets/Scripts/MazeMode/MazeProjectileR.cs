using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeProjectileR : MazeProjectile
{
    public void Start()
    {
        direction = Vector3.right;
        speed = 20f;
        transform.position=GameObject.Find("RightLaserPos").transform.position;
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        Debug.Log("Rightward projectile collided!");
    }
}
