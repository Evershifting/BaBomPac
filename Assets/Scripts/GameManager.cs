﻿using System.Collections;
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
    int currentLevel, foodAmount, life;

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

    public int Life
    {
        get
        {
            return life;
        }

        set
        {
            life = value;
            if (life <=0)
            {
                Debug.Log("GameOver");
            }
        }
    }

    private void Start()
    {
        Life = _config.BaseAmountOfLifes;
        StartLevel(currentLevel);
    }

    void StartLevel(int level)
    {
        _fieldManager.SpawnField(_config.Levels[level].SizeX, _config.Levels[CurrentLevel].SizeY);
    }



}
