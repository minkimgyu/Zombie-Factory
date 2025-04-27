using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : BaseWeapon
{
    Action<bool> ActiveAmmoViewer;

    public void PlayStabSound()
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundControllable.SoundName.KnifeStap, 0.7f);
    }

    public override void OnRooting(WeaponBlackboard blackboard)
    {
        base.OnRooting(blackboard);
        ActiveAmmoViewer = blackboard.ActiveAmmoViewer;
    }

    public override bool NowDrop() { return false; }

    public override void OnEquip()
    {
        base.OnEquip();
        ActiveAmmoViewer?.Invoke(false);
    }

    public override void ResetData(KnifeData data, BaseFactory effectFactory)
    {
        _equipFinishTime = data.equipFinishTime;
        _weaponWeight = data.weaponWeight;

        _eventStrategy[EventType.Main] = new AutoEvent(EventType.Main, data.mainAttackDelay, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);
        _eventStrategy[EventType.Sub] = new AutoEvent(EventType.Sub, data.subAttackDelay, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);

        _actionStrategy[EventType.Main] = new LeftKnifeAttack(_weaponName, data.range, _targetLayer,
           data.mainAnimationCount, data.mainAttackEffectDelayTime, data.attackLinkDuration, data.mainAttackDamageData, _animator);

        _actionStrategy[EventType.Sub] = new RightKnifeAttack(_weaponName, data.range, _targetLayer,
           data.subAttackEffectDelayTime, data.subAttackDamageData, _animator);

        _recoilStrategy[EventType.Main] = new NoRecoilGenerator();
        _recoilStrategy[EventType.Sub] = new NoRecoilGenerator();
        _reloadStrategy = new NoReload();
    }
}
