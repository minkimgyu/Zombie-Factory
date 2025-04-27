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

        //���⿡ Action �����ؼ� �Ѿ��� �Ҹ�Ǵ� �κ��� �����غ���
        _eventStrategy[EventType.Main] = new ManualEvent(EventType.Main, data.mainFireInterval, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);
        _eventStrategy[EventType.Sub] = new ManualEvent(EventType.Sub, data.subFireInterval, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);


        _actionStrategy[EventType.Main] = new ScatterProjectileAttack(_weaponName, ISoundControllable.SoundName.ShotgunFire, data.range, _targetLayer, data.mainFireCnt,
            data.penetratePower, data.bulletSpreadPowerDecreaseRatio, data.pelletCount, data.spreadOffset, data.damageDictionary, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge);

        // ���⸦ ���� ���, �����ؾ���
        _actionStrategy[EventType.Sub] = new NoAction();

        _recoilStrategy[EventType.Main] = new ManualRecoilGenerator(data.mainFireInterval, data.recoveryDuration, mainRangeData);
        _recoilStrategy[EventType.Sub] = new NoRecoilGenerator();

        _reloadStrategy = new MagazineReload(_weaponName, data.reloadFinishDuration, data.reloadExitDuration, data.maxAmmoCountInMagazine, _animator, OnReloadRequested, OnPlayOwnerAnimation);
    }
}
