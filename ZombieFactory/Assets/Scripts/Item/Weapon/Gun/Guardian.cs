using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Guardian : VariationGun
{
    public override void ResetData(GuardianData data, RecoilRangeData recoilData, BaseFactory effectFactory)
    {
        _equipFinishTime = data.equipFinishTime;
        _weaponWeight = data.weaponWeight;

        _maxAmmoCountInMagazine = data.maxAmmoCountInMagazine;
        _maxAmmoCountsInPossession = data.maxAmmoCountsInPossession;

        _ammoCountsInMagazine = _maxAmmoCountInMagazine;
        _ammoCountsInPossession = _maxAmmoCountsInPossession;

        _eventStorage.Add(new(EventType.Main, Conditon.ZoomIn),
            new ManualEvent(EventType.Main, data.fireIntervalWhenZoomIn, OnEventStart, OnEventUpdate, OnEventEnd, OnAction));

        _eventStorage.Add(new(EventType.Main, Conditon.ZoomOut)
            , new ManualEvent(EventType.Main, data.fireIntervalWhenZoomOut, OnEventStart, OnEventUpdate, OnEventEnd, OnAction));

        _eventStorage.Add(new(EventType.Sub, Conditon.Both)
            , new ManualEvent(EventType.Sub, data.zoomDelay, OnEventStart, OnEventUpdate, OnEventEnd, OnAction));


        _actionStorage.Add(new(EventType.Main, Conditon.Both),
            new SingleProjectileAttackWithWeight(_weaponName, ISoundControllable.SoundName.DMRFire, data.range, _targetLayer, data.fireCnt,
            data.penetratePower, data.displacementSpreadMultiplyRatio, data.mainWeightApplier, data.damageDictionary, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge));

        _actionStorage.Add(new(EventType.Sub, Conditon.Both),
            new Zoom(data.cameraPositionWhenZoom.V3, data.zoomDuration, data.normalFieldOfView, data.zoomFieldOfView, OnZoomRequested));


        _recoilStorage.Add(new(EventType.Main, Conditon.ZoomIn),
            new ManualRecoilGenerator(data.fireIntervalWhenZoomIn, data.recoveryDuration, recoilData));

        _recoilStorage.Add(new(EventType.Main, Conditon.ZoomOut),
           new ManualRecoilGenerator(data.fireIntervalWhenZoomOut, data.recoveryDuration, recoilData));

        _recoilStorage.Add(new(EventType.Sub, Conditon.Both), new NoRecoilGenerator());

        _reloadStrategy = new MagazineReload(_weaponName, data.reloadFinishDuration, data.reloadExitDuration, data.maxAmmoCountInMagazine, _animator, OnReloadRequested, OnPlayOwnerAnimation);


        MatchStrategy();
    }
}
