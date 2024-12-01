using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

abstract public class ItemCreater
{
    protected BaseItem _prefab;
    protected ItemData _data;

    public ItemCreater(BaseItem prefab, ItemData data)
    {
        _prefab = prefab; 
        _data = data;
    }

    public abstract BaseItem Create();
}

public class ItemFactory : BaseFactory
{
    Dictionary<BaseItem.Name, ItemCreater> _weaponCreaters;

    public ItemFactory(AddressableHandler addressableHandler, BaseFactory effectFactory)
    {
        _weaponCreaters = new Dictionary<BaseItem.Name, ItemCreater>();

        // 여기서 추가
        _weaponCreaters[BaseItem.Name.AidPack] =

        new AidPackCreater(
           addressableHandler.ItemPrefabs[BaseItem.Name.AidPack],
           new AidPackData(30)
       );

        _weaponCreaters[BaseItem.Name.AmmoPack] =

        new AmmoPackCreater(
           addressableHandler.ItemPrefabs[BaseItem.Name.AmmoPack],
           new AmmoPackData(100)
        );

        _weaponCreaters[BaseItem.Name.Vandal] =

        new AutomaticGunCreater(
           addressableHandler.ItemPrefabs[BaseItem.Name.Vandal],
           addressableHandler.ItemDataDictionary[BaseItem.Name.Vandal],
           addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.Vandal],
           effectFactory
       );

        _weaponCreaters[BaseItem.Name.Phantom] = 
            
        new AutomaticGunCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.Phantom], 
            addressableHandler.ItemDataDictionary[BaseItem.Name.Phantom],
            addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.Phantom],
            effectFactory
        );

        _weaponCreaters[BaseItem.Name.Odin] =

        new AutomaticGunCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.Odin],
            addressableHandler.ItemDataDictionary[BaseItem.Name.Odin],
            addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.Odin],
            effectFactory
        );

        _weaponCreaters[BaseItem.Name.Judge] =

        new JudgeCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.Judge],
            addressableHandler.ItemDataDictionary[BaseItem.Name.Judge],
            addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.Judge],
            effectFactory
        );

        _weaponCreaters[BaseItem.Name.Stinger] =

        new StingerCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.Stinger],
            addressableHandler.ItemDataDictionary[BaseItem.Name.Stinger],
            addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.Stinger],
            addressableHandler.RightRecoilDataDictionary[BaseItem.Name.Stinger],
            effectFactory
        );

        _weaponCreaters[BaseItem.Name.Guardian] =

        new GuardianCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.Guardian],
            addressableHandler.ItemDataDictionary[BaseItem.Name.Guardian],
            addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.Guardian],
            effectFactory
        );

        _weaponCreaters[BaseItem.Name.Bucky] =

       new BuckyCreater(
           addressableHandler.ItemPrefabs[BaseItem.Name.Bucky],
           addressableHandler.ItemDataDictionary[BaseItem.Name.Bucky],
           addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.Bucky],
           addressableHandler.RightRecoilDataDictionary[BaseItem.Name.Bucky],
           effectFactory
       );

        _weaponCreaters[BaseItem.Name.Operator] =

        new OperatorCreater(
           addressableHandler.ItemPrefabs[BaseItem.Name.Operator],
           addressableHandler.ItemDataDictionary[BaseItem.Name.Operator],
           addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.Operator],
           addressableHandler.RightRecoilDataDictionary[BaseItem.Name.Operator],
           effectFactory
        );

        _weaponCreaters[BaseItem.Name.Classic] =

        new ClassicCreater(
           addressableHandler.ItemPrefabs[BaseItem.Name.Classic],
           addressableHandler.ItemDataDictionary[BaseItem.Name.Classic],
           addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.Classic],
           addressableHandler.RightRecoilDataDictionary[BaseItem.Name.Classic],
           effectFactory
        );

        _weaponCreaters[BaseItem.Name.Knife] =

        new KnifeCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.Knife],
            addressableHandler.ItemDataDictionary[BaseItem.Name.Knife],
            effectFactory
        );
    }

    public override BaseItem Create(BaseItem.Name name)
    {
        return _weaponCreaters[name].Create();
    }
}
