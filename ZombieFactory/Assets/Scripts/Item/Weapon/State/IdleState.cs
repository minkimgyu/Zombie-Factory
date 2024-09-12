using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IdleState : BaseWeaponState
{
    Dictionary<BaseWeapon.Type, BaseWeapon> _weaponsContainer;
    Func<BaseWeapon> ReturnWeapon;

    public IdleState(
        FSM<WeaponController.State> fsm,
        Dictionary<BaseWeapon.Type, BaseWeapon> weaponsContainer,
        Func<BaseWeapon> ReturnWeapon) : base(fsm)
    {
        _weaponsContainer = weaponsContainer;
        this.ReturnWeapon = ReturnWeapon;
    }

    public override void OnStateEnter()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        if (equipedWeapon == null) return;

        if (equipedWeapon.CanAutoReload() == true)
        {
            _baseFSM.SetState(WeaponController.State.Reload);
        }
    }

    public override void OnHandleEquip(BaseWeapon.Type type)
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        if (equipedWeapon != null && equipedWeapon.WeaponType == type) return; // 이미 같은 타입의 아이템이 장착되어 있으면 리턴

        _baseFSM.SetState(WeaponController.State.Equip, type, "SendWeaponTypeToEquip");
    }

    public override void OnHandleEventStart(BaseWeapon.EventType type)
    {
        switch (type)
        {
            case BaseWeapon.EventType.Main:
                _baseFSM.SetState(WeaponController.State.LeftAction);
                break;
            case BaseWeapon.EventType.Sub:
                _baseFSM.SetState(WeaponController.State.RightAction);
                break;
        }
    }

    public override void OnHandleDrop()
    {
        _baseFSM.SetState(WeaponController.State.Drop);
    }

    public override void OnHandleReload()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        if (equipedWeapon.CanReload() == false) return;

        _baseFSM.SetState(WeaponController.State.Reload);
    }

    public override void OnWeaponReceived(BaseWeapon weapon)
    {
        bool containWeapon = _weaponsContainer.ContainsKey(weapon.WeaponType);
        if(containWeapon)
        {
            _baseFSM.SetState(WeaponController.State.Drop, weapon, "DropSameTypeWeaponAndRootNewWeapon");
        }
        else
        {
            _baseFSM.SetState(WeaponController.State.Root, weapon, "RootNewWeapon");
        }
    }
}
