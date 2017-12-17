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
    public void Add(Vector3 position)
    {
        var enemy = _enemyPool.Spawn(_config.Levels[_gameManager.CurrentLevel].EnemySpeed);
        enemy.transform.position = position;
        enemy.MoveToPosition = _fieldManager.GetCellFromPosition(position);
        _enemies.Add(enemy);
    }
    public void Remove(Enemy enemy)
    {
        _enemyPool.Despawn(enemy);
        _enemies.Remove(enemy);
    }
    public void Clean()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            _enemyPool.Despawn(_enemies[i]);
            _enemies.Remove(_enemies[i]);
        }
    }
    public void Clear()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            Remove(_enemies[i]);
        }
    }
}
