using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomCreater : AutomaticGunCreater
{
    public PhantomCreater(BaseWeapon prefab, WeaponData data, RecoilMapData mapData, BaseFactory effectFactory) : base(prefab, data, mapData, effectFactory)
    {
    }
}