using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : Gun
{
    public override void ResetData(JudgeData data, RecoilRangeData mainRangeData, BaseFactory effectFactory)
    {
        _equipFinishTime = data.equipFinishTime;
        _weaponWeight = data.weaponWeight;

        _maxAmmoCountInMagazine = data.maxAmmoCountInMagazine;
        _maxAmmoCountsInPossession = data.maxAmmoCountsInPossession;

        _ammoCountsInMagazine = _maxAmmoCountInMagazine;
        _ammoCountsInPossession = _maxAmmoCountsInPossession;

        //여기에 Action 연결해서 총알이 소모되는 부분을 구현해보자
        _eventStates[EventType.Main] = new ManualEvent(EventType.Main, data.mainFireInterval, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);
        _eventStates[EventType.Sub] = new ManualEvent(EventType.Sub, data.subFireInterval, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);


        _actionStates[EventType.Main] = new ScatterProjectileAttack(_weaponName, ISoundControllable.SoundName.ShotgunFire, data.range, _targetLayer, data.mainFireCnt,
            data.penetratePower, data.bulletSpreadPowerDecreaseRatio, data.pelletCount, data.spreadOffset, data.damageDictionary, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge);

        // 무기를 버릴 경우, 제거해야함
        _actionStates[EventType.Sub] = new NoAction();

        _recoilStates[EventType.Main] = new ManualRecoilGenerator(data.mainFireInterval, data.recoveryDuration, mainRangeData);
        _recoilStates[EventType.Sub] = new NoRecoilGenerator();

        _reloadState = new MagazineReload(_weaponName, data.reloadFinishDuration, data.reloadExitDuration, data.maxAmmoCountInMagazine, _animator, OnReloadRequested, OnPlayOwnerAnimation);
    }
}
