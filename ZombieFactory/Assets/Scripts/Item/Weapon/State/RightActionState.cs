using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RightActionState : BaseWeaponState
{
    Func<BaseWeapon> ReturnWeapon;

    public RightActionState(FSM<WeaponController.State> fsm, Func<BaseWeapon> ReturnWeapon) : base(fsm)
    {
        this.ReturnWeapon = ReturnWeapon;
    }

    public override void OnStateEnter()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        equipedWeapon.OnRightClickStart();
    }

    public override void OnStateExit()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        equipedWeapon.OnRightClickEnd();
    }

    public override void OnStateUpdate()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        equipedWeapon.OnRightClickProgress();

        if (equipedWeapon.CanAutoReload())
        {
            _baseFSM.SetState(WeaponController.State.Reload);
        }
    }

    public override void OnHandleEventEnd(BaseWeapon.EventType type)
    {
        if (type == BaseWeapon.EventType.Sub)
        {
            _baseFSM.SetState(WeaponController.State.Idle);
        }
    }

    public override void OnHandleReload()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        if (equipedWeapon.CanReload() == false) return;

        _baseFSM.SetState(WeaponController.State.Reload);
    }
}