using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FieldManager : MonoBehaviour
{
    [Inject]
    Config _config;
    [Inject]
    GameManager _gameManager;
    [Inject]
    EnemySpawner _enemySpawner;
    [SerializeField]
    public List<Cell> cells;


    //fields used for ease of fields' creation
    GameObject instance;
    int x, y, z;
    Vector3 position;

    Transform floorParent, innerWallsParent, outerWallsParent, foodParent;
    private void Awake()
    {
        cells = new List<Cell>();
    }
    public void SpawnField(int sizeX, int sizeY)
    {
        CleanField();
        //Camera setup
        Camera.main.transform.position = new Vector3(sizeX / 2, 10, sizeY / 2); //10 is comfortable height
        Camera.main.orthographicSize = Mathf.Max(sizeY, sizeX) / 2 + 2; //+2 to stop borders clamping to camera field of view

        //Field generation with filling lists of each cell's neighbours
        for (int i = 0; i < sizeX + 2; i++)
        {
            for (int j = 0; j < sizeY + 2; j++)
            {
                cells.Add(new Cell());
                cells[i * (sizeY + 2) + j].Position = new Vector2(i, j);
                x = i;
                y = 0;
                z = j;
                position = new Vector3(x, y, z);
                if (i >= 1 && j >= 1 && i < sizeX + 1 && j < sizeY + 1)
                {
                    instance = Instantiate(_config.Floor);
                    instance.transform.position = position;
                    instance.transform.SetParent(floorParent);
                    cells[i * (sizeY + 2) + j].IsWalkable = true;
                    cells[i * (sizeY + 2) + j].IsFlyable = true;

                    if (j % 2 == 0 && i % 2 == 0)
                    {
                        //zu
                        instance = Instantiate(_config.InnerWall);
                        instance.transform.position = position;
                        instance.transform.SetParent(innerWallsParent);
                        cells[i * (sizeY + 2) + j].IsWalkable = false;
                        cells[i * (sizeY + 2) + j].HasWall = true;
                    }
                    else
                    {
                        instance = Instantiate(_config.Food);
                        instance.transform.position = position;
                        instance.transform.SetParent(foodParent);
                        cells[i * (sizeY + 2) + j].HasFood = true;
                        _gameManager.FoodAmount++;
                    }
                    //Filling neighbours of each cell
                    if (j > 1)
                    {
                        cells[i * (sizeY + 2) + j].SetNeighbour(cells[i * (sizeY + 2) + j - 1]);
                        cells[i * (sizeY + 2) + j - 1].SetNeighbour(cells[i * (sizeY + 2) + j]);
                    }
                    if (i > 1)
                    {
                        cells[i * (sizeY + 2) + j].SetNeighbour(cells[(i - 1) * (sizeY + 2) + j]);
                        cells[(i - 1) * (sizeY + 2) + j].SetNeighbour(cells[i * (sizeY + 2) + j]);
                    }
                }
                else
                {
                    instance = Instantiate(_config.OuterWall);
                    instance.transform.position = position;
                    instance.transform.SetParent(outerWallsParent);
                    cells[i * (sizeY + 2) + j].IsWalkable = false;
                    cells[i * (sizeY + 2) + j].IsFlyable = false;
                }
            }
        }
        SpawnEnemies(_config.Levels[_gameManager.CurrentLevel].EnemyAmount);
    }
    public void CleanField()
    {
        cells = new List<Cell>();
        if (floorParent)
            Destroy(floorParent.gameObject);
        floorParent = new GameObject().transform;
        floorParent.name = "floorParent";
        if (foodParent)
            Destroy(foodParent.gameObject);
        foodParent = new GameObject().transform;
        foodParent.name = "foodParent";
        if (innerWallsParent)
            Destroy(innerWallsParent.gameObject);
        innerWallsParent = new GameObject().transform;
        innerWallsParent.name = "innerWallsParent";
        if (outerWallsParent)
            Destroy(outerWallsParent.gameObject);
        outerWallsParent = new GameObject().transform;
        outerWallsParent.name = "outerWallsParent";
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
                if (cells[i * (_config.Levels[_gameManager.CurrentLevel].SizeY + 2) + j].IsWalkable)
                {
                    position = new Vector3(i, 0, j);
                    _enemySpawner.Add(position);
                    cells[i * (_config.Levels[_gameManager.CurrentLevel].SizeY + 2) + j].IsWalkable = false;
                    amount--;
                }
            }
        }
    }
    public Cell GetCellFromPosition(Vector2 position)
    {
        int index = (int)position.x * (_config.Levels[_gameManager.CurrentLevel].SizeY + 2) + (int)position.y;
        if (index < cells.Count)
            return cells[index];
        else
        {
            Debug.Log("Wrong cell position. Returning cells[0]");
            return null;
        }
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

}
