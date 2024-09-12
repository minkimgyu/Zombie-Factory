using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ItemData { }

[System.Serializable]
public class WeaponData : ItemData
{
    public float range;
    public float equipFinishTime;
    public float weaponWeight;

    public WeaponData(float range, float equipFinishTime, float weaponWeight)
    {
        this.range = range;
        this.equipFinishTime = equipFinishTime;
        this.weaponWeight = weaponWeight;
    }
}

[System.Serializable]
public class GunData : WeaponData
{
    public int maxAmmoCountInMagazine;
    public int maxAmmoCountsInPossession;

    public float reloadFinishDuration;
    public float reloadExitDuration;

    public GunData(float range, float equipFinishTime, float weaponWeight, 
        int maxAmmoCountInMagazine, int maxAmmoCountsInPossession, float reloadFinishDuration, float reloadExitDuration) 
        : base(range, equipFinishTime, weaponWeight)
    {
        this.maxAmmoCountInMagazine = maxAmmoCountInMagazine;
        this.maxAmmoCountsInPossession = maxAmmoCountsInPossession;
        this.reloadFinishDuration = reloadFinishDuration;
        this.reloadExitDuration = reloadExitDuration;
    }
}

[System.Serializable]
public class AutomaticGunData : GunData
{
    public float fireIntervalWhenZoomIn;
    public float fireIntervalWhenZoomOut;

    public int fireCnt;
    public float penetratePower;

    public float recoveryDuration;

    public float recoilRatioWhenZoomIn;
    public float recoilRatioWhenZoomOut;

    public float displacementSpreadMultiplyRatio;
    public float zoomDuration;
    public float zoomDelay;
    public float normalFieldOfView;
    public float zoomFieldOfView;

    public SerializableVector3 cameraPositionWhenZoom;
    public Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary;

    public AutomaticGunData(
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
        float recoilRatioWhenZoomIn,
        float recoilRatioWhenZoomOut,
        float displacementSpreadMultiplyRatio,
        float zoomDuration,
        float zoomDelay,
        float normalFieldOfView,
        float zoomFieldOfView,
        SerializableVector3 cameraPositionWhenZoom,
        Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary) : base(range, equipFinishTime, weaponWeight, maxAmmoCountInMagazine, maxAmmoCountsInPossession, reloadFinishDuration, reloadExitDuration)
    {
        this.fireIntervalWhenZoomIn = fireIntervalWhenZoomIn;
        this.fireIntervalWhenZoomOut = fireIntervalWhenZoomOut;
        this.fireCnt = fireCnt;
        this.penetratePower = penetratePower;
        this.recoveryDuration = recoveryDuration;
        this.recoilRatioWhenZoomIn = recoilRatioWhenZoomIn;
        this.recoilRatioWhenZoomOut = recoilRatioWhenZoomOut;
        this.displacementSpreadMultiplyRatio = displacementSpreadMultiplyRatio;
        this.zoomDuration = zoomDuration;
        this.zoomDelay = zoomDelay;
        this.normalFieldOfView = normalFieldOfView;
        this.zoomFieldOfView = zoomFieldOfView;
        this.cameraPositionWhenZoom = cameraPositionWhenZoom;
        this.damageDictionary = damageDictionary;
    }
}

public class AutomaticGunCreater : GunCreater
{
    BaseRecoilData _recoilData;

    public AutomaticGunCreater(BaseItem prefab, ItemData data, BaseRecoilData recoilData, BaseFactory effectFactory)
        :base(prefab, data, effectFactory)
    {
        _recoilData = recoilData;
    }

    public override BaseItem Create()
    {
        BaseItem weapon = UnityEngine.Object.Instantiate(_prefab);

        AutomaticGunData data = _data as AutomaticGunData;
        RecoilMapData recoilData = _recoilData as RecoilMapData;

        weapon.Initialize();
        weapon.ResetData(data, recoilData, _effectFactory);
        return weapon;
    }
}
