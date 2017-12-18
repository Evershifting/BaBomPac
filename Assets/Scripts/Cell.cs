using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class Cell : IHeapItem<Cell>
{
    [SerializeField]
    bool isWalkable, isFlyable;
    List<Cell> neighbours = new List<Cell>();
    [SerializeField]
    Vector2 position = new Vector2();

    //pathfinding
    int hCost, gCost, heapIndex;
    Cell parent;

    public bool IsWalkable
    {
        get
        {
            return isWalkable;
        }
        set
        {
            isWalkable = value;
        }
    }
    public bool IsFlyable
    {
        get
        {
            return isFlyable;
        }

        set
        {
            isFlyable = value;
        }
    }
    public Vector2 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    public int HCost
    {
        get
        {
            return hCost;
        }

        set
        {
            hCost = value;
        }
    }
    public int GCost
    {
        get
        {
            return gCost;
        }

        set
        {
            gCost = value;
        }
    }
    public int FCost
    {
        get
        {
            return HCost + GCost;
        }
    }
    public List<Cell> Neighbours
    {
        get
        {
            return neighbours;
        }
    }
    public Cell Parent
    {
        get
        {
            return parent;
        }

        set
        {
            parent = value;
        }
    }
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    public void SetNeighbour(Cell cell)
    {
        Neighbours.Add(cell);
    }
    public Vector3 GetPositionVector3()
    {
        Vector3 position;
        position = new Vector3(this.position.x, 0, this.position.y);
        return position;
    }
    public int CompareTo(Cell other)
    {
        int result = FCost.CompareTo(other.FCost);
        if (result == 0)
        {
            result = GCost.CompareTo(other.GCost);
        }
        return -result; //we need to return -result because higher F or G cost means that Cell has lower priority
    }
}
