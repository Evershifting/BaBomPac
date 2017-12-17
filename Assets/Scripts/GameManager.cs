using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum NeighbourPosition
{
    Up, Down, Left, Right
}

public class GameManager : MonoBehaviour
{
    [Inject]
    Config _config;
    [Inject]
    FieldManager _fieldManager;
    [SerializeField]
    int currentLevel, foodAmount;

    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
    }

    public int FoodAmount
    {
        get
        {
            return foodAmount;
        }

        set
        {
            foodAmount = value;
            if (foodAmount <= 0)
            {
                currentLevel++;
                StartLevel(currentLevel);
            }
        }
    }

    private void Start()
    {
        StartLevel(currentLevel);
    }

    void StartLevel(int level)
    {
        _fieldManager.SpawnField(_config.Levels[level].SizeX, _config.Levels[CurrentLevel].SizeY);
    }



}
