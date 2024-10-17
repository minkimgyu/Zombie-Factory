using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class GunCreater : ItemCreater
{
    protected BaseFactory _effectFactory;
    public GunCreater(BaseItem prefab, ItemData data, BaseFactory effectFactory) : base(prefab, data)
    {
        _effectFactory = effectFactory;
    }
}
