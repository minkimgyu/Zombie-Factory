using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleStage : BaseStage
{
    ItemSpawner _weaponSpawner;
    ZombieSpawner[] _zombieSpawners;

    int _spawnCount = 0;
    int _maxSpawnCount = 5;

    public override void Initialize(
        BaseFactory lifeFactory,
        BaseFactory weaponFactory,

        Action OnStageClearRequested,
        Action OnMoveToNextStageRequested)
    {
        base.Initialize(lifeFactory, weaponFactory, OnStageClearRequested, OnMoveToNextStageRequested);
        _weaponSpawner = GetComponentInChildren<ItemSpawner>();
        _weaponSpawner.Initialize(weaponFactory);

        _zombieSpawners = GetComponentsInChildren<ZombieSpawner>();
        for (int i = 0; i < _zombieSpawners.Length; i++)
        {
            _zombieSpawners[i].Initialize(_maxSpawnCount, lifeFactory, OnZombieDie);
        }
    }

    void OnZombieDie()
    {
        _spawnCount -= 1;
        Debug.Log(_spawnCount);

        if (_spawnCount == 0)
        {
            OnStageClearRequested?.Invoke();
        }
    }

    public override void Spawn()
    {
        _weaponSpawner.Spawn();
        _spawnCount = _maxSpawnCount * _zombieSpawners.Length;
        for (int i = 0; i < _zombieSpawners.Length; i++)
        {
            _zombieSpawners[i].Spawn();
        }
    }
}
