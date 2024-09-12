using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : BaseWeapon
{
    public override void OnEquip()
    {
        base.OnEquip();
        OnShowRounds?.Invoke(false, 0, 0);
    }

    public override void ResetData(BatData data)
    {
        _eventStrategies[EventType.Main] = new AutoEvent(EventType.Main, data.delayBetweenAttack, OnEventStart, OnEventUpdate, OnEventEnd, OnAction);
        _eventStrategies[EventType.Sub] = new NoEvent();

        _actionStrategies[EventType.Main] = new BatAttack(_weaponName, data.range, _targetLayer, data.damageDelayTime, data.attackDamageData);
        _actionStrategies[EventType.Sub] = new NoAction();

        _recoilStrategies[EventType.Main] = new NoRecoilGenerator();
        _recoilStrategies[EventType.Sub] = new NoRecoilGenerator();
    }
}
