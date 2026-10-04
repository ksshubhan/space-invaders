using UnityEngine;

public class MazeProjectile : MonoBehaviour
{
    public new BoxCollider2D collider { get; private set; }
    protected Vector3 direction;
    public float speed;

    public void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    protected void Update()
    {
    }

    // Method to shoot the projectile in a specific direction
    public void Shoot(Vector2 direction)
    {
        transform.right = direction;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    // Checking for collision with wall
    protected void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Wall"))
        {
            HandleCollision();
            Destroy(gameObject);
        }
    }

    // Checking nature of collision
    protected virtual void HandleCollision()
    {
        Debug.Log("Projectile collided!");
    }
}
