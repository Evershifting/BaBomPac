using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Config/Level")]
public class ConfigLevel : ScriptableObject
{
    [SerializeField]
    int sizeX, sizeY, enemyAmount;
    [SerializeField]
    [Tooltip("Error allowance for enemies' pathing")]
    int error;
    [SerializeField]
    float enemySpeed;

    public int SizeX
    {
        get
        {
            return sizeX;
        }
    }
    public int SizeY
    {
        get
        {
            return sizeY;
        }
    }
    public int EnemyAmount
    {
        get
        {
            return enemyAmount;
        }
    }
    public float EnemySpeed
    {
        get
        {
            return enemySpeed;
        }
    }
    public int Error
    {
        get
        {
            return error;
        }
    }
}
