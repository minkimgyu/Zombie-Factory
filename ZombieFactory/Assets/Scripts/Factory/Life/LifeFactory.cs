using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class LifeData
{
    public float maxHp;

    public LifeData(float maxHp)
    {
        this.maxHp = maxHp;
    }
}

abstract public class LifeCreater
{
    protected BaseLife _lifePrefab;
    protected LifeData _lifeData;

    public LifeCreater(BaseLife lifePrefab, LifeData lifeData)
    { _lifePrefab = lifePrefab; _lifeData = lifeData; }

    public abstract BaseLife Create();
}

public class LifeFactory : BaseFactory
{
    Dictionary<BaseLife.Name, LifeCreater> _lifeCreaters;

    public LifeFactory(AddressableHandler addressableHandler, BaseFactory effectFactory, BaseFactory ragdollFactory)
    {
        _lifeCreaters = new Dictionary<BaseLife.Name, LifeCreater>();
        _lifeCreaters[BaseLife.Name.Player] = new PlayerCreater(addressableHandler.LifePrefabs[BaseLife.Name.Player], addressableHandler.LifeDataDictionary[BaseLife.Name.Player], effectFactory);
        _lifeCreaters[BaseLife.Name.Rook] = new HelperCreater(addressableHandler.LifePrefabs[BaseLife.Name.Rook], addressableHandler.LifeDataDictionary[BaseLife.Name.Rook], effectFactory);




        //_lifeCreaters[BaseLife.Name.Oryx] = new HelperCreater(lifePrefabs[BaseLife.Name.Oryx], lifeDatas[BaseLife.Name.Oryx]);
        //_lifeCreaters[BaseLife.Name.Warden] = new HelperCreater(lifePrefabs[BaseLife.Name.Warden], lifeDatas[BaseLife.Name.Warden]);

        //_lifeCreaters[BaseLife.Name.Mask] = new ZombieCreater(addressableHandler.LifePrefabs[BaseLife.Name.Mask], addressableHandler.LifeDataDictionary[BaseLife.Name.Mask], effectFactory);
        _lifeCreaters[BaseLife.Name.PoliceZombie] = new ZombieCreater(addressableHandler.LifePrefabs[BaseLife.Name.PoliceZombie], addressableHandler.LifeDataDictionary[BaseLife.Name.PoliceZombie], effectFactory, ragdollFactory);
        //_lifeCreaters[BaseLife.Name.Witch] = new ZombieCreater(addressableHandler.LifePrefabs[BaseLife.Name.Witch], addressableHandler.LifeDataDictionary[BaseLife.Name.Witch], effectFactory);
        //_lifeCreaters[BaseLife.Name.Mild] = new ZombieCreater(addressableHandler.LifePrefabs[BaseLife.Name.Mild], addressableHandler.LifeDataDictionary[BaseLife.Name.Mild], effectFactory);
    }

    public override BaseLife Create(BaseLife.Name name)
    {
        return _lifeCreaters[name].Create();
    }
}
