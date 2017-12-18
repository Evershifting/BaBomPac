using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    public class Pool : MonoMemoryPool<float, /*Vector3, */Enemy>
    {
        protected override void Reinitialize(float p1, /*Vector3 p2, */Enemy enemy)
        {
            enemy.Speed = p1;

        }
    }

    [Inject]
    Player _player;
    [Inject]
    Config _config;
    [Inject]
    GameManager _gameManager;
    [Inject]
    FieldManager _fieldManager;
    Cell playerPosition, moveToPosition;
    [SerializeField]
    float speed;

    bool isFlying;
    bool isMoving;
    float t;
    Vector3 previousPosition;
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
            if (speed <= _config.MinEnemySpeed)
            {
                speed = _config.MinEnemySpeed;
            }
        }
    }

    public Cell MoveToPosition
    {
        get
        {
            return moveToPosition;
        }

        set
        {
            moveToPosition = value;
        }
    }

    public bool IsFlying
    {
        get
        {
            return isFlying;
        }

        set
        {
            isFlying = value;
        }
    }

    private void Update()
    {
        if (!isMoving)
        {
            t = 0;
            previousPosition = transform.position;
            CheckPlayerPosition();
            MoveToPosition = FindPath(_fieldManager.GetCellFromPosition(transform.position), playerPosition);
            if (MoveToPosition != null)
                isMoving = true;
        }
        else
        {
            if (MoveToPosition.IsWalkable
                || (IsFlying && MoveToPosition.IsFlyable))
            {
                Move();
            }
            else
            {
                isMoving = false;
            }
        }
    }

    void Move()
    {
        t = Time.deltaTime * speed;
        transform.position = Vector3.MoveTowards(transform.position, MoveToPosition.GetPositionVector3(), t);

        if (transform.position == MoveToPosition.GetPositionVector3())
        {
            isMoving = false;
        }
    }
    void CheckPlayerPosition()
    {
        int x, y;
        x = (int)_player.transform.position.x;
        y = (int)_player.transform.position.z;
        x += Random.Range(-_config.Levels[_gameManager.CurrentLevel].Error, _config.Levels[_gameManager.CurrentLevel].Error);
        y += Random.Range(-_config.Levels[_gameManager.CurrentLevel].Error, _config.Levels[_gameManager.CurrentLevel].Error);
        x = Mathf.Clamp(x, 1, _config.Levels[_gameManager.CurrentLevel].SizeX);
        y = Mathf.Clamp(y, 1, _config.Levels[_gameManager.CurrentLevel].SizeY);
        playerPosition = _fieldManager.GetCellFromPosition(new Vector2(x, y));
        if (!playerPosition.IsWalkable || playerPosition == _fieldManager.GetCellFromPosition(transform.position))
        {
            CheckPlayerPosition();
        }
    }


    //zu
    /// <summary>
    /// Pathfinding
    /// </summary>
    /// <param name="_startPosition"></param>
    /// <param name="_targetPosition"></param>
    private Cell FindPath(Cell _startPosition, Cell _targetPosition)
    {

        Heap<Cell> openSet = new Heap<Cell>(_config.Levels[_gameManager.CurrentLevel].SizeX * _config.Levels[_gameManager.CurrentLevel].SizeY);
        HashSet<Cell> closedSet = new HashSet<Cell>();
        openSet.Add(_startPosition);

        while (openSet.Count > 0)
        {
            Cell currentCell = openSet.RemoveFirst();
            closedSet.Add(currentCell);
            if (currentCell == _targetPosition)
            {
                return RetracePath(_startPosition, _targetPosition);
            }
            foreach (Cell neighbour in currentCell.Neighbours)
            {
                if (closedSet.Contains(neighbour) || (!neighbour.IsWalkable && (!IsFlying && !neighbour.IsFlyable)))
                {
                    continue;
                }
                int newMovementCostToNeighbour = currentCell.GCost + GetDistance(currentCell, neighbour); //we can use "+ 1" instead of GetDistance(), but if later we'll have to add some "magic cells" etc. GetDistance will work better
                if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, _targetPosition);
                    neighbour.Parent = currentCell;
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        Debug.Log("No Path!");
        return null;
    }

    Cell RetracePath(Cell startCell, Cell endCell)
    {
        List<Cell> path = new List<Cell>();
        Cell currentCell = endCell;

        while (currentCell != startCell)
        {
            path.Add(currentCell);
            currentCell = currentCell.Parent;
        }
        path.Reverse();
        return path[0];
    }

    int GetDistance(Cell _start, Cell _end)
    {
        int distX = (int)Mathf.Abs(_start.Position.x - _end.Position.x);
        int distY = (int)Mathf.Abs(_start.Position.y - _end.Position.y);
        return distX + distY;
    }
}
