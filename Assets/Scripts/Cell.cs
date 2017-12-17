using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class Cell
{
    [SerializeField]
    bool isWalkable = true, isFlyable;
    List<Cell> neighbours = new List<Cell>();
    [SerializeField]
    Vector2 position = new Vector2();
    bool hasFood = false, hasWall = false, isEmpty = true;
    GameObject wall, food;
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
    public bool HasFood
    {
        get
        {
            return hasFood;
        }

        set
        {
            hasFood = value;
        }
    }
    public bool HasWall
    {
        get
        {
            return hasWall;
        }

        set
        {
            hasWall = value;
        }
    }
    public bool IsEmpty
    {
        get
        {
            return isEmpty;
        }

        set
        {
            isEmpty = value;
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

    public void SetNeighbour(Cell cell)
    {
        neighbours.Add(cell);
    }
    public Vector3 GetPositionVector3()
    {
        Vector3 position;
        position = new Vector3(this.position.x, 0, this.position.y);
        return position;
    }
}
