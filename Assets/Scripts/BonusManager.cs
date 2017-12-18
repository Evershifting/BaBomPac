using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BonusManager : MonoBehaviour {

    [Inject]
    Config _config;
    [Inject]
    FieldManager _fieldManager;
    public int currentBonusAmount = 0, maximumBonusAmount = 3;
    float spawnTimer = 1f;
    float timer = 0f;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnTimer)
        {
            timer = 0f;
            SpawnBonus();
        }
    }

    private void SpawnBonus()
    {
        int randomRoll;
        randomRoll = Random.Range(0, 100);
        if (randomRoll < _config.SpawnChance && currentBonusAmount < maximumBonusAmount)
        {
            currentBonusAmount++;
            GameObject bonus;
            randomRoll = Random.Range(0, _config.Bonuses.Count);
            bonus = Instantiate(_config.Bonuses[randomRoll]);
            bonus.name = _config.Bonuses[randomRoll].name;
            randomRoll = Random.Range(0, _fieldManager.EmptyCells.Count);
            bonus.transform.position = _fieldManager.EmptyCells[randomRoll].GetPositionVector3();
            bonus.transform.position += Vector3.up / 2f;
            bonus.transform.parent = _fieldManager.BonusParent;
            _fieldManager.EmptyCells.Remove(_fieldManager.EmptyCells[randomRoll]);
        }
    }

}
