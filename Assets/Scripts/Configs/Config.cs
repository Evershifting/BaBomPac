using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Config/Config")]
public class Config : ScriptableObject
{
    [Header("Levels")]
    [SerializeField]
    private List<ConfigLevel> levels;
    public List<ConfigLevel> Levels
    {
        get
        {
            return levels;
        }
    }

    [Header("Prefabs")]
    [SerializeField]
    private GameObject floor;
    [SerializeField]
    private GameObject innerWall, outerWall, food, enemy, player;

    [Header("Bonus")]
    [SerializeField, Tooltip("Chance of spawning each second, %")]
    float spawnChance = 5;
    [SerializeField]
    List<GameObject> bonuses;

    [Header("Other")]
    [SerializeField]
    float basePlayerSpeed = 5f;
    [SerializeField]
    float minEnemySpeed = 0.75f, minPlayerSpeed = 1f;
    [SerializeField]
    int baseAmountOfLifes = 3;

    public GameObject InnerWall
    {
        get
        {
            return innerWall;
        }
    }
    public GameObject OuterWall
    {
        get
        {
            return outerWall;
        }
    }
    public GameObject Food
    {
        get
        {
            return food;
        }
    }
    public GameObject Enemy
    {
        get
        {
            return enemy;
        }
    }
    public GameObject Player
    {
        get
        {
            return player;
        }
    }
    public GameObject Floor
    {
        get
        {
            return floor;
        }
    }
    public float SpawnChance
    {
        get
        {
            return spawnChance;
        }
    }
    public List<GameObject> Bonuses
    {
        get
        {
            return bonuses;
        }
    }
    public float PlayerSpeed
    {
        get
        {
            return basePlayerSpeed;
        }
    }
    public float MinEnemySpeed
    {
        get
        {
            return minEnemySpeed;
        }
    }
    public float MinPlayerSpeed
    {
        get
        {
            return minPlayerSpeed;
        }
    }
    public int BaseAmountOfLifes
    {
        get
        {
            return baseAmountOfLifes;
        }
    }
}
