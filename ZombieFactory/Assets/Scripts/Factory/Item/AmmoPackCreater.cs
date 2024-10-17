using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoPackData : ItemData
{
    public int ammoCount;

    public AmmoPackData(int ammoCount)
    {
        this.ammoCount = ammoCount;
    }
}

public class AmmoPackCreater : ItemCreater
{
    public AmmoPackCreater(BaseItem prefab, AmmoPackData data) : base(prefab, data)
    {
    }

    public override BaseItem Create()
    {
        BaseItem item = UnityEngine.Object.Instantiate(_prefab);
        item.ResetData(_data as AmmoPackData);
        return item;
    }
}