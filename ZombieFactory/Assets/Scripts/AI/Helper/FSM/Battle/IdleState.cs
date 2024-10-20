using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;
using UnityEditor;

namespace AI.Swat.Battle
{
    public class IdleState : BaseBattleState
    {
        SightComponent _sightComponent;
        WeaponController _weaponController;

        public IdleState(FSM<Swat.BattleState> fsm,
            WeaponController weaponController,
            SightComponent sightComponent) : base(fsm)
        {
            _weaponController = weaponController;
            _sightComponent = sightComponent;
        }

        public override void OnStateEnter()
        {
            BaseWeapon mainWeapon = _weaponController.ReturnWeapon(BaseWeapon.Type.Main);
            if (mainWeapon != null && mainWeapon.IsAmmoEmpty() == false)
            {
                _weaponController.OnHandleEquip(BaseWeapon.Type.Main);
                return;
            }

            BaseWeapon subWeapon = _weaponController.ReturnWeapon(BaseWeapon.Type.Sub);
            if (subWeapon != null && subWeapon.IsAmmoEmpty() == false)
            {
                _weaponController.OnHandleEquip(BaseWeapon.Type.Sub);
                return;
            }
        }

        public override void OnStateUpdate()
        {
            bool isInSight = _sightComponent.IsTargetInSight();
            if (isInSight == false) return;
            _baseFSM.SetState(Swat.BattleState.Attack);
        }
    }
}