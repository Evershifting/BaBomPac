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
    Player _player;
    [Inject]
    Config _config;
    [Inject]
    GameManager _gameManager;
    [Inject]
    FieldManager _fieldManager;
    [SerializeField]
    Cell playerPosition;
    float speed;

    bool isMoving;
    float t;
    Vector3 previousPosition;
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    private void Update()
    {
        //zu
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Azu");
            CheckPlayerPosition();
        }

        if (!isMoving)
        {
            t = 0;
            previousPosition = transform.position;
            FindPath(playerPosition);
            isMoving = true;
        }
        else
        {
            if (playerPosition.IsWalkable)
            {
                Move();
            }
            else
            {
                isMoving = false;
            }
        }
    }

    void Move()
    {
        t = Time.deltaTime * speed;
        transform.position = Vector3.MoveTowards(transform.position, playerPosition.GetPositionVector3(), t);

        if (transform.position == playerPosition.GetPositionVector3())
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
    }

    private void FindPath(Cell playerPosition)
    {
        throw new NotImplementedException();
    }
}
