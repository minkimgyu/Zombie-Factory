using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Operator : VariationGun
{
    public override void ResetData(OperatorData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory)
    {
        _equipFinishTime = data.equipFinishTime;
        _weaponWeight = data.weaponWeight;

        _maxAmmoCountInMagazine = data.maxAmmoCountInMagazine;
        _maxAmmoCountsInPossession = data.maxAmmoCountsInPossession;

        _ammoCountsInMagazine = _maxAmmoCountInMagazine;
        _ammoCountsInPossession = _maxAmmoCountsInPossession;

        _eventStorage.Add(new(EventType.Main, Conditon.ZoomIn),
            new ManualEvent(EventType.Main, data.mainActionDelayWhenZoomIn, OnEventStart, OnEventUpdate, OnEventEnd, OnAction));

        _eventStorage.Add(new(EventType.Main, Conditon.ZoomOut),
            new ManualEvent(EventType.Main, data.mainActionDelayWhenZoomOut, OnEventStart, OnEventUpdate, OnEventEnd, OnAction));

        _eventStorage.Add(new(EventType.Sub, Conditon.Both),
            new ManualEvent(EventType.Sub, data.subActionDelay, OnEventStart, OnEventUpdate, OnEventEnd, OnAction));


        _actionStorage.Add(new(EventType.Main, Conditon.ZoomIn),
            new SingleProjectileAttack(_weaponName, ISoundControllable.SoundName.SniperFire, data.range, _targetLayer, data.fireCnt,
            data.penetratePower, data.mainActionbulletSpreadPowerRatio, data.damageDictionary, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge));

        _actionStorage.Add(new(EventType.Main, Conditon.ZoomOut),
            new SingleProjectileAttackWithWeight(_weaponName, ISoundControllable.SoundName.SniperFire, data.range, _targetLayer, data.fireCnt,
            data.penetratePower, data.mainActionbulletSpreadPowerRatio, data.mainWeightApplier, data.damageDictionary, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge));

        _actionStorage.Add(new(EventType.Sub, Conditon.Both),
            new DoubleZoomState(data.zoomCameraPosition.V3, data.zoomDuration, data.normalFieldOfView, data.zoomFieldOfView, data.doubleZoomFieldOfView, OnZoomRequested));

        _recoilStorage.Add(new(EventType.Main, Conditon.ZoomIn),
            new ManualRecoilGenerator(data.mainActionDelayWhenZoomIn, data.recoveryDuration, mainRangeData));

        _recoilStorage.Add(new(EventType.Main, Conditon.ZoomOut),
            new ManualRecoilGenerator(data.mainActionDelayWhenZoomOut, data.recoveryDuration, subRangeData));

        _recoilStorage.Add(new(EventType.Sub, Conditon.Both), new NoRecoilGenerator());

        _reloadState = new MagazineReload(_weaponName, data.reloadFinishDuration, data.reloadExitDuration, data.maxAmmoCountInMagazine, _animator, OnReloadRequested, OnPlayOwnerAnimation);
        MatchState();
    }
}
