using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OperatorData : GunData
{
    public float mainActionDelayWhenZoomIn;

    public float mainActionDelayWhenZoomOut;

    public int fireCnt;

    public float penetratePower;

    public float recoveryDuration;



    public float subActionDelay;


    public float mainActionbulletSpreadPowerRatio;



    public float meshDisableDelay;


    public float zoomDuration;


    public float normalFieldOfView;


    public float zoomFieldOfView;


    public float doubleZoomFieldOfView;


    public SerializableVector3 zoomCameraPosition;


    public WeightApplier mainWeightApplier;

    public Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary;

    public OperatorData(
        float range,
        float equipFinishTime,
        float weaponWeight,
        int maxAmmoCountInMagazine,
        int maxAmmoCountsInPossession,
        float reloadFinishDuration,
        float reloadExitDuration,

        float mainActionDelayWhenZoomIn,
        float mainActionDelayWhenZoomOut,
        int fireCnt,
        float penetratePower,
        float recoveryDuration,
        float subActionDelay,
        float mainActionbulletSpreadPowerRatio,
        float meshDisableDelay,
        float zoomDuration,
        float normalFieldOfView,
        float zoomFieldOfView,
        float doubleZoomFieldOfView,
        SerializableVector3 zoomCameraPosition,
        WeightApplier mainWeightApplier,
        Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary) : base(range, equipFinishTime, weaponWeight, maxAmmoCountInMagazine, maxAmmoCountsInPossession, reloadFinishDuration, reloadExitDuration)
    {
        this.mainActionDelayWhenZoomIn = mainActionDelayWhenZoomIn;
        this.mainActionDelayWhenZoomOut = mainActionDelayWhenZoomOut;
        this.fireCnt = fireCnt;
        this.penetratePower = penetratePower;
        this.recoveryDuration = recoveryDuration;
        this.subActionDelay = subActionDelay;
        this.mainActionbulletSpreadPowerRatio = mainActionbulletSpreadPowerRatio;
        this.meshDisableDelay = meshDisableDelay;
        this.zoomDuration = zoomDuration;
        this.normalFieldOfView = normalFieldOfView;
        this.zoomFieldOfView = zoomFieldOfView;
        this.doubleZoomFieldOfView = doubleZoomFieldOfView;
        this.zoomCameraPosition = zoomCameraPosition;
        this.mainWeightApplier = mainWeightApplier;
        this.damageDictionary = damageDictionary;
    }
}

public class OperatorCreater : GunCreater
{
    RecoilRangeData _mainRecoilData;
    RecoilRangeData _subRecoilData;

    public OperatorCreater(BaseItem prefab, ItemData data, BaseRecoilData mainRecoilData, BaseRecoilData subRecoilData, BaseFactory effectFactory) 
        : base(prefab, data, effectFactory)
    {
        _mainRecoilData = mainRecoilData as RecoilRangeData;
        _subRecoilData = subRecoilData as RecoilRangeData;
    }

    public override BaseItem Create()
    {
        BaseItem weapon = UnityEngine.Object.Instantiate(_prefab);
        OperatorData data = _data as OperatorData;

        weapon.Initialize();
        weapon.ResetData(data, _mainRecoilData, _subRecoilData, _effectFactory);
        return weapon;
    }
}
