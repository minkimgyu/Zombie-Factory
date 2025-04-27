using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classic : Gun
{
    public override void ResetData(ClassicData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory)
    {
        _equipFinishTime = data.equipFinishTime;
        _weaponWeight = data.weaponWeight;

        _maxAmmoCountInMagazine = data.maxAmmoCountInMagazine;
        _maxAmmoCountsInPossession = data.maxAmmoCountsInPossession;

        _ammoCountsInMagazine = _maxAmmoCountInMagazine;
        _ammoCountsInPossession = _maxAmmoCountsInPossession;

        _eventStrategy[EventType.Main] = new ManualEvent(EventType.Main, data.mainShootInterval, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);
        _eventStrategy[EventType.Sub] = new ManualEvent(EventType.Sub, data.subShootInterval, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);

        

        _actionStrategy[EventType.Main] = new SingleProjectileAttackWithWeight(_weaponName, ISoundControllable.SoundName.PistolFire, data.range, _targetLayer, data.mainFireCnt,
            data.penetratePower, data.displacementSpreadMultiplyRatio, data.mainWeightApplier, data.damageDictionary, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge);


        _actionStrategy[EventType.Sub] = new ScatterProjectileAttackWithWeight(_weaponName, ISoundControllable.SoundName.PistolFire, data.range, _targetLayer, data.subFireCnt,
            data.penetratePower, data.displacementSpreadMultiplyRatio, data.subFireCnt, data.subActionSpreadOffset, data.subWeightApplier, data.damageDictionary,
            _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount, SpawnMuzzleFlashEffect, SpawnEmptyCartridge);

        _recoilStrategy[EventType.Main] = new ManualRecoilGenerator(data.mainShootInterval, data.recoveryDuration, mainRangeData);
        _recoilStrategy[EventType.Sub] = new ManualRecoilGenerator(data.subShootInterval, data.recoveryDuration, subRangeData);


        _reloadStrategy = new MagazineReload(_weaponName, data.reloadFinishDuration, data.reloadExitDuration, data.maxAmmoCountInMagazine, _animator, OnReloadRequested, OnPlayOwnerAnimation);
    }
}