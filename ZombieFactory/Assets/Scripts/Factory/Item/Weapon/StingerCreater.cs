using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class StingerData : GunData
{
    public float autoFireInterval;
    public float burstFireInterval;

    public float penetratePower;
    public float recoveryDuration;

    public float zoomDelay;
    public int burstFireCntInOneAction;
    public int mainFireCnt;

    public float burstAttackRecoverDuration;
    public float bulletSpreadPowerDecreaseRatio;
    public float recoilRatio;

    public float subActionDelay;
    public float zoomDuration;


    public float normalFieldOfView;
    public float zoomFieldOfView;
    public SerializableVector3 zoomCameraPosition;

    public WeightApplier weightApplier;

    public Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary;

    public StingerData(
        float range,
        float equipFinishTime,
        float weaponWeight,
        int maxAmmoCountInMagazine,
        int maxAmmoCountsInPossession,
        float reloadFinishDuration,
        float reloadExitDuration,

        float autoFireInterval,
        float burstFireInterval,
        float penetratePower,
        float recoveryDuration,
        float zoomDelay,
        int burstFireCntInOneAction,
        int mainFireCnt,
        float burstAttackRecoverDuration,
        float bulletSpreadPowerDecreaseRatio,
        float recoilRatio,
        float subActionDelay,
        float zoomDuration,
        float normalFieldOfView,
        float zoomFieldOfView,
        SerializableVector3 zoomCameraPosition,
        WeightApplier weightApplier,
        Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary) : base(range, equipFinishTime, weaponWeight, maxAmmoCountInMagazine, maxAmmoCountsInPossession, reloadFinishDuration, reloadExitDuration)
    {
        this.autoFireInterval = autoFireInterval;
        this.burstFireInterval = burstFireInterval;
        this.penetratePower = penetratePower;
        this.recoveryDuration = recoveryDuration;
        this.zoomDelay = zoomDelay;
        this.burstFireCntInOneAction = burstFireCntInOneAction;
        this.mainFireCnt = mainFireCnt;
        this.burstAttackRecoverDuration = burstAttackRecoverDuration;
        this.bulletSpreadPowerDecreaseRatio = bulletSpreadPowerDecreaseRatio;
        this.recoilRatio = recoilRatio;
        this.subActionDelay = subActionDelay;
        this.zoomDuration = zoomDuration;
        this.normalFieldOfView = normalFieldOfView;
        this.zoomFieldOfView = zoomFieldOfView;
        this.zoomCameraPosition = zoomCameraPosition;
        this.weightApplier = weightApplier;
        this.damageDictionary = damageDictionary;
    }
}

public class StingerCreater : GunCreater
{
    BaseRecoilData _mainRecoilData;
    BaseRecoilData _subRecoilData;

    public StingerCreater(BaseItem prefab, ItemData data, BaseRecoilData mainMapData, BaseRecoilData subRangeData, BaseFactory effectFactory) 
        : base(prefab, data, effectFactory)
    {
        _mainRecoilData = mainMapData;
        _subRecoilData = subRangeData;
    }

    public override BaseItem Create()
    {
        BaseItem weapon = UnityEngine.Object.Instantiate(_prefab);
        weapon.Initialize();

        StingerData data = _data as StingerData;
        RecoilMapData mainRecoilData = _mainRecoilData as RecoilMapData;
        RecoilRangeData subRecoilData = _subRecoilData as RecoilRangeData;

        weapon.ResetData(data, mainRecoilData, subRecoilData, _effectFactory);
        return weapon;
    }
}
