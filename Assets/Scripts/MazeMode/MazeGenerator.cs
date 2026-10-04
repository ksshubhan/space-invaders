using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] PowerPellet pelletPrefab;
    [SerializeField] public Vector2Int mazeSize;

    public GameObject MazeShip;
    public GameObject MazeInvader;
    public MazeNode[,] grid;
    public MazeNode PlayerNode;
    public MazeNode InvaderNode;

    public List<MazeNode> allNodes;

    public int numToRemove = 5;

    private Dictionary<string, MazeNode[]> neighbourDictionary;

    public MazeNode[] Neighbours(MazeNode node)
    {
        return neighbourDictionary[node.id];
    }
    
    public IEnumerator GenerateMaze(Vector2Int size)
    {
        neighbourDictionary = new Dictionary<string, MazeNode[]>();

        List<MazeNode> nodes = new List<MazeNode>();
        List<PowerPellet> pellets = new List<PowerPellet>();
        
        // Creating nodes
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2 nodePos = new Vector2(x - (size.x / 2f), y - (size.y / 2f));
                
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                newNode.SetID($"X{x}Y{y}");
                nodes.Add(newNode);

                Vector2 pelletPos = new Vector2(x - (size.x /2f), y - (size.y / 2f));
                
                PowerPellet newPellet = Instantiate(pelletPrefab, pelletPos, Quaternion.identity, transform);
                pellets.Add(newPellet);

                yield return null;
            }
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                List<MazeNode> neighbours = new List<MazeNode>();
                if (y < size.y - 1)
                    neighbours.Add(nodes[x + ((y + 1) * mazeSize.x)]);
                if (x < size.x - 1)
                    neighbours.Add(nodes[(x + 1) + (y  * mazeSize.x)]);
                if (y > 0)
                    neighbours.Add(nodes[x + ((y - 1) * mazeSize.x)]);
                if (x > 0)
                    neighbours.Add(nodes[x - 1 + (y * mazeSize.x)]);

                neighbourDictionary.Add(nodes[x + ((y) * mazeSize.x)].id, neighbours.ToArray());

                yield return null;
            }
        }

        this.allNodes = nodes;

        Stack<MazeNode> currentPath = new Stack<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();
        
        // Choosing starting node
        PlayerNode = nodes[Random.Range(0, nodes.Count)];
        currentPath.Push(PlayerNode);
        this.MazeShip.transform.position = currentPath.Peek().transform.position;


        Vector3 ztempS = this.MazeShip.transform.position;
        ztempS.z -= 1;
        this.MazeShip.transform.position = ztempS;
        
        Vector3 ytempS = this.MazeShip.transform.position;
        ytempS.y -= 1;
        this.MazeShip.transform.position = ytempS;

        InvaderNode = nodes[Random.Range(0, nodes.Count)];
        this.MazeInvader.transform.position = InvaderNode.transform.position;

        Vector3 ztempI = this.MazeInvader.transform.position;
        ztempI.z -= 1;
        this.MazeInvader.transform.position = ztempI;

        Vector3 ytempI = this.MazeInvader.transform.position;
        ytempI.y -= 1;
        this.MazeInvader.transform.position = ytempI;
        
        // Used for testing
        // currentPath[0].SetState(NodeState.End);

        while (completedNodes.Count < nodes.Count)
        {
            // Checking nodes next to current node 
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath.Peek());
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;   

            if (currentNodeX < size.x - 1)
            {
                // Checking node to the right of the current node 
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) && !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                // Checking node to the left of the current node 
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) && !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Checking node above current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) && !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Checking node below current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) && !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choosing next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                // Removing walls to clear a path
                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath.Peek().RemoveWall(0);
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath.Peek().RemoveWall(1);
                        break;
                    case 3:     
                        chosenNode.RemoveWall(3);
                        currentPath.Peek().RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath.Peek().RemoveWall(3);
                        break;
                }
                    
                currentPath.Push(chosenNode);
                
                // Used for testing 
                // chosenNode.SetState(NodeState.End);

            }
            else
            {
                completedNodes.Add(currentPath.Peek());
                
                // Used for testing 
                // currentPath[currentPath.Count - 1].SetState(NodeState.Path);
                
                currentPath.Pop();
            }

            yield return new WaitForSeconds(0.05f);
        }

        int wallsRemoved = 0;
        
        while(wallsRemoved < numToRemove)
        {
            int index = Random.Range(0, this.allNodes.Count);
            if (index < 9 || index % 9 == 0 || (index - 8) % 9 == 0 || index > 71)
            {
                continue;
            }
            MazeNode randomNode = this.allNodes[index];

            if (randomNode)

            switch(Random.Range(0, 4))
            {
                case 0:
                    // east
                    randomNode.PosXWall.SetActive(false);
                    this.allNodes[index + 9].NegXWall.SetActive(false);
                    break;
                case 1:
                    // west
                    randomNode.NegXWall.SetActive(false);
                    this.allNodes[index - 9].PosXWall.SetActive(false);
                    break;
                case 2:
                    // north
                    randomNode.PosZWall.SetActive(false);
                    this.allNodes[index + 1].NegZWall.SetActive(false);
                    break;
                case 3:
                default:
                    // south
                    randomNode.NegZWall.SetActive(false);
                        this.allNodes[index - 1].PosZWall.SetActive(false);
                    break;
            }

            wallsRemoved++;
        }

        MazeShip.GetComponent<MazePlayer>().enabled = true;
        MazeInvader.GetComponent<MazeInvader>().enabled = true;
    }
}
