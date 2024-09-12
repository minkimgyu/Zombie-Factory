using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LeftActionState : BaseWeaponState
{
    Func<BaseWeapon> ReturnWeapon;

    public LeftActionState(FSM<WeaponController.State> fsm, Func<BaseWeapon> ReturnWeapon) : base(fsm)
    {
        this.ReturnWeapon = ReturnWeapon;
    }

    public override void OnStateEnter()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        equipedWeapon.OnLeftClickStart();
    }

    public override void OnStateExit()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        equipedWeapon.OnLeftClickEnd();
    }

    public override void OnStateUpdate()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        equipedWeapon.OnLeftClickProcess();

        // 사격 도중 총알이 떨어진 경우, State에 들어왔을 때는 총알이 존재했지만
        // Update 중 총알이 다 떨어진 경우
        if (equipedWeapon.CanAutoReload())
        {
            _baseFSM.SetState(WeaponController.State.Reload);
        }
    }

    public override void OnHandleEventEnd(BaseWeapon.EventType type)
    {
        if(type == BaseWeapon.EventType.Main)
        {
            _baseFSM.SetState(WeaponController.State.Idle);
        }
    }
}
