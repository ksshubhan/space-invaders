using System.Collections.Generic;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    [Header("Invaders")]
    public Invader[] prefabs = new Invader[5];
    public AnimationCurve speed = new AnimationCurve();
    public Vector3 direction { get; private set; } = Vector3.right;
    public Vector3 initialPosition { get; private set; }

    [Header("Grid")]
    public int rows = 5;
    public int columns = 11;

    [Header("Missiles")]
    public Projectile missilePrefab;
    public float missileSpawnRate = 2f;

    public float firingRangeThreshold = 5f;

    private Transform playerTransform; 

    public float baseProjectileSpeed = 15f; 
    public float increasePerInvaderKilled = 25f; 

    private void Awake()
    {
        initialPosition = transform.position;
        playerTransform = FindObjectOfType<Player>().transform; 
        CreateInvaderGrid();
    }

    private void Start()
    {
        playerTransform = FindObjectOfType<Player>()?.transform; 

        if (playerTransform == null)
        {
            Debug.LogError("Player transform not found!");
        }
        else
        {
            Debug.Log("Player transform not found!" + playerTransform.gameObject.name);
            
            // Decrease the missile spawn rate to increase firing frequency
            InvokeRepeating(nameof(MissileAttack), 0f, missileSpawnRate);
        }
    }

    private void CreateInvaderGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            float width = 2f * (columns - 1);
            float height = 2f * (rows - 1);

            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);

            for (int j = 0; j < columns; j++)
            {
                Invader invader = Instantiate(prefabs[i], transform);

                Vector3 position = rowPosition;
                position.x += 2f * j;
                invader.transform.localPosition = position;
            }
        }
    }

    private void MissileAttack()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player transform not found!");
            return;
        }

        int playerColumnIndex = Mathf.RoundToInt(playerTransform.position.x);

        // Creating a list to store invaders in the firing range column
        List<Transform> invadersInFiringRangeColumn = new List<Transform>();

        // Iterating over all invaders to find those within the firing range column
        foreach (Transform invader in transform)
        {
            int invaderColumnIndex = Mathf.RoundToInt(invader.position.x);

            if (invaderColumnIndex == playerColumnIndex && invader.gameObject.activeInHierarchy)
            {
                invadersInFiringRangeColumn.Add(invader);
            }
        }

        // If there are invaders in the firing range column, select a random one to fire a projectile
        if (invadersInFiringRangeColumn.Count > 0)
        {
            Transform invaderToFire = invadersInFiringRangeColumn[Random.Range(0, invadersInFiringRangeColumn.Count)];
            Debug.Log("Projectile fired from invader at position: " + invaderToFire.position);
            
            Projectile projectileInstance = Instantiate(missilePrefab, invaderToFire.position, Quaternion.identity);
            float percentInvadersKilled = 1f - ((float)GetAliveCount() / (float)(rows * columns));
            float newProjectileSpeed = baseProjectileSpeed + (increasePerInvaderKilled * percentInvadersKilled);
            
            projectileInstance.speed = newProjectileSpeed;
            Debug.Log("Projectile speed: " + projectileInstance.speed + "Percent invaders killed: " + percentInvadersKilled);
        }
    }


    private void Update()
    {
        // Calculating percentage of invaders killed
        int totalCount = rows * columns;
        int amountAlive = GetAliveCount();
        int amountKilled = totalCount - amountAlive;
        float percentKilled = (float)amountKilled / (float)totalCount;

        // Evaluating speed of invaders based on how many have been killed
        float speed = this.speed.Evaluate(percentKilled);
        transform.position += direction * speed * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        // Invaders will advance to the next row after reaching the edge the screen
        foreach (Transform invader in transform)
        {
            // Skip invaders that have been killed
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            // Checking the left edge or right edge based on the current direction
            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1f))
            {
                AdvanceRow();
                break;
            }
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1f))
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {
        // Flipping direction invaders are moving
        direction = new Vector3(-direction.x, 0f, 0f);

        // Moving grid of invaders down a row
        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }

    // When the round is over the invaders will return
    // to their original positions 
    public void ResetInvaders()
    {
        direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }

    // How many invaders are left
    public int GetAliveCount()
    {
        int count = 0;

        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf)
            {
                count++;
            }
        }

        return count;
    }

}
