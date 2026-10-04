using UnityEngine;

public class MazePlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;

    public MazeProjectileR laserRPrefab;
    private MazeProjectileR laserR;

    public MazeProjectileL laserLPrefab;
    private MazeProjectileL laserL;

    public MazeProjectileU laserUPrefab;
    private MazeProjectileU laserU;

    public MazeProjectileD laserDPrefab;
    private MazeProjectileD laserD;

    public float laserCooldown = 0.5f;
    private float lastShot = 0.0f;

    private bool isArrowKeyPressed = false;

    void FixedUpdate()
    {
        PlayerMovement();
    }

    private void Update()
    {
        LaserShooting();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerCollision(collider);
    }

    private void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Moving the player based on input
        rb.velocity = new Vector2(horizontalInput, verticalInput) * moveSpeed;

        // Checking if arrow key is pressed
        isArrowKeyPressed = (horizontalInput != 0 || verticalInput != 0);
    }

    private void LaserShooting()
    {
        // Checking if enough time has passed since the last laser shot
        if (Time.time - lastShot > laserCooldown)
        {
            // Checking for input 
            if (isArrowKeyPressed && Input.GetKeyDown(KeyCode.Space))
            {
                ShootProjectile();
            }
        }
    }

    // Determining direction based on arrow key pressed
    private void ShootProjectile()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ShootDirection(Vector2.left, laserLPrefab);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            ShootDirection(Vector2.right, laserRPrefab);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            ShootDirection(Vector2.up, laserUPrefab);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            ShootDirection(Vector2.down, laserDPrefab);
        }
    }

    private void ShootDirection(Vector2 direction, MazeProjectile projectilePrefab)
    {
        // Spawns projectile based on direction 
        MazeProjectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.Shoot(direction);
        lastShot = Time.time;
    }

    private void PlayerCollision(Collider2D collider)
    {
        // Checking if the player collided with an invader
        if (collider.tag == "Invader")
        {
            MazeManager.Instance.OnPlayerKilled(this);
        }
    }
}
