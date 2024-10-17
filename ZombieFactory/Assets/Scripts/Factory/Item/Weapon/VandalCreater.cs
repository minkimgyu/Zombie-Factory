using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VandalCreater : AutomaticGunCreater
{
    public VandalCreater(BaseWeapon prefab, WeaponData data, RecoilMapData mapData, BaseFactory effectFactory) : base(prefab, data, mapData, effectFactory)
    {
    }
}