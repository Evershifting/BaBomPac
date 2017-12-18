using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemySpawner
{
    [Inject]
    Config _config;
    [Inject]
    GameManager _gameManager;
    [Inject]
    FieldManager _fieldManager;
    [Inject]
    readonly Enemy.Pool _enemyPool;
    readonly List<Enemy> _enemies = new List<Enemy>();

    public List<Enemy> Enemies
    {
        get
        {
            return _enemies;
        }
    }

    public void Add(Vector3 position)
    {
        var enemy = _enemyPool.Spawn(_config.Levels[_gameManager.CurrentLevel].EnemySpeed);
        enemy.transform.position = position;
        enemy.MoveToPosition = _fieldManager.GetCellFromPosition(position);
        enemy.Speed *= (1 + Random.Range(-0.2f, 0.2f));
        Enemies.Add(enemy);
    }
    public void Remove(Enemy enemy)
    {
        _enemyPool.Despawn(enemy);
        Enemies.Remove(enemy);
    }
    public void Clean()
    {
        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
            _enemyPool.Despawn(Enemies[i]);
            Enemies.Remove(Enemies[i]);
        }
    }
    public void Clear()
    {
        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
            Remove(Enemies[i]);
        }
    }
}
