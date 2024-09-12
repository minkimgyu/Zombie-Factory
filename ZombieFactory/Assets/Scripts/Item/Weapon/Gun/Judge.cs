using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : Gun
{
    public override void ResetData(JudgeData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory)
    {
        _maxAmmoCountInMagazine = data.maxAmmoCountInMagazine;
        _maxAmmoCountsInPossession = data.maxAmmoCountsInPossession;

        _ammoCountsInMagazine = _maxAmmoCountInMagazine;
        _ammoCountsInPossession = _maxAmmoCountsInPossession;

        //���⿡ Action �����ؼ� �Ѿ��� �Ҹ�Ǵ� �κ��� �����غ���
        _eventStrategies[EventType.Main] = new ManualEvent(EventType.Main, data.mainFireInterval, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);
        _eventStrategies[EventType.Sub] = new ManualEvent(EventType.Sub, data.subFireInterval, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);


        _actionStrategies[EventType.Main] = new ScatterProjectileAttack(_weaponName, data.range, _targetLayer, data.mainFireCnt,
            data.penetratePower, data.bulletSpreadPowerDecreaseRatio, data.pelletCount, data.spreadOffset, data.damageDictionary, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge);

        // ���⸦ ���� ���, �����ؾ���
        _actionStrategies[EventType.Sub] = new SingleAndExplosionScatterAttackCombination(

            _weaponName, data.range, _targetLayer, data.mainFireCnt, data.subActionSinglePenetratePower, data.subScatterActionBulletSpreadPowerDecreaseRatio, data.subFireCnt,
            data.subActionScatterPenetratePower, data.subSingleActionBulletSpreadPowerDecreaseRatio, data.pelletCount, data.spreadOffset, data.frontDistance, data.explosionEffectName, data.damageDictionary,
            data.subSingleAttackDamageDictionary, data.findRange, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge);

        _recoilStrategies[EventType.Main] = new ManualRecoilGenerator(data.mainFireInterval, data.recoveryDuration, mainRangeData);
        _recoilStrategies[EventType.Sub] = new ManualRecoilGenerator(data.subFireInterval, data.recoveryDuration, subRangeData);

        _equipFinishTime = data.equipFinishTime;
        _reloadDuration = data.reloadFinishDuration;
        _reloadExitDuration = data.reloadExitDuration;
    }
}
