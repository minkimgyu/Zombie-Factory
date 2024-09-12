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
        
        _weaponCreaters[BaseItem.Name.AR] = 
            
        new AutomaticGunCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.AR], 
            addressableHandler.ItemDataDictionary[BaseItem.Name.AR],
            addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.AR],
            effectFactory
        );

        _weaponCreaters[BaseItem.Name.AK] =

         new AutomaticGunCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.AK],
            addressableHandler.ItemDataDictionary[BaseItem.Name.AK],
            addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.AK],
            effectFactory
        );

        _weaponCreaters[BaseItem.Name.Bat] =

        new BatCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.Bat],
            addressableHandler.ItemDataDictionary[BaseItem.Name.Bat]
        );

        _weaponCreaters[BaseItem.Name.AutoShotgun] = 
            
        new JudgeCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.AutoShotgun],
            addressableHandler.ItemDataDictionary[BaseItem.Name.AutoShotgun],
            addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.AutoShotgun],
            addressableHandler.RightRecoilDataDictionary[BaseItem.Name.AutoShotgun],
            effectFactory
        );

        _weaponCreaters[BaseItem.Name.SMG] = 
            
        new StingerCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.SMG],
            addressableHandler.ItemDataDictionary[BaseItem.Name.SMG],
            addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.SMG],
            addressableHandler.RightRecoilDataDictionary[BaseItem.Name.SMG],
            effectFactory
        );

        _weaponCreaters[BaseItem.Name.DMR] = 
            
        new GuardianCreater(
            addressableHandler.ItemPrefabs[BaseItem.Name.DMR],
            addressableHandler.ItemDataDictionary[BaseItem.Name.DMR],
            addressableHandler.LeftRecoilDataDictionary[BaseItem.Name.DMR],
            effectFactory
        );
    }

    public override BaseItem Create(BaseItem.Name name)
    {
        return _weaponCreaters[name].Create();
    }
}
