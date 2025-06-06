using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuckyData : GunData
{
    public float mainFireInterval;
    public float subFireInterval;


    public int mainFireCnt;
    public int subFireCnt;

    public float recoveryDuration;

    public float penetratePower;

    public int pelletCount;
    public float spreadOffset;



    public float bulletSpreadPowerDecreaseRatio;
    public float reloadBeforeDuration;
    public float frontDistance;



    public string explosionEffectName;
    public int subActionPelletCount;
    public float subScatterActionBulletSpreadPowerDecreaseRatio;
    public float subSingleActionBulletSpreadPowerDecreaseRatio;
    public float subActionSpreadOffset;
    public float subActionSinglePenetratePower;
    public float subActionScatterPenetratePower;

    public float findRange;
    public Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary;
    public Dictionary<IHitable.Area, DistanceAreaData[]> subSingleAttackDamageDictionary;

    public BuckyData(
        float range,
        float equipFinishTime,
        float weaponWeight,
        int maxAmmoCountInMagazine,
        int maxAmmoCountsInPossession,
        float reloadFinishDuration,
        float reloadExitDuration,

        float mainFireInterval,
        float subFireInterval,
        int mainFireCnt,
        int subFireCnt,
        float recoveryDuration,
        float penetratePower,
        int pelletCount,
        float spreadOffset,
        float bulletSpreadPowerDecreaseRatio,
        float reloadBeforeDuration,
        float frontDistance,
        string explosionEffectName,
        int subActionPelletCount,
        float subScatterActionBulletSpreadPowerDecreaseRatio,
        float subSingleActionBulletSpreadPowerDecreaseRatio,
        float subActionSpreadOffset,
        float subActionSinglePenetratePower,
        float subActionScatterPenetratePower,
        float findRange,
        Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary,
        Dictionary<IHitable.Area, DistanceAreaData[]> subSingleAttackDamageDictionary) : base(range, equipFinishTime, weaponWeight, maxAmmoCountInMagazine, maxAmmoCountsInPossession, reloadFinishDuration, reloadExitDuration)
    {
        this.mainFireInterval = mainFireInterval;
        this.subFireInterval = subFireInterval;
        this.mainFireCnt = mainFireCnt;
        this.subFireCnt = subFireCnt;
        this.recoveryDuration = recoveryDuration;
        this.penetratePower = penetratePower;
        this.pelletCount = pelletCount;
        this.spreadOffset = spreadOffset;
        this.bulletSpreadPowerDecreaseRatio = bulletSpreadPowerDecreaseRatio;
        this.reloadBeforeDuration = reloadBeforeDuration;
        this.frontDistance = frontDistance;
        this.explosionEffectName = explosionEffectName;
        this.subActionPelletCount = subActionPelletCount;
        this.subScatterActionBulletSpreadPowerDecreaseRatio = subScatterActionBulletSpreadPowerDecreaseRatio;
        this.subSingleActionBulletSpreadPowerDecreaseRatio = subSingleActionBulletSpreadPowerDecreaseRatio;
        this.subActionSpreadOffset = subActionSpreadOffset;
        this.subActionSinglePenetratePower = subActionSinglePenetratePower;
        this.subActionScatterPenetratePower = subActionScatterPenetratePower;
        this.findRange = findRange;
        this.damageDictionary = damageDictionary;
        this.subSingleAttackDamageDictionary = subSingleAttackDamageDictionary;
    }
}

public class BuckyCreater : GunCreater
{
    RecoilRangeData _mainRecoilData;
    RecoilRangeData _subRecoilData;

    public BuckyCreater(BaseItem prefab, ItemData data, BaseRecoilData mainRecoilData, BaseRecoilData subRecoilData, BaseFactory effectFactory) 
        : base(prefab, data, effectFactory)
    {
        _mainRecoilData = mainRecoilData as RecoilRangeData;
        _subRecoilData = subRecoilData as RecoilRangeData;
    }

    public override BaseItem Create()
    {
        BaseItem weapon = UnityEngine.Object.Instantiate(_prefab);
        BuckyData data = _data as BuckyData;

        weapon.Initialize();
        weapon.ResetData(data, _mainRecoilData, _subRecoilData, _effectFactory);
        return weapon;
    }
}
