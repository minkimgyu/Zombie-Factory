using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ZombieSpawner : BaseSpawner
{
    [SerializeField] Color _rangeColor;
    [SerializeField] BaseLife.Name[] _lifeNames;
    [SerializeField] float _range = 3f;

    BaseFactory _lifeFactory;
    int _spawnCount;
    Action OnDie;

    public override void Initialize(int spawnCount, BaseFactory factory, Action OnDie) 
    {
        _spawnCount = spawnCount;
        _lifeFactory = factory;
        this.OnDie = OnDie;
    }

    protected BaseLife CreateRandomLife()
    {
        BaseLife.Name lifeName = _lifeNames[Random.Range(0, _lifeNames.Length)];
        return _lifeFactory.Create(lifeName);
    }

    protected Vector3 ReturnRandomPos()
    {
        Vector2 randomPos = Random.insideUnitCircle * _range;
        return transform.position + new Vector3(randomPos.x, 0, randomPos.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _rangeColor;
        Gizmos.DrawSphere(transform.position, _range);
    }

    public override void Spawn()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            BaseLife zombie = CreateRandomLife();
            Vector3 pos = ReturnRandomPos();

            zombie.AddObserverEvent(OnDie);
            zombie.ResetPosition(pos);
        }
    }
}
