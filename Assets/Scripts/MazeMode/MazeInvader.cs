using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeInvader : MonoBehaviour
{
    public Pathfinding pathfinding;
    public MazeGenerator m_generator;

    public float pathCalculationTime = 2.0f;
    public float lastPathCalculation = 0.0f;

    private float timeStarted = 0.0f;
    public float timeTillGo = 4.0f;

    public float speed = 1.0f;
    public float speedIncreasePerPellet = 0.1f;
    private int pelletsCollected = 0;

    private Queue<MazeNode> lastPath;

    public float snapRange = 0.1f;
    private bool firstCalculation = true;


    private void Start()
    {
        timeStarted = Time.time;
    }

    private void Update()
    {
        if (Time.time < timeStarted + timeTillGo)
        {
            return;
        }

        // Calculating path and moving invader
        CalculatePath();
        MoveInvader();
    }

    // Calculating the path to the player
    private void CalculatePath()
    {
        try
        {
            if (Time.time - lastPathCalculation > pathCalculationTime || firstCalculation)
            {
                lastPathCalculation = Time.time;

                // Find the nearest player node
                MazeNode foundPlayer = FindNearestPlayerNode();

                // If a player node is found, calculate the path
                if (foundPlayer != null)
                {
                    this.lastPath = pathfinding.AStar(m_generator.InvaderNode.id, foundPlayer.id);
                    firstCalculation = false;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception during path calculation: {ex.Message}");
        }
    }

    // Finding the nearest player node
    private MazeNode FindNearestPlayerNode()
    {
        // Dictionary to store distances to each node
        Dictionary<string, float> distances = new Dictionary<string, float>();

        // Calculating distances from the invader to all nodes
        foreach (MazeNode node in m_generator.allNodes)
        {
            distances.Add(node.id, Vector3.Distance(node.transform.position, m_generator.MazeShip.transform.position));
        }

        // Find the node with the minimum distance (nearest player node)
        string foundID = distances.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
        Debug.Log(foundID);

        // Extract X and Y elements from the node ID
        string[] elements = foundID.Remove(0, 1).Split("Y");
        string lookingForID = $"X{elements[0]}Y{int.Parse(elements[1])}";
        
        // Find the MazeNode object with the corresponding ID
        MazeNode foundNode = m_generator.allNodes.Find(node => node.id == lookingForID);

        return foundNode;
    }

    // Moving invader along calculated path
    private void MoveInvader()
    {
        if (lastPath != null && lastPath.Count > 0)
        {
            speed += pelletsCollected * speedIncreasePerPellet * Time.deltaTime;

            MazeNode target = lastPath.Peek();
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.transform.position) < snapRange)
            {
                MoveToNextNode(target);
            }
        }
    }

    // Moving to next node in the path
    private void MoveToNextNode(MazeNode targetNode)
    {
        try
        {
            // Removing current target node from path
            lastPath.Dequeue();

            // Extracting X and Y elements from node ID
            string[] elements = targetNode.id.Remove(0, 1).Split("Y");
            string lookingForID = $"X{elements[0]}Y{int.Parse(elements[1])}";

            // Finding MazeNode object with the corresponding ID
            MazeNode foundNode = m_generator.allNodes.Find(node => node.id == lookingForID);

            if (foundNode == null)
            {
                throw new UnityException("Catastrophic failure");
            }

            // Update InvaderNode, reset calculation timer, and reset pellet count
            m_generator.InvaderNode = foundNode;
            lastPathCalculation = 0.0f;
            pelletsCollected = 0;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception during moving to the next node: {ex.Message}");
        }
    }

    // Checking for collisions with lasers and pellets
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            MazeManager.Instance.OnInvaderKilled(this);
        }
        else if (collider.gameObject.tag == "Collectable")
        {
            pelletsCollected++;
        }
    }
}
