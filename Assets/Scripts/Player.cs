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

    [SerializeField]
    Vector2 position;
    [SerializeField]
    float speed = 3f;
    [SerializeField]
    bool flying = false;
    [SerializeField]
    bool isMoving = false;
    float t;
    [SerializeField]
    Vector3 previousPosition, targetPosition;
    [SerializeField]
    Direction direction = Direction.Stop;
    public enum Direction
    {
        Stop, Up, Down, Left, Right
    }
    private void Update()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            direction = Direction.Left;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            direction = Direction.Right;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            direction = Direction.Up;
        }
        if (Input.GetAxis("Vertical") < 0)
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
            previousPosition = transform.position;
        }
        else
        {
            if (_fieldManager.IsCellWalkable(targetPosition))
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
        t += Time.deltaTime * speed;

        //zu
        transform.position = Vector3.Lerp(previousPosition, targetPosition, t);
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, t);

        if (transform.position == targetPosition)
        {
            isMoving = false;
            direction = Direction.Stop;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        _signalCollision.Fire(other);
    }
}
