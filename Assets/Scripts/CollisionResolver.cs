using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CollisionResolver
{
    [Inject]
    private SignalCollision _signalCollision;
    [Inject]
    GameManager _gameManager;
    [Inject]
    Player _player;
    [Inject]
    FieldManager _fieldManager;

    public CollisionResolver(SignalCollision signalCollision)
    {
        _signalCollision = signalCollision;
        _signalCollision.Listen(CollisionResolve);
    }
    private void OnApplicationQuit()
    {
        _signalCollision.Unlisten(CollisionResolve);
    }
    void CollisionResolve(Collider collider)
    {
        if (collider.tag == "Food")
        {
            GameObject.Destroy(collider.gameObject);
            _fieldManager.GetCellFromPosition(_player.transform.position).HasFood = false;
            _gameManager.FoodAmount--;
        }
        if (collider.tag == "Enemy")
        {
            Debug.Log("Boom");
            _player.transform.position = new Vector3(1, 0, 1);
        }
        if (collider.tag == "Bonus")
        {

        }
    }
}
