using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
   private MazeGenerator m_Generator;
    private void Awake()
    {
        m_Generator = FindObjectOfType<MazeGenerator>();
    }

    // A* algorithm for pathfinding from startID to goalID
    public Queue<MazeNode> AStar(string startID, string goalID)
    {
        MazeNode start = null;
        MazeNode goal = null;

        // Finding the start node and the goal node based on their IDs
        for (int x = 0; x < m_Generator.mazeSize.x; x++)
        {
            for (int y = 0; y < m_Generator.mazeSize.y; y++)
            {
                if (m_Generator.allNodes[x + (y * m_Generator.mazeSize.x)].id == startID) start = m_Generator.allNodes[x + ((y * m_Generator.mazeSize.x))];
                if (m_Generator.allNodes[x + (y * m_Generator.mazeSize.x)].id == goalID) goal = m_Generator.allNodes[x + ((y * m_Generator.mazeSize.x))];
            }
        }

        // Dictionaries to store the next node to reach the goal and the cost to reach node
        Dictionary<string, MazeNode> NextNodeToGoal = new Dictionary<string, MazeNode> (); 
        Dictionary<string, int> costToReachNode = new Dictionary<string, int> ();

        // Priority queue for nodes to be explored
        PriorityQueue<MazeNode> frontier = new PriorityQueue<MazeNode>();
        
        frontier.Enqueue(goal, 0);
        costToReachNode[goal.id] = 0; 

        while (frontier.Count > 0)
        {
            MazeNode curNode = frontier.Dequeue();
            curNode.SetState(NodeState.Unused);

            // Checking for walls in different directions to determine if the move is valid
            foreach (MazeNode neighbour in m_Generator.Neighbours(curNode))
            {
                int newCost = costToReachNode[curNode.id] + neighbour._Cost;
                if (costToReachNode.ContainsKey(neighbour.id) == false || newCost < costToReachNode[neighbour.id]) 
                {
                    // right wall
                    if (curNode.transform.position.x < neighbour.transform.position.x)
                    {
                        if (IsWallNegX(neighbour) == true)
                        {
                            continue;
                        }
                    }
                    // left wall
                    else if (curNode.transform.position.x > neighbour.transform.position.x)
                    {
                        if (IsWallPosX(neighbour) == true)
                        {
                            continue;
                        }
                    }
                    // top wall
                    else if (curNode.transform.position.y < neighbour.transform.position.y)
                    {
                        if (IsWallNegZ(neighbour) == true)
                        {
                            continue;
                        }
                    }
                    // bottom wall
                    else if (curNode.transform.position.y > neighbour.transform.position.y)
                    {
                        if (IsWallPosZ(neighbour) == true)
                        {
                            continue;
                        }
                    }

                    // If there is no wall present
                    costToReachNode.Add(neighbour.id, newCost); 
                    
                    // Used for testing
                    // neighbour.SetState(NodeState.Path);
                    
                    int priority = newCost;
                    frontier.Enqueue(neighbour, priority);
                    
                    if (NextNodeToGoal.ContainsKey(neighbour.id) == false)
                    {
                        NextNodeToGoal.Add(neighbour.id, curNode); 
                    }

                    else
                    {
                        NextNodeToGoal[neighbour.id] = curNode;
                    }    
                }    
            }
        }

        // Reconstructing path from goal to start
        Queue<MazeNode> path = new Queue<MazeNode> ();
        MazeNode pathNode = start;

        while (goal.id != pathNode.id)
        {
            pathNode.SetState(NodeState.Path);
            pathNode = NextNodeToGoal[pathNode.id];
            path.Enqueue(pathNode); 
        }
        return path;
    }

    // Checking if there is a wall in the positive x direction 
    bool IsWallPosX(MazeNode neighbour)
    {
        if (neighbour.PosXWall.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }  
    }
    
    // Checking if there is a wall in the negative x direction
    bool IsWallNegX(MazeNode neighbour)
    {
        if (neighbour.NegXWall.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Checking if there is a wall in the positive z direction
    bool IsWallPosZ(MazeNode neighbour)
    {
        if (neighbour.PosZWall.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Checking if there is a wall in the negative z direction
    bool IsWallNegZ(MazeNode neighbour)
    {
        if (neighbour.NegZWall.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
