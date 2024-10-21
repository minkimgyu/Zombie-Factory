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

    public virtual BaseLife Create() { return default; }
    public virtual BaseLife Create(List<BaseItem.Name> weaponNames) { return default; }
}

public class LifeFactory : BaseFactory
{
    Dictionary<BaseLife.Name, LifeCreater> _lifeCreaters;

    public LifeFactory(AddressableHandler addressableHandler, HelperMediator mediator, BaseFactory effectFactory, BaseFactory itemFactory, BaseFactory ragdollFactory)
    {
        _lifeCreaters = new Dictionary<BaseLife.Name, LifeCreater>();

        _lifeCreaters[BaseLife.Name.Player] = new PlayerCreater(
            addressableHandler.LifePrefabs[BaseLife.Name.Player],
            mediator,
            addressableHandler.LifeDataDictionary[BaseLife.Name.Player],
            effectFactory);




        _lifeCreaters[BaseLife.Name.Warden] = new SwatCreater(
            addressableHandler.LifePrefabs[BaseLife.Name.Warden],
            mediator,
            addressableHandler.LifeDataDictionary[BaseLife.Name.Warden],
            itemFactory,
            ragdollFactory,
            effectFactory);


        _lifeCreaters[BaseLife.Name.Rook] = new SwatCreater(
            addressableHandler.LifePrefabs[BaseLife.Name.Rook],
            mediator,
            addressableHandler.LifeDataDictionary[BaseLife.Name.Rook],
            itemFactory,
            ragdollFactory,
            effectFactory);




        _lifeCreaters[BaseLife.Name.WitchZombie] = new ZombieCreater(
            addressableHandler.LifePrefabs[BaseLife.Name.WitchZombie],
            addressableHandler.LifeDataDictionary[BaseLife.Name.WitchZombie],
            effectFactory,
            ragdollFactory);

        _lifeCreaters[BaseLife.Name.PoliceZombie] = new ZombieCreater(
            addressableHandler.LifePrefabs[BaseLife.Name.PoliceZombie],
            addressableHandler.LifeDataDictionary[BaseLife.Name.PoliceZombie],
            effectFactory,
            ragdollFactory);

        _lifeCreaters[BaseLife.Name.MaskZombie] = new ZombieCreater(
            addressableHandler.LifePrefabs[BaseLife.Name.MaskZombie],
            addressableHandler.LifeDataDictionary[BaseLife.Name.MaskZombie],
            effectFactory,
            ragdollFactory);
    }

    public override BaseLife Create(BaseLife.Name name)
    {
        return _lifeCreaters[name].Create();
    }

    public override BaseLife Create(BaseLife.Name name, List<BaseItem.Name> itemNames)
    {
        return _lifeCreaters[name].Create(itemNames);
    }
}
