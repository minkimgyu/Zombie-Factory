using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class LifeData
{
    public float maxHp;
    public IIdentifiable.Type type;

    public LifeData(float maxHp, IIdentifiable.Type type)
    {
        this.maxHp = maxHp;
        this.type = type;
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

    public LifeFactory(AddressableHandler addressableHandler, BaseFactory effectFactory)
    {
        _lifeCreaters = new Dictionary<BaseLife.Name, LifeCreater>();
        _lifeCreaters[BaseLife.Name.Player] = new PlayerCreater(addressableHandler.LifePrefabs[BaseLife.Name.Player], addressableHandler.LifeDataDictionary[BaseLife.Name.Player], effectFactory);
        //_lifeCreaters[BaseLife.Name.Oryx] = new HelperCreater(lifePrefabs[BaseLife.Name.Oryx], lifeDatas[BaseLife.Name.Oryx]);
        //_lifeCreaters[BaseLife.Name.Rook] = new HelperCreater(lifePrefabs[BaseLife.Name.Rook], lifeDatas[BaseLife.Name.Rook]);
        //_lifeCreaters[BaseLife.Name.Warden] = new HelperCreater(lifePrefabs[BaseLife.Name.Warden], lifeDatas[BaseLife.Name.Warden]);

        //_lifeCreaters[BaseLife.Name.Mask] = new ZombieCreater(lifePrefabs[BaseLife.Name.Mask], lifeDatas[BaseLife.Name.Mask]);
        //_lifeCreaters[BaseLife.Name.Police] = new ZombieCreater(lifePrefabs[BaseLife.Name.Police], lifeDatas[BaseLife.Name.Police]);
        //_lifeCreaters[BaseLife.Name.Witch] = new ZombieCreater(lifePrefabs[BaseLife.Name.Witch], lifeDatas[BaseLife.Name.Witch]);
        //_lifeCreaters[BaseLife.Name.Mild] = new ZombieCreater(lifePrefabs[BaseLife.Name.Mild], lifeDatas[BaseLife.Name.Mild]);
    }

    public override BaseLife Create(BaseLife.Name name)
    {
        return _lifeCreaters[name].Create();
    }
}
