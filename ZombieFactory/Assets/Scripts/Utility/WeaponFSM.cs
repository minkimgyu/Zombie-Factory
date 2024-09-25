//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Agent.Controller;

//abstract public class BaseWeaponState : BaseState1<WeaponController.State>
//{
//    public BaseWeaponState(FSM1<WeaponController.State> fsm) : base(fsm)
//    {
//    }

//    public override void OnStateEnter() { }
//    public override void OnStateExit() { }
//    public override void OnStateUpdate() { }
//}

//public class WeaponFSM : FSM1<WeaponController.State>
//{
//    public void OnStateFixedUpdate() => _currentState.OnStateFixedUpdate();
//    public void OnCollisionEnter(Collision collision) => _currentState.OnCollisionEnter(collision);


//    public void OnHandleEquip(BaseWeapon.Type type) => _currentState.OnHandleEquip(type);
//    public void OnHandleDrop() => _currentState.OnHandleDrop();
//    public void OnHandleReload() => _currentState.OnHandleReload();

//    public void OnHandleEventStart(BaseWeapon.EventType type) => _currentState.OnHandleEventStart(type);
//    public void OnHandleEventEnd() => _currentState.OnHandleEventEnd();

//    public void OnWeaponReceived(BaseWeapon weapon) => _currentState.OnWeaponReceived(weapon);
//}
