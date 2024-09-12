using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BatData : WeaponData
{
    public float delayBetweenAttack;
    public float damageDelayTime;

    public DirectionData attackDamageData;

    public BatData(
        float range,
        float equipFinishTime,
        float weaponWeight,

        float delayBetweenAttack,
        float damageDelayTime,
        DirectionData attackDamageData) : base(range, equipFinishTime, weaponWeight)
    {
        this.delayBetweenAttack = delayBetweenAttack;
        this.damageDelayTime = damageDelayTime;
        this.attackDamageData = attackDamageData;
    }
}

public class BatCreater : ItemCreater
{
    public BatCreater(BaseItem prefab, ItemData data) : base(prefab, data)
    {
    }

    public override BaseItem Create()
    {
        BaseItem weapon = UnityEngine.Object.Instantiate(_prefab);
        weapon.Initialize();

        BatData data = _data as BatData;
        weapon.ResetData(data);
        return weapon;
    }
}
