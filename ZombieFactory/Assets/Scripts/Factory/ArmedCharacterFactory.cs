using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArmedCharacterFactory : BaseFactory
{
    BaseFactory _itemFactory;
    BaseFactory _effectFactory;
    BaseFactory _lifeFactory;

    public ArmedCharacterFactory(BaseFactory weaponFactory, BaseFactory effectFactory, BaseFactory lifeFactory)
    {
        _effectFactory = effectFactory;
        _itemFactory = weaponFactory;
        _lifeFactory = lifeFactory;
    }

    public override BaseLife Create(BaseLife.Name name, List<BaseItem.Name> weaponNames)
    {
        BaseLife life = _lifeFactory.Create(name);

        for (int i = 0; i < weaponNames.Count; i++)
        {
            BaseWeapon weapon = _itemFactory.Create(weaponNames[i]) as BaseWeapon;
            life.AddWeapon(weapon);
        }

        return life;
    }
}
