using UnityEngine;
using System;
using System.Collections.Generic;

public class RootState : BaseWeaponState
{
    Dictionary<BaseWeapon.Type, BaseWeapon> _weaponsContainer;

    WeaponBlackboard _eventBlackboard;
    Transform _weaponParent;
    Func<BaseWeapon> ReturnWeapon;

    public RootState(
        FSM<WeaponController.State> fsm,
        Dictionary<BaseWeapon.Type, BaseWeapon> weaponsContainer,
        Transform weaponParent,
        WeaponBlackboard eventBlackboard,
        Func<BaseWeapon> ReturnWeapon) : base(fsm)
    {
        _weaponsContainer = weaponsContainer;
        _weaponParent = weaponParent;
        _eventBlackboard = eventBlackboard;

        this.ReturnWeapon = ReturnWeapon;
    }

    void AttachWeaponToArm(BaseWeapon weapon)
    {
        weapon.transform.SetParent(_weaponParent);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.PositionWeapon(false);
    }

    public override void OnStateEnter(BaseWeapon weapon, string message)
    {
        _eventBlackboard.AddPreview?.Invoke(weapon.WeaponName, weapon.WeaponType);

        // 현재 장착한 무기랑 같은 타입의 무기인 경우
        // 아니면 다른 경우
        weapon.OnRooting(_eventBlackboard);

        _weaponsContainer.Add(weapon.WeaponType, weapon);
        AttachWeaponToArm(weapon);

        BaseWeapon equipedWeapon = ReturnWeapon();

        if (equipedWeapon == null)
        {
            // Drop -> Root
            // 새로 들어온 무기를 장착해준다.
            _baseFSM.SetState(WeaponController.State.Equip, weapon.WeaponType, "EquipWeapon");
        }
        else
        {
            // 새로 들어온 무기와 현재 장착하고 있는 무기의 타입이 같은 경우
            if (equipedWeapon.WeaponType == weapon.WeaponType)
            {
                _baseFSM.SetState(WeaponController.State.Equip, weapon.WeaponType, "EquipWeapon");
            }
            else
            {
                // 다른 경우
                _baseFSM.SetState(WeaponController.State.Idle);
            }
        }
    }
}
