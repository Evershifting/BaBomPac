using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Zenject;

public class BonusManager : MonoBehaviour
{
    [Inject]
    Player _player;
    [Inject]
    EnemySpawner _enemySpawner;
    [Inject]
    Config _config;
    [Inject]
    GameManager _gameManager;
    public static BonusManager instance;
    void Awake()
    {
        instance = this;
    }
    public void Ghost()
    {
        _player.Flying = true;
    }
    public void GhostBad()
    {
        foreach (Enemy enemy in _enemySpawner.Enemies)
        {
            enemy.IsFlying = true;
        }
    }
    public void Haste()
    {
        _player.Speed *= 1.5f;
    }
    public void HasteBad()
    {
        foreach (Enemy enemy in _enemySpawner.Enemies)
        {
            enemy.Speed *= 1.5f;
        }
    }
    public void Freeze()
    {
        foreach (Enemy enemy in _enemySpawner.Enemies)
        {
            enemy.Speed *= 0.5f;
        }
    }
    public void FreezeBad()
    {
        _player.Speed *= 0.5f;
    }
    public void HolyRandom()
    {
        if (_config.Bonuses.Count>1)
        {
            MethodInfo mi;
            int random = Random.Range(0, _config.Bonuses.Count);
            mi = typeof(BonusManager).GetMethod(_config.Bonuses[random].name);
            mi.Invoke(this, null);
            Debug.Log("Holy Random brings you " + _config.Bonuses[random].name);
        }
    }
    public void Life()
    {
        _gameManager.Life++;
    }
    public void LifeBad()
    {
        _gameManager.Life--;
    }
    public void Shield()
    {
        _player.Shielded = true;
    }
    public void Flame()
    {
        _player.FlameCharges = 3;
    }
}
