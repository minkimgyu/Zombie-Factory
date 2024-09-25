using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseState1<T>
{
    protected BaseFSM1<T> _baseFSM;
    public BaseState1(FSM1<T> fsm) { _baseFSM = fsm; }

    public abstract void OnStateEnter();
    public virtual void OnStateEnter(BaseWeapon newWeapon, string message) { }
    public virtual void OnStateEnter(BaseWeapon.Type weaponType, string message) { }

    public abstract void OnStateUpdate();
    public abstract void OnStateExit();

    public virtual void OnStateFixedUpdate() { }

    public virtual void OnCollisionEnter(Collision collision) { }

    public virtual void OnHandleStop() { }
    public virtual void OnHandleJump() { }
    public virtual void OnHandleMove(Vector3 input) { }

    public virtual void OnHandleEquip(BaseWeapon.Type type) { }
    public virtual void OnHandleDrop() { }
    public virtual void OnHandleReload() { }
    public virtual void OnHandleEventStart(BaseWeapon.EventType type) { }
    public virtual void OnHandleEventEnd() { }

    public virtual void OnWeaponReceived(BaseWeapon weapon) { }
}