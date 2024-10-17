using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GuardianData : GunData
{
    public float fireIntervalWhenZoomIn;
    public float fireIntervalWhenZoomOut;

    public int fireCnt;

    public float penetratePower;
    public float recoveryDuration;
    public float zoomDelay;

    public float zoomDuration;
    public float normalFieldOfView;
    public float zoomFieldOfView;

    public WeightApplier mainWeightApplier;

    public float displacementSpreadMultiplyRatio;

    public SerializableVector3 cameraPositionWhenZoom;
    public Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary;

    public GuardianData(
        float range, 
        float equipFinishTime, 
        float weaponWeight, 
        int maxAmmoCountInMagazine, 
        int maxAmmoCountsInPossession, 
        float reloadFinishDuration, 
        float reloadExitDuration,

        float fireIntervalWhenZoomIn,
        float fireIntervalWhenZoomOut,
        int fireCnt,
        float penetratePower,
        float recoveryDuration,
        float zoomDelay,
        float zoomDuration,
        float normalFieldOfView,
        float zoomFieldOfView,
        WeightApplier mainWeightApplier,
        float displacementSpreadMultiplyRatio,
        SerializableVector3 cameraPositionWhenZoom,
        Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary) : base(range, equipFinishTime, weaponWeight, maxAmmoCountInMagazine, maxAmmoCountsInPossession, reloadFinishDuration, reloadExitDuration)
    {
        this.fireIntervalWhenZoomIn = fireIntervalWhenZoomIn;
        this.fireIntervalWhenZoomOut = fireIntervalWhenZoomOut;
        this.fireCnt = fireCnt;
        this.penetratePower = penetratePower;
        this.recoveryDuration = recoveryDuration;
        this.zoomDelay = zoomDelay;
        this.zoomDuration = zoomDuration;
        this.normalFieldOfView = normalFieldOfView;
        this.zoomFieldOfView = zoomFieldOfView;
        this.mainWeightApplier = mainWeightApplier;
        this.displacementSpreadMultiplyRatio = displacementSpreadMultiplyRatio;
        this.cameraPositionWhenZoom = cameraPositionWhenZoom;
        this.damageDictionary = damageDictionary;
    }
}

public class GuardianCreater : GunCreater
{
    BaseRecoilData _mainRecoilData;

    public GuardianCreater(BaseItem prefab, ItemData data, BaseRecoilData mainRecoilData, BaseFactory effectFactory) : base(prefab, data, effectFactory)
    {
        _mainRecoilData = mainRecoilData;
    }

    public override BaseItem Create()
    {
        BaseItem weapon = UnityEngine.Object.Instantiate(_prefab);
        weapon.Initialize();

        GuardianData data = _data as GuardianData;
        RecoilRangeData mainRecoilData = _mainRecoilData as RecoilRangeData;

        weapon.ResetData(data, mainRecoilData, _effectFactory);
        return weapon;
    }
}
