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
    [Inject]
    UIManager _UIManager;
    [Inject]
    Player _player;
    [SerializeField]
    int currentLevel, foodAmount, life;

    bool isGameOver = false;

    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
            if (currentLevel >= _config.Levels.Count)
            {
                GameOver(true);
            }
            else
            {
                StartLevel(currentLevel);
            }
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
            _UIManager.UpdateUI();
            foodAmount = value;
            if (foodAmount <= 0)
            {
                CurrentLevel++;
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
            _UIManager.UpdateUI();
            if (life <= 0)
            {
                GameOver(false);
            }
        }
    }

    private void Start()
    {
        Life = _config.BaseAmountOfLifes;
        StartLevel(currentLevel);
    }
    private void Update()
    {
        if (isGameOver)
        {
            if (Input.anyKeyDown)
            {
                isGameOver = false;
                _UIManager.GameOver(true);
                _player.gameObject.SetActive(true);
                StartLevel(0);
            }
        }
    }
    void GameOver(bool gameWon)
    {
        _player.gameObject.SetActive(false);
        _fieldManager.CleanField();
        isGameOver = true;
        _UIManager.GameOver(gameWon);
    }
    void StartLevel(int level)
    {
        _fieldManager.SpawnField(_config.Levels[level].SizeX, _config.Levels[CurrentLevel].SizeY);
        _UIManager.UpdateUI();
    }
}
