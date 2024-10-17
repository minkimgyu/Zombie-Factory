using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AidPackData : ItemData
{
    public float healPoint;

    public AidPackData(float healPoint)
    {
        this.healPoint = healPoint;
    }
}

public class AidPackCreater : ItemCreater
{
    public AidPackCreater(BaseItem prefab, AidPackData data) : base(prefab, data)
    {
    }

    public override BaseItem Create()
    {
        BaseItem item = UnityEngine.Object.Instantiate(_prefab);
        item.ResetData(_data as AidPackData);
        return item;
    }
}