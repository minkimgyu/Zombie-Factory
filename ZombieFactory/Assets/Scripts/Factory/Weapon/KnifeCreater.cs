using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KnifeData : WeaponData
{
    public float mainAttackDelay;

    public float subAttackDelay;

    public float delayWhenOtherSideAttack;

    public float attackLinkDuration;

    public int mainAnimationCount;

    public float mainAttackEffectDelayTime;

    public float subAttackEffectDelayTime;

    public DirectionData mainAttackDamageData;

    public DirectionData subAttackDamageData;

    public KnifeData(
        float range,
        float equipFinishTime,
        float weaponWeight,

        float mainAttackDelay,
        float subAttackDelay,
        float delayWhenOtherSideAttack,
        float attackLinkDuration,
        int mainAnimationCount,
        float mainAttackEffectDelayTime,
        float subAttackEffectDelayTime,
        DirectionData mainAttackDamageData,
        DirectionData subAttackDamageData) : base(range, equipFinishTime, weaponWeight)
    {
        this.mainAttackDelay = mainAttackDelay;
        this.subAttackDelay = subAttackDelay;
        this.delayWhenOtherSideAttack = delayWhenOtherSideAttack;
        this.attackLinkDuration = attackLinkDuration;
        this.mainAnimationCount = mainAnimationCount;
        this.mainAttackEffectDelayTime = mainAttackEffectDelayTime;
        this.subAttackEffectDelayTime = subAttackEffectDelayTime;
        this.mainAttackDamageData = mainAttackDamageData;
        this.subAttackDamageData = subAttackDamageData;
    }
}

public class KnifeCreater : GunCreater
{
    public KnifeCreater(BaseItem prefab, ItemData data, BaseFactory effectFactory) : base(prefab, data, effectFactory)
    {
    }

    public override BaseItem Create()
    {
        BaseItem weapon = UnityEngine.Object.Instantiate(_prefab);
        KnifeData data = _data as KnifeData;

        weapon.Initialize();
        weapon.ResetData(data, _effectFactory);
        return weapon;
    }
}
