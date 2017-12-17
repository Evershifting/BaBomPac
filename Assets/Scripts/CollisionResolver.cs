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
            _gameManager.FoodAmount--;
        }
        if (collider.tag == "Enemy")
        {
            Debug.Log("Boom");
        }
        if (collider.tag == "Bonus")
        {

        }
    }
}
