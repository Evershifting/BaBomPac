using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class FieldManager : MonoBehaviour
{
    [Inject]
    Config _config;
    [Inject]
    GameManager _gameManager;
    [Inject]
    EnemySpawner _enemySpawner;
    [SerializeField]
    List<Cell> cells, emptyCells;

    //fields used for ease of fields' creation
    GameObject instance;
    int x, y, z;
    Vector3 position;
    Transform floorParent, innerWallsParent, outerWallsParent, foodParent, bonusParent, field;

    public List<Cell> Cells
    {
        get
        {
            return cells;
        }

        set
        {
            cells = value;
        }
    }
    public List<Cell> EmptyCells
    {
        get
        {
            return emptyCells;
        }

        set
        {
            emptyCells = value;
        }
    }
    public Transform BonusParent
    {
        get
        {
            return bonusParent;
        }

        private set
        {
            bonusParent = value;
        }
    }

    private void Awake()
    {
        Cells = new List<Cell>();
        EmptyCells = new List<Cell>();
    }
    public void SpawnField(int sizeX, int sizeY)
    {
        SetupCamera(sizeX + 2, sizeY + 2);
        CleanField();

        //Field generation and filling lists of each cell's neighbours
        for (int i = 0; i < sizeX + 2; i++)
        {
            for (int j = 0; j < sizeY + 2; j++)
            {
                Cells.Add(new Cell());
                Cells[i * (sizeY + 2) + j].Position = new Vector2(i, j);
                x = i;
                y = 0;
                z = j;
                position = new Vector3(x, y, z);
                if (i >= 1 && j >= 1 && i < sizeX + 1 && j < sizeY + 1)
                {
                    instance = Instantiate(_config.Floor);
                    instance.transform.position = position;
                    instance.transform.SetParent(floorParent);
                    Cells[i * (sizeY + 2) + j].IsWalkable = true;
                    Cells[i * (sizeY + 2) + j].IsFlyable = true;

                    if (j % 2 == 0 && i % 2 == 0)
                    {
                        instance = Instantiate(_config.InnerWall);
                        instance.transform.position = position;
                        instance.transform.SetParent(innerWallsParent);
                        Cells[i * (sizeY + 2) + j].IsWalkable = false;
                    }
                    else if (!(i == 1 && j == 1))
                    {
                        instance = Instantiate(_config.Food);
                        instance.transform.position = position;
                        instance.transform.SetParent(foodParent);
                        _gameManager.FoodAmount++;
                    }
                    //Filling neighbours of each cell
                    if (j > 1)
                    {
                        Cells[i * (sizeY + 2) + j].SetNeighbour(Cells[i * (sizeY + 2) + j - 1]);
                        Cells[i * (sizeY + 2) + j - 1].SetNeighbour(Cells[i * (sizeY + 2) + j]);
                    }
                    if (i > 1)
                    {
                        Cells[i * (sizeY + 2) + j].SetNeighbour(Cells[(i - 1) * (sizeY + 2) + j]);
                        Cells[(i - 1) * (sizeY + 2) + j].SetNeighbour(Cells[i * (sizeY + 2) + j]);
                    }
                }
                else
                {
                    instance = Instantiate(_config.OuterWall);
                    instance.transform.position = position;
                    instance.transform.SetParent(outerWallsParent);
                    Cells[i * (sizeY + 2) + j].IsWalkable = false;
                    Cells[i * (sizeY + 2) + j].IsFlyable = false;
                }
                if (Cells[i * (sizeY + 2) + j].IsWalkable)
                {
                    EmptyCells.Add(Cells[i * (sizeY + 2) + j]);
                }
            }
        }
        SpawnEnemies(_config.Levels[_gameManager.CurrentLevel].EnemyAmount);
    }
    void SetupCamera(int sizeX, int sizeY)
    {
        Camera.main.transform.position = new Vector3(sizeX / 2, 10, sizeY / 2); //10 is comfortable height
        Camera.main.orthographicSize = Mathf.Max(sizeY, sizeX) / 2 + 2; //+2 to stop borders clamping to camera field of view
    }
    public void CleanField()
    {
        Cells = new List<Cell>();
        EmptyCells = new List<Cell>();

        if (field)
            Destroy(field.gameObject);
        field = new GameObject().transform;
        field.name = "field";
        field.transform.position = new Vector3(-1, 0, -1);

        BonusParent = new GameObject().transform;
        BonusParent.name = "bonusParent";
        BonusParent.transform.parent = field;

        floorParent = new GameObject().transform;
        floorParent.name = "floorParent";
        floorParent.transform.parent = field;

        foodParent = new GameObject().transform;
        foodParent.name = "foodParent";
        foodParent.transform.parent = field;

        innerWallsParent = new GameObject().transform;
        innerWallsParent.name = "innerWallsParent";
        innerWallsParent.transform.parent = field;

        outerWallsParent = new GameObject().transform;
        outerWallsParent.name = "outerWallsParent";
        outerWallsParent.transform.parent = field;

        _enemySpawner.Clean();
    }
    public void SpawnEnemies(int value)
    {
        int amount = value;
        Vector3 position;
        for (int i = _config.Levels[_gameManager.CurrentLevel].SizeX + 1; i > 1; i--)
        {
            if (amount <= 0)
            {
                break;
            }
            for (int j = _config.Levels[_gameManager.CurrentLevel].SizeY + 1; j > 1 + 1; j--)
            {
                if (amount <= 0)
                {
                    break;
                }
                if (Cells[i * (_config.Levels[_gameManager.CurrentLevel].SizeY + 2) + j].IsWalkable)
                {
                    position = new Vector3(i, 0, j);
                    _enemySpawner.Add(position);
                    amount--;
                }
            }
        }
    }

    public Cell GetCellFromPosition(Vector2 position)
    {
        int index = (int)position.x * (_config.Levels[_gameManager.CurrentLevel].SizeY + 2) + (int)position.y;
        if (index < Cells.Count)
            return Cells[index];
        else
        {
            Debug.Log("Wrong cell position. Returning cells[0]");
            return null;
        }
    }
    public Cell GetCellFromPosition(Vector3 position)
    {
        Vector2 newPosition = new Vector2(position.x, position.z);
        return GetCellFromPosition(newPosition);
    }
    public bool IsCellWalkable(Vector2 target)
    {
        Cell targetCell = GetCellFromPosition(target);
        if (targetCell != null)
        {
            return targetCell.IsWalkable;
        }
        else
        {
            return false;
        }
    }
    public bool IsCellWalkable(Vector3 target)
    {
        Vector2 position = new Vector2(target.x, target.z);
        return IsCellWalkable(position);
    }
    public bool IsCellFlyable(Vector2 target)
    {
        Cell targetCell = GetCellFromPosition(target);
        if (targetCell != null)
        {
            return targetCell.IsFlyable;
        }
        else
        {
            return false;
        }
    }
    public bool IsCellFlyable(Vector3 target)
    {
        Vector2 position = new Vector2(target.x, target.z);
        return IsCellFlyable(position);
    }
}
