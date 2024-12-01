using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stinger : VariationGun
{
    public override void ResetData(StingerData data, RecoilMapData mainMapData, RecoilRangeData subRangeData, BaseFactory effectFactory)
    {
        _equipFinishTime = data.equipFinishTime;
        _weaponWeight = data.weaponWeight;

        _maxAmmoCountInMagazine = data.maxAmmoCountInMagazine;
        _maxAmmoCountsInPossession = data.maxAmmoCountsInPossession;

        _ammoCountsInMagazine = _maxAmmoCountInMagazine;
        _ammoCountsInPossession = _maxAmmoCountsInPossession;

        _eventStorage.Add(new(EventType.Main, Conditon.ZoomIn),
            new BurstEvent(EventType.Main, data.burstFireInterval, data.burstFireCntInOneAction, OnEventStart, OnEventUpdate, OnEventEnd, OnAction));

        _eventStorage.Add(new(EventType.Main, Conditon.ZoomOut),
            new AutoEvent(EventType.Main, data.autoFireInterval, OnEventStart, OnEventUpdate, OnEventEnd, OnAction));

        _eventStorage.Add(new(EventType.Sub, Conditon.Both),
            new ManualEvent(EventType.Sub, data.zoomDelay, OnEventStart, OnEventUpdate, OnEventEnd, OnAction));


        _actionStorage.Add(new(EventType.Main, Conditon.ZoomIn),
            new SingleProjectileAttackWithWeight(_weaponName, ISoundControllable.SoundName.RifleFire, data.range, _targetLayer, data.mainFireCnt,
            data.penetratePower, data.bulletSpreadPowerDecreaseRatio, data.weightApplier, data.damageDictionary, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge));

        _actionStorage.Add(new(EventType.Main, Conditon.ZoomOut),
             new SingleProjectileAttack(_weaponName, ISoundControllable.SoundName.RifleFire, data.range, _targetLayer, data.mainFireCnt,
             data.penetratePower, data.bulletSpreadPowerDecreaseRatio, data.damageDictionary, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
             SpawnMuzzleFlashEffect, SpawnEmptyCartridge));


        _actionStorage.Add(new(EventType.Sub, Conditon.Both),
            new ZoomState(data.zoomCameraPosition.V3, data.zoomDuration, data.normalFieldOfView, data.zoomFieldOfView, OnZoomRequested));

        _recoilStorage.Add(new(EventType.Main, Conditon.ZoomOut),
            new AutoRecoilGenerator(data.autoFireInterval, data.recoveryDuration, data.recoilRatio, mainMapData));

        _recoilStorage.Add(new(EventType.Main, Conditon.ZoomIn),
            new BurstRecoilGenerator(data.burstFireInterval, data.recoveryDuration, subRangeData));

        _recoilStorage.Add(new(EventType.Sub, Conditon.Both), new NoRecoilGenerator());

        _reloadState = new MagazineReload(_weaponName, data.reloadFinishDuration, data.reloadExitDuration, data.maxAmmoCountInMagazine, _animator, OnReloadRequested, OnPlayOwnerAnimation);
        MatchState();
    }
}
