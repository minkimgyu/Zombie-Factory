using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EquipState : BaseWeaponState
{
    Timer _timer;

    Action<BaseWeapon> ResetWeapon;
    Func<BaseWeapon> ReturnWeapon;

    Dictionary<BaseWeapon.Type, BaseWeapon> _weaponsContainer;

    public EquipState(
        FSM<WeaponController.State> fsm,
        Dictionary<BaseWeapon.Type, BaseWeapon> weaponsContainer,

        Action<BaseWeapon> ResetWeapon,
        Func<BaseWeapon> ReturnWeapon) : base(fsm)
    {
        _timer = new Timer();

        _weaponsContainer = weaponsContainer;

        this.ResetWeapon = ResetWeapon;
        this.ReturnWeapon = ReturnWeapon;
    }

    //public override void OnMessageReceived(string message, BaseWeapon.Type weaponTypeToEquip)
    //{
    //    _weaponTypeToEquip = weaponTypeToEquip;
    //}

    public override void OnStateUpdate()
    {
        if (_timer.CurrentState == Timer.State.Finish)
        {
            _baseFSM.SetState(WeaponController.State.Idle);
        }
    }

    /// <summary>
    /// �̰Ŵ� ���� �̺�Ʈ�� WeaponController���� �ۼ�����
    /// </summary>
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
            // ������ ������ Ÿ�̸Ӱ� ���µǱ� ������ ��� ����
        }
    }

    public override void OnStateEnter(BaseWeapon.Type weaponType, string message)
    {
        if (_weaponsContainer.ContainsKey(weaponType) == false)
        {
            _baseFSM.SetState(WeaponController.State.Idle);
            return;
        }

        BaseWeapon equipedWeapon = ReturnWeapon();

        if (equipedWeapon != null)
        {
            equipedWeapon.OnUnEquip();
            equipedWeapon.gameObject.SetActive(false);
        }

        BaseWeapon weaponToEquip = _weaponsContainer[weaponType];
        ResetWeapon?.Invoke(weaponToEquip);

        weaponToEquip.gameObject.SetActive(true);
        weaponToEquip.OnEquip();

        _timer.Start(weaponToEquip.EquipFinishTime); // ������
    }

    public override void OnStateExit() => _timer.Reset();
}
