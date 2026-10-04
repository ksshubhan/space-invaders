using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum NodeState
{ 
    Start,
    End,
    Path,
    Unused
}

public class MazeNode : MonoBehaviour
{
    // Wall game objects for each direction
    public GameObject PosXWall;
    public GameObject NegXWall;
    public GameObject PosZWall;
    public GameObject NegZWall;

    // Unique identifier for each maze node
    public string id;
    
    // x and y coordinates for maze node
    private int _x;
    private int _y;

    [SerializeField] GameObject[] walls;
    [SerializeField] SpriteRenderer floor;

    // Indicator object to highlight the node
    public GameObject indicator;

    // Removing specific wall depending on index 
    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].gameObject.SetActive(false);
    }

    // Used for testing
    // Visual indicator of node state
    public void SetState(NodeState state)
    {
        switch (state)
        {
            case NodeState.Start:
            
            case NodeState.End:
            
            case NodeState.Path:
                this.indicator.SetActive(true);
                break;
            
            case NodeState.Unused:
            
            default:
                this.indicator.SetActive(false);
                break;
        }
    }

    // Constructor to set unique id for maze mode
    public MazeNode SetID(string _id)
    {   
        this.id = _id;
        return this;
    }

    // Getting cost of moving through maze node
    public int _Cost
    {
        get
        {
            return 1;
        }
    }
    
    // Getting x and y coordinates of maze node
    public int _X => _x;
    public int _Y => _y;
}
