using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MysteryShip : MonoBehaviour
{
    public float speed = 5f;
    public float cycleTime = 30f;
    public int score = 300;

    public Vector2 leftDestination { get; private set; }
    public Vector2 rightDestination { get; private set; }
    public int direction { get; private set; } = -1;
    public bool spawned { get; private set; }

    private void Start()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main camera not found. Ensure a camera is tagged as 'MainCamera' in the scene.");
            return;
        }

        // Setting MysteryShip's coordinates
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        leftDestination = new Vector2(leftEdge.x - 1f, transform.position.y);
        rightDestination = new Vector2(rightEdge.x + 1f, transform.position.y);

        Despawn();
    }

    private void Update()
    {
        if (!spawned)
        {
            return;
        }

        MoveShip();
    }

    private void MoveShip()
    {
        Vector3 movementDirection = (direction == 1) ? Vector3.right : Vector3.left;

        transform.position += movementDirection * speed * Time.deltaTime;

        if ((direction == 1 && transform.position.x >= rightDestination.x) ||
            (direction == -1 && transform.position.x <= leftDestination.x))
        {
            Despawn();
        }
    }

    private void Spawn()
    {
        direction *= -1;

        // Setting ship's initial position based on direction
        transform.position = (direction == 1) ? leftDestination : rightDestination;

        spawned = true;
    }

    private void Despawn()
    {
        spawned = false;

        // Setting the ship's off-screen position based on direction
        transform.position = (direction == 1) ? rightDestination : leftDestination;

        if (cycleTime > 0)
        {
            Invoke(nameof(Spawn), cycleTime);
        }
        else
        {
            Debug.LogWarning("cycleTime should be greater than zero to respawn the Mystery Ship.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Despawn();
            GameManager.Instance?.OnMysteryShipKilled(this);
        }
    }
}
