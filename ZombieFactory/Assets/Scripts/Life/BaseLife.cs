using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseLife : MonoBehaviour, IDamageable, IHealable, ITarget
{
    public enum Name
    {
        Player,

        Oryx,
        Rook,
        Warden,

        Mask,
        Police,
        Witch,
        Mild
    }

    public enum LifeState
    {
        Alive,
        Die
    }

    // HitTarget[] _hitTargets;
    protected float _maxHp;
    protected float _hp;

    protected Action<float> OnHpChangeRequested;
    protected IIdentifiable.Type _myType;
    protected LifeState _lifeState;

    public virtual void Initialize()
    {
        _lifeState = LifeState.Alive;
        //_hitTargets = GetComponentsInChildren<HitTarget>();
        //for (int i = 0; i < _hitTargets.Length; i++)
        //{
        //    _hitTargets[i].Initialize(this, this);
        //}
    }

    //public virtual void ResetData(PlayerData data) { }
    //public virtual void ResetData(HelperData data) { }
    //public virtual void ResetData(ZombieData data) { }


    public virtual void AddObserverEvent() { }
    public virtual void OnDieRequested() { }

    public bool IsOpponent(List<IIdentifiable.Type> types)
    {
        for (int i = 0; i < types.Count; i++)
        {
            if (types[i] == _myType) return true;
        }

        return false;
    }

    public void GetDamage(float damage)
    {
        if (_lifeState == LifeState.Die) return;

        _hp -= damage;
        if (_hp < 0)
        {
            _lifeState = LifeState.Die;
            _hp = 0;
            OnHpChangeRequested?.Invoke(_hp / _maxHp);

            OnDieRequested();
            return;
        }

        OnHpChangeRequested?.Invoke(_hp / _maxHp);
    }

    public void GetHeal(float healPoint)
    {
        if (_lifeState == LifeState.Die) return;

        _hp += healPoint;
        if (_hp > _maxHp)
        {
            _hp = _maxHp;
            OnHpChangeRequested?.Invoke(_hp / _maxHp);
            return;
        }

        OnHpChangeRequested?.Invoke(_hp / _maxHp);
    }

    public Vector3 ReturnDirection()
    {
        return transform.forward;
    }

    public Vector3 ReturnPosition()
    {
        return transform.position;
    }
}
