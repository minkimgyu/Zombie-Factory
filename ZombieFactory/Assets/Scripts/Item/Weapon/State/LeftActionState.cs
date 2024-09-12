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

        // ��� ���� �Ѿ��� ������ ���, State�� ������ ���� �Ѿ��� ����������
        // Update �� �Ѿ��� �� ������ ���
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
