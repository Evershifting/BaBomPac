using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [Inject]
    GameManager _gameManager;
    [Inject]
    FieldManager _fieldManager;
    [Inject]
    readonly SignalCollision _signalCollision;
    [Inject]
    Config _config;

    [SerializeField]
    float speed = 3f;
    [SerializeField]
    bool flying = false, shielded = false;
    [SerializeField]
    int flameCharges = 0;
    [SerializeField]
    bool isMoving = false;
    float t;
    [SerializeField]
    Vector3 targetPosition;
    [SerializeField]
    Direction direction = Direction.Stop;

    public bool IsFlying
    {
        get
        {
            return flying;
        }

        set
        {
            flying = value;
        }
    }
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
            if (speed <= _config.MinPlayerSpeed)
            {
                speed = _config.MinPlayerSpeed;
            }
        }
    }
    public bool Shielded
    {
        get
        {
            return shielded;
        }

        set
        {
            shielded = value;
        }
    }
    public int FlameCharges
    {
        get
        {
            return flameCharges;
        }

        set
        {
            flameCharges = value;
        }
    }

    public enum Direction
    {
        Stop, Up, Down, Left, Right
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            direction = Direction.Left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = Direction.Right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction = Direction.Up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = Direction.Down;
        }

        if (!isMoving)
        {
            switch (direction)
            {
                case Direction.Stop:
                    targetPosition = transform.position;
                    isMoving = false;
                    break;
                case Direction.Up:
                    targetPosition = transform.position + Vector3.forward;
                    isMoving = true;
                    break;
                case Direction.Down:
                    targetPosition = transform.position - Vector3.forward;
                    isMoving = true;
                    break;
                case Direction.Left:
                    targetPosition = transform.position - Vector3.right;
                    isMoving = true;
                    break;
                case Direction.Right:
                    targetPosition = transform.position + Vector3.right;
                    isMoving = true;
                    break;
                default:
                    break;
            }
            t = 0;
        }
        else
        {
            if (_fieldManager.IsCellWalkable(targetPosition)
                || (IsFlying && _fieldManager.IsCellFlyable(targetPosition)))
            {
                Move();
            }
            else
            {
                direction = Direction.Stop;
                isMoving = false;
            }
        }
    }

    void Move()
    {
        t = Time.deltaTime * Speed;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, t);

        if (transform.position == targetPosition)
        {
            isMoving = false;
            direction = Direction.Stop;
        }
    }

    public void Respawn()
    {
        transform.position = new Vector3(1, 0, 1);
        targetPosition = transform.position;
        Speed = _config.PlayerSpeed;
        Shielded = false;
        FlameCharges = 0;
        IsFlying = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        _signalCollision.Fire(other);
    }
}
