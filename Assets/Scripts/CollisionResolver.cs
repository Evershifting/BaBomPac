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
    [Inject]
    BonusManager _bonusManager;
    [Inject]
    EnemySpawner _enemySpawner;
    [Inject]
    UIManager _UIManager;

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
            if (_player.FlameCharges > 0)
            {
                _player.FlameCharges--;
                _enemySpawner.Remove(collider.gameObject.GetComponent<Enemy>());
                _fieldManager.SpawnEnemies(1);
                _UIManager.UpdateUI();
            }
            else if (_player.Shielded)
            {
                _player.Shielded = false;
                _UIManager.UpdateUI();
            }
            else
            {
                _enemySpawner.Remove(collider.gameObject.GetComponent<Enemy>());
                _fieldManager.SpawnEnemies(1);
                _gameManager.Life--;
                _player.Respawn();
            }

        }
        if (collider.tag == "Bonus")
        {
            _bonusManager.currentBonusAmount--;
            _fieldManager.EmptyCells.Add(_fieldManager.GetCellFromPosition(collider.transform.position));
            collider.GetComponent<Bonus>().Use();
            GameObject.Destroy(collider.gameObject);
        }
    }
}
