using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    private new BoxCollider2D collider;
    public Vector3 direction = Vector3.up;
    public float speed = 20f;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collider2D other)
    {
        try
        {
            Bunker bunker = other.gameObject.GetComponent<Bunker>();

            if (bunker == null || bunker.CheckCollision(collider, transform.position))
            {
                Destroy(gameObject);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in CheckCollision: {ex.Message}");
        }
    }
}
