using UnityEngine;
using System;
using System.Collections.Generic;

public class ReloadState : BaseWeaponState
{
    Dictionary<BaseWeapon.Type, BaseWeapon> _weaponsContainer;

    Func<BaseWeapon> ReturnWeapon;
    Action<BaseWeapon> ChangeWeapon;
    bool _isTPS;

    public ReloadState(
        FSM<WeaponController.State> fsm,
        Dictionary<BaseWeapon.Type, BaseWeapon> weaponsContainer,
        bool isTPS,
        Action<BaseWeapon> ChangeWeapon,
        Func<BaseWeapon> ReturnWeapon) : base(fsm)
    {
        _weaponsContainer = weaponsContainer;
        _isTPS = isTPS;
        this.ChangeWeapon = ChangeWeapon;
        this.ReturnWeapon = ReturnWeapon;
    }

    public override void OnStateUpdate()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        bool isFinish = equipedWeapon.IsReloadFinish();
        if (isFinish)
        {
            equipedWeapon.OnReloadEnd();
            _baseFSM.SetState(WeaponController.State.Idle);
            return;
        }
    }

    public override void OnHandleEquip(BaseWeapon.Type type)
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        if (equipedWeapon.WeaponType == type) return; // 이미 같은 타입의 아이템이 장착되어 있으면 리턴

        _baseFSM.SetState(WeaponController.State.Equip, type, "SendWeaponTypeToEquip");
    }

    public override void OnHandleEventStart(BaseWeapon.EventType type)
    {
        BaseWeapon equipedWeapon = ReturnWeapon();

        switch (type)
        {
            case BaseWeapon.EventType.Main:
                bool nowCancelMainAction = equipedWeapon.CanCancelReloadAndGoToMainAction();
                if (nowCancelMainAction == false) break;

                _baseFSM.SetState(WeaponController.State.LeftAction);
                break;
            case BaseWeapon.EventType.Sub:
                bool nowCancelSubAction = equipedWeapon.CanCancelReloadAndGoToSubAction();
                if (nowCancelSubAction == false) break;

                _baseFSM.SetState(WeaponController.State.RightAction);
                break;
        }
    }

    public override void OnWeaponReceived(BaseWeapon weapon)
    {
        bool containWeapon = _weaponsContainer.ContainsKey(weapon.WeaponType);
        if (containWeapon)
        {
            _baseFSM.SetState(WeaponController.State.Drop, weapon, "DropSameTypeWeaponAndRootNewWeapon");
        }
        else
        {
            _baseFSM.SetState(WeaponController.State.Root, weapon, "RootNewWeapon");
        }
    }

    public override void OnStateExit()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        equipedWeapon.ResetReload();
    }

    public override void OnStateEnter()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        equipedWeapon.OnReloadStart(_isTPS);
    }
}
