using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Zenject;

public class BonusEffects : MonoBehaviour
{
    [Inject]
    Player _player;
    [Inject]
    EnemySpawner _enemySpawner;
    [Inject]
    Config _config;
    [Inject]
    GameManager _gameManager;
    [Inject]
    UIManager _UIManager;
    public static BonusEffects instance;
    void Awake()
    {
        instance = this;
    }
    public void Ghost()
    {
        _player.IsFlying = true;
    }
    public void GhostBad()
    {
        foreach (Enemy enemy in _enemySpawner.Enemies)
        {
            enemy.IsFlying = true;
            enemy.GetComponent<Renderer>().material.color = Color.red / 2f + Color.blue / 2f;
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
            mi = typeof(BonusEffects).GetMethod(_config.Bonuses[random].name);
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
        _UIManager.UpdateUI();
    }
    public void Flame()
    {
        _player.FlameCharges = 3;
        _UIManager.UpdateUI();
    }
}
