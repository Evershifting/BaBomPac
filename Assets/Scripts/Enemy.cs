using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    public class Pool : MonoMemoryPool<float, /*Vector3, */Enemy>
    {
        protected override void Reinitialize(float p1, /*Vector3 p2, */Enemy enemy)
        {
            enemy.Speed = p1;

        }
    }
    [Inject]
    EnemySpawner _enemySpawner;
    [Inject]
    Player _player;
    [Inject]
    Config _config;
    [Inject]
    GameManager _gameManager;
    [Inject]
    FieldManager _fieldManager;
    [Inject]
    Pathfinder _pathfinder;
    Cell playerPosition, moveToPosition;
    [SerializeField]
    float speed;

    bool isFlying;
    bool isMoving;
    float t;
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
            if (speed <= _config.MinEnemySpeed)
            {
                speed = _config.MinEnemySpeed;
            }
        }
    }
    public Cell MoveToPosition
    {
        get
        {
            return moveToPosition;
        }

        set
        {
            moveToPosition = value;
        }
    }
    public bool IsFlying
    {
        get
        {
            return isFlying;
        }

        set
        {
            isFlying = value;
        }
    }

    private void Update()
    {
        if (!isMoving)
        {
            t = 0;
            CheckPlayerPosition();
            MoveToPosition = _pathfinder.FindPath(this, _fieldManager.GetCellFromPosition(transform.position), playerPosition);
            if (MoveToPosition != null /*&& PositionAvailable()*/)
                isMoving = true;
        }
        else
        {
            if (MoveToPosition.IsWalkable || (IsFlying && MoveToPosition.IsFlyable))
            {
                Move();
            }
            else
            {
                isMoving = false;
            }
        }
    }

    private bool PositionAvailable()
    {
        bool movePossible = true;
        foreach (Enemy enemy in _enemySpawner.Enemies)
        {
            if (enemy.MoveToPosition == MoveToPosition && enemy != this)
            {
                movePossible = false;
                Debug.Log("Move not possible");
                break;
            }
        }
        return movePossible;
    }

    void Move()
    {
        t = Time.deltaTime * speed;
        transform.position = Vector3.MoveTowards(transform.position, MoveToPosition.GetPositionVector3(), t);

        if (transform.position == MoveToPosition.GetPositionVector3())
        {
            isMoving = false;
        }
    }
    void CheckPlayerPosition()
    {
        int x, y;
        x = (int)_player.transform.position.x;
        y = (int)_player.transform.position.z;
        x += Random.Range(-_config.Levels[_gameManager.CurrentLevel].Error, _config.Levels[_gameManager.CurrentLevel].Error);
        y += Random.Range(-_config.Levels[_gameManager.CurrentLevel].Error, _config.Levels[_gameManager.CurrentLevel].Error);
        x = Mathf.Clamp(x, 1, _config.Levels[_gameManager.CurrentLevel].SizeX);
        y = Mathf.Clamp(y, 1, _config.Levels[_gameManager.CurrentLevel].SizeY);
        playerPosition = _fieldManager.GetCellFromPosition(new Vector2(x, y));
        if (!isFlying && !playerPosition.IsWalkable) //player is hiding in wall
        {
            playerPosition = _fieldManager.GetCellFromPosition(transform.position);
        }
        else
        if ((IsFlying && !playerPosition.IsFlyable) ||
            playerPosition == _fieldManager.GetCellFromPosition(transform.position))
        {
            //CheckPlayerPosition();
            playerPosition = _fieldManager.GetCellFromPosition(transform.position);
        }
    }


    //zu
   
}
