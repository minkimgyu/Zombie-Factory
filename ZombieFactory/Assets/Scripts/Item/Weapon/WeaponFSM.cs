using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseWeaponState : BaseState<WeaponController.State>
{
    public BaseWeaponState(FSM<WeaponController.State> fsm) : base(fsm)
    {
    }

    public override void OnStateEnter(BaseWeapon.Type weaponType, string message) { }
    public override void OnStateEnter() { }
    public override void OnStateExit() { }
    public override void OnStateUpdate() { }
}

public class WeaponFSM : FSM<WeaponController.State>
{
    public void OnStateFixedUpdate() => _currentState.OnStateFixedUpdate();
    public void OnCollisionEnter(Collision collision) => _currentState.OnCollisionEnter(collision);


    public void OnHandleEquip(BaseWeapon.Type type) => _currentState.OnHandleEquip(type);
    public void OnHandleDrop() => _currentState.OnHandleDrop();
    public void OnHandleReload() => _currentState.OnHandleReload();

    public void OnHandleEventStart(BaseWeapon.EventType type) => _currentState.OnHandleEventStart(type);
    public void OnHandleEventEnd(BaseWeapon.EventType type) => _currentState.OnHandleEventEnd(type);

    public void OnWeaponReceived(BaseWeapon weapon) => _currentState.OnWeaponReceived(weapon);
}
