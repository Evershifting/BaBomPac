using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pathfinder  {

    [Inject]
    Config _config;
    [Inject]
    GameManager _gameManager;
    [Inject]
    FieldManager _fieldManager;
    public Cell FindPath(Enemy enemy, Cell _startPosition, Cell _targetPosition)
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
                return RetracePath(enemy, _startPosition, _targetPosition);
            }
            foreach (Cell neighbour in currentCell.Neighbours)
            {
                if (closedSet.Contains(neighbour) ||
                (!enemy.IsFlying && !neighbour.IsWalkable) ||
                (enemy.IsFlying && !neighbour.IsFlyable))
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
        return null;
    }
    Cell RetracePath(Enemy enemy, Cell startCell, Cell endCell)
    {
        List<Cell> path = new List<Cell>();
        Cell currentCell = endCell;

        while (currentCell != startCell)
        {
            path.Add(currentCell);
            currentCell = currentCell.Parent;
        }
        path.Reverse();
        if (path.Count > 0)
            return path[0];
        else
            return _fieldManager.GetCellFromPosition(enemy.transform.position);
    }
    int GetDistance(Cell _start, Cell _end)
    {
        int distX = (int)Mathf.Abs(_start.Position.x - _end.Position.x);
        int distY = (int)Mathf.Abs(_start.Position.y - _end.Position.y);
        return distX + distY;
    }
}
