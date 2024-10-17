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

        // ���� ������ ����� ���� Ÿ���� ������ ���
        // �ƴϸ� �ٸ� ���
        weapon.OnRooting(_eventBlackboard);

        _weaponsContainer.Add(weapon.WeaponType, weapon);
        AttachWeaponToArm(weapon);

        BaseWeapon equipedWeapon = ReturnWeapon();

        if (equipedWeapon == null)
        {
            // Drop -> Root
            // ���� ���� ���⸦ �������ش�.
            _baseFSM.SetState(WeaponController.State.Equip, weapon.WeaponType, "EquipWeapon");
        }
        else
        {
            // ���� ���� ����� ���� �����ϰ� �ִ� ������ Ÿ���� ���� ���
            if (equipedWeapon.WeaponType == weapon.WeaponType)
            {
                _baseFSM.SetState(WeaponController.State.Equip, weapon.WeaponType, "EquipWeapon");
            }
            else
            {
                // �ٸ� ���
                _baseFSM.SetState(WeaponController.State.Idle);
            }
        }
    }
}
