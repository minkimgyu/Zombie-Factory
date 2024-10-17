using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DropState : BaseWeaponState
{
    float _weaponThrowPower;

    Func<BaseWeapon> ReturnWeapon;
    Action<BaseWeapon> ChangeWeapon;

    Dictionary<BaseWeapon.Type, BaseWeapon> _weaponsContainer;
    WeaponBlackboard _eventBlackboard;

    public DropState(
        FSM<WeaponController.State> fsm,
        float weaponThrowPower,
        Dictionary<BaseWeapon.Type, BaseWeapon> weaponsContainer,
        WeaponBlackboard eventBlackboard,

        Func<BaseWeapon> ReturnWeapon,
        Action<BaseWeapon> ChangeWeapon) : base(fsm)
    {
        _weaponThrowPower = weaponThrowPower;

        _weaponsContainer = weaponsContainer;

        this.ReturnWeapon = ReturnWeapon;
        this.ChangeWeapon = ChangeWeapon;
        _eventBlackboard = eventBlackboard;
    }

    bool DropWeapon(BaseWeapon weapon)
    {
        _eventBlackboard.RemovePreview(weapon.WeaponType);
        weapon.ThrowWeapon(_weaponThrowPower);
        weapon.gameObject.SetActive(true);

        weapon.OnDrop(_eventBlackboard);

        _weaponsContainer.Remove(weapon.WeaponType);
        return true;
    }

    public override void OnStateEnter()
    {
        BaseWeapon equipedWeapon = ReturnWeapon();
        BaseWeapon.Type type = ReturnNextWeaponType(equipedWeapon.WeaponType);
        bool canDrop = equipedWeapon.CanDrop();
        if (canDrop)
        {
            DropWeapon(equipedWeapon);
            ChangeWeapon?.Invoke(null);
            _baseFSM.SetState(WeaponController.State.Equip, type, "EquipNextWeapon");
        }
        else
        {
            _baseFSM.RevertToPreviousState();
        }
    }

    public override void OnStateEnter(BaseWeapon newWeapon, string message)
    {
        // bool을 리턴해서 만약 false면 Idle로 돌아감
        BaseWeapon equipedWeapon = ReturnWeapon();

        if (newWeapon.WeaponType == equipedWeapon.WeaponType) // 현재 장착하고 있는 무기의 타입과 같은 경우
        {
            bool canDrop = equipedWeapon.CanDrop();

            if (canDrop)
            {
                _eventBlackboard.RemovePreview?.Invoke(newWeapon.WeaponType);
                DropWeapon(equipedWeapon);
                ChangeWeapon?.Invoke(null);
                _baseFSM.SetState(WeaponController.State.Root, newWeapon, "RootWeapon");
            }
            else
            {
                _baseFSM.RevertToPreviousState();
            }
        }
        else // 다른 경우
        {
            BaseWeapon sameTypeWeapon = _weaponsContainer[newWeapon.WeaponType];
            if (sameTypeWeapon == null)
            {
                _baseFSM.SetState(WeaponController.State.Root, newWeapon, "RootWeaponWithNoDrop");
                return;
            }

            bool canDrop = sameTypeWeapon.CanDrop();
            if (canDrop)
            {
                DropWeapon(sameTypeWeapon);
                _baseFSM.SetState(WeaponController.State.Root, newWeapon, "RootWeapon");
            }
            else
            {
                _baseFSM.RevertToPreviousState();
            }
        }
    }

    public BaseWeapon.Type ReturnNextWeaponType(BaseWeapon.Type currentType)
    {
        if (currentType == BaseWeapon.Type.Main)
        {
            if (_weaponsContainer.ContainsKey(currentType)) return BaseWeapon.Type.Sub;
            else return BaseWeapon.Type.Melee;
        }
        else return BaseWeapon.Type.Melee;
    }
}