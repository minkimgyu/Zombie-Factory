using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : BaseWeapon
{
    Action<bool> ActiveAmmoViewer;

    public override void OnRooting(WeaponBlackboard blackboard)
    {
        base.OnRooting(blackboard);
        ActiveAmmoViewer = blackboard.ActiveAmmoViewer;
    }

    public override void OnEquip()
    {
        base.OnEquip();
        ActiveAmmoViewer?.Invoke(false);
    }

    public override void ResetData(KnifeData data, BaseFactory effectFactory)
    {
        _equipFinishTime = data.equipFinishTime;
        _weaponWeight = data.weaponWeight;

        _eventStrategies[EventType.Main] = new AutoEvent(EventType.Main, data.mainAttackDelay, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);
        _eventStrategies[EventType.Sub] = new AutoEvent(EventType.Sub, data.subAttackDelay, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);

        _actionStrategies[EventType.Main] = new LeftKnifeAttack(_weaponName, data.range, _targetLayer,
           data.mainAnimationCount, data.mainAttackEffectDelayTime, data.attackLinkDuration, data.mainAttackDamageData, _animator);

        _actionStrategies[EventType.Sub] = new RightKnifeAttack(_weaponName, data.range, _targetLayer,
           data.subAttackEffectDelayTime, data.subAttackDamageData, _animator);

        _recoilStrategies[EventType.Main] = new NoRecoilGenerator();
        _recoilStrategies[EventType.Sub] = new NoRecoilGenerator();
        _reloadStrategy = new NoReload();
    }
}
