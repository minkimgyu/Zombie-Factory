using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucky : Gun
{
    public void PlayPushAmmoSound()
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundControllable.SoundName.PushAmmo);
    }

    public override void ResetData(BuckyData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory)
    {
        _equipFinishTime = data.equipFinishTime;
        _weaponWeight = data.weaponWeight;

        _maxAmmoCountInMagazine = data.maxAmmoCountInMagazine;
        _maxAmmoCountsInPossession = data.maxAmmoCountsInPossession;

        _ammoCountsInMagazine = _maxAmmoCountInMagazine;
        _ammoCountsInPossession = _maxAmmoCountsInPossession;

        //여기에 Action 연결해서 총알이 소모되는 부분을 구현해보자
        _eventStrategy[EventType.Main] = new ManualEvent(EventType.Main, data.mainFireInterval, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);
        _eventStrategy[EventType.Sub] = new ManualEvent(EventType.Sub, data.subFireInterval, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);


        _actionStrategy[EventType.Main] = new ScatterProjectileAttack(_weaponName, ISoundControllable.SoundName.ShotgunFire, data.range, _targetLayer, data.mainFireCnt,
            data.penetratePower, data.bulletSpreadPowerDecreaseRatio, data.pelletCount, data.spreadOffset, data.damageDictionary, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge);

        // 무기를 버릴 경우, 제거해야함
        _actionStrategy[EventType.Sub] = new SingleAndExplosionScatterAttackCombination(

            _weaponName, ISoundControllable.SoundName.ShotgunDotFire, ISoundControllable.SoundName.ShotgunExplosionFire, data.range, _targetLayer, data.mainFireCnt, data.subActionSinglePenetratePower, data.subScatterActionBulletSpreadPowerDecreaseRatio, data.subFireCnt,
            data.subActionScatterPenetratePower, data.subSingleActionBulletSpreadPowerDecreaseRatio, data.pelletCount, data.spreadOffset, data.frontDistance, data.explosionEffectName, data.damageDictionary,
            data.subSingleAttackDamageDictionary, data.findRange, _animator, effectFactory, ReturnMuzzlePos, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge);

        _recoilStrategy[EventType.Main] = new ManualRecoilGenerator(data.mainFireInterval, data.recoveryDuration, mainRangeData);
        _recoilStrategy[EventType.Sub] = new ManualRecoilGenerator(data.subFireInterval, data.recoveryDuration, subRangeData);

        _reloadStrategy = new RoundByRoundReload(_weaponName, data.reloadBeforeDuration, data.reloadFinishDuration, data.reloadExitDuration, data.maxAmmoCountInMagazine, _animator, OnReloadRequested, OnPlayOwnerAnimation);
    }
}