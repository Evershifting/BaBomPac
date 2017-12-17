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
}
