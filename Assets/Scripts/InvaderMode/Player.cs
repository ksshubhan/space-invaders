using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Projectile laserPrefab;
    private Projectile activeLaser;

    private void Update()
    {
        MovePlayer();
        ClampPlayerPosition();
        FireLaser();
    }

    // Updating player position based on the input
    private void MovePlayer()
    {
        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += speed * Time.deltaTime;
        }

        transform.position = position;
    }

    // Setting boundaries for player
    private void ClampPlayerPosition()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftEdge.x, rightEdge.x), transform.position.y, transform.position.z);
    }

    private void FireLaser()
    {
        // Only one laser can be active at a time, so check that there is not already an active laser
        if (activeLaser == null && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            activeLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsEnemyCollision(other))
        {
            GameManager.Instance?.OnPlayerKilled(this);
        }
    }

    // Checking if there is a collision with hostiles i.e (Missile or Invader)
    private bool IsEnemyCollision(Collider2D other)
    {
        return other.gameObject.layer == LayerMask.NameToLayer("Missile") || other.gameObject.layer == LayerMask.NameToLayer("Invader");
    }
}
