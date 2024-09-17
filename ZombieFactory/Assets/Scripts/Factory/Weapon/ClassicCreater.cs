using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClassicData : GunData
{
    public float mainShootInterval;
    public float subShootInterval;

    public int mainFireCnt;
    public int subFireCnt;

    public float recoveryDuration;

    public float penetratePower;

    public int subActionPelletCount;
    public float subActionSpreadOffset;

    public WeightApplier mainWeightApplier;
    public WeightApplier subWeightApplier;

    public float displacementSpreadMultiplyRatio;
    public int subAttackBulletCounts;
    public float spreadOffset;
    public float mainActionDelay;
    public float subActionDelay;

    public float bulletSpreadPowerRatio;
    public float bulletSpreadPowerDecreaseRatio;

    public Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary;

    public ClassicData(
        float range,
        float equipFinishTime,
        float weaponWeight,
        int maxAmmoCountInMagazine,
        int maxAmmoCountsInPossession,
        float reloadFinishDuration,
        float reloadExitDuration,

        float mainShootInterval,
        float subShootInterval,
        int mainFireCnt,
        int subFireCnt,
        float recoveryDuration,
        float penetratePower,
        int subActionPelletCount,
        float subActionSpreadOffset,
        WeightApplier mainWeightApplier,
        WeightApplier subWeightApplier,
        float displacementSpreadMultiplyRatio,
        int subAttackBulletCounts,
        float spreadOffset,
        float mainActionDelay,
        float subActionDelay,
        float bulletSpreadPowerRatio,
        float bulletSpreadPowerDecreaseRatio,
        Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary) : base(range, equipFinishTime, weaponWeight, maxAmmoCountInMagazine, maxAmmoCountsInPossession, reloadFinishDuration, reloadExitDuration)
    {
        this.mainShootInterval = mainShootInterval;
        this.subShootInterval = subShootInterval;
        this.mainFireCnt = mainFireCnt;
        this.subFireCnt = subFireCnt;
        this.recoveryDuration = recoveryDuration;
        this.penetratePower = penetratePower;
        this.subActionPelletCount = subActionPelletCount;
        this.subActionSpreadOffset = subActionSpreadOffset;
        this.mainWeightApplier = mainWeightApplier;
        this.subWeightApplier = subWeightApplier;
        this.displacementSpreadMultiplyRatio = displacementSpreadMultiplyRatio;
        this.subAttackBulletCounts = subAttackBulletCounts;
        this.spreadOffset = spreadOffset;
        this.mainActionDelay = mainActionDelay;
        this.subActionDelay = subActionDelay;
        this.bulletSpreadPowerRatio = bulletSpreadPowerRatio;
        this.bulletSpreadPowerDecreaseRatio = bulletSpreadPowerDecreaseRatio;
        this.damageDictionary = damageDictionary;
    }
}

public class ClassicCreater : GunCreater
{
    RecoilRangeData _mainRecoilData;
    RecoilRangeData _subRecoilData;

    public ClassicCreater(BaseItem prefab, ItemData data, BaseRecoilData mainRecoilData, BaseRecoilData subRecoilData, BaseFactory effectFactory) 
        : base(prefab, data, effectFactory)
    {
        _mainRecoilData = mainRecoilData as RecoilRangeData;
        _subRecoilData = subRecoilData as RecoilRangeData;
    }

    public override BaseItem Create()
    {
        BaseItem weapon = UnityEngine.Object.Instantiate(_prefab);
        ClassicData data = _data as ClassicData;

        weapon.Initialize();
        weapon.ResetData(data, _mainRecoilData, _subRecoilData, _effectFactory);
        return weapon;
    }
}
