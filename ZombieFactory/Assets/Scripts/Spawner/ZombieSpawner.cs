using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    BaseFactory _zombieFactory;
    [SerializeField] int _zombieCount = 5;
    [SerializeField] float _range = 3f;

    BaseLife.Name[] zombies = { BaseLife.Name.PoliceZombie}; // , BaseLife.Name.WitchZombie, BaseLife.Name.MaskZombie 

    public void Initialize(BaseFactory zombieFactory)
    {
        _zombieFactory = zombieFactory;

        for (int i = 0; i < _zombieCount; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * _range;
            Vector3 randomRange = new Vector3(randomPos.x, transform.position.y, randomPos.y);

            Debug.Log(randomRange);

            Spawn(randomRange);
        }
    }

    void Spawn(Vector3 randomRange)
    {
        BaseLife.Name zombieName = zombies[Random.Range(0, zombies.Length)];
        BaseLife zombie = _zombieFactory.Create(zombieName);
        zombie.transform.position = randomRange;
    }
}
