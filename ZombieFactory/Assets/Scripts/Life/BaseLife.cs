using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseLife : MonoBehaviour, IDamageable, IHealable, ITarget
{
    public enum Name
    {
        Player,

        Rook,
        Warden,

        MaskZombie,
        PoliceZombie,
        WitchZombie,
    }

    public enum LifeState
    {
        Alive,
        Die
    }

    HitPoint[] _hitPoints;
    protected float _maxHp;
    protected float _hp;

    protected Action<float> OnHpChangeRequested;
    protected IIdentifiable.Type _myType;
    protected LifeState _lifeState;

    protected BaseFactory _effectFactory;

    public void ResetPosition(Vector3 pos)
    {
        // interpolate 사용 시 Rigidbody를 사용해서 위치를 변경해줘야한다.
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.position = pos;
    }

    public virtual void InitializeFSM() { }

    public virtual void Initialize() 
    {
        _hp = _maxHp;
        _lifeState = LifeState.Alive;
        _hitPoints = GetComponentsInChildren<HitPoint>();
        for (int i = 0; i < _hitPoints.Length; i++)
        {
            _hitPoints[i].Initialize(this, this, _effectFactory);
        }
    }

    public virtual void ResetData(PlayerData data, HelperMediator mediator, BaseFactory effectFactory) { }
    public virtual void ResetData(SwatData data, BaseFactory effectFactory, BaseFactory ragdollFactory) { }
    public virtual void ResetData(ZombieData data, BaseFactory effectFactory, BaseFactory ragdollFactory) { }

    public virtual void AddObserverEvent
    (
        Action<Vector3, Vector3> MoveCamera,
        Action<float, float> OnFieldOfViewChange,

        Action<float> OnHpChangeRequested,
        Action<bool> SwitchCrosshair,

        Action<bool> ActiveAmmoViewer,
        Action<int, int> UpdateAmmoViewer,
        Action<BaseItem.Name, BaseWeapon.Type> AddPreview,
        Action<BaseWeapon.Type> RemovePreview)
    { }

    public virtual void AddObserverEvent(Action OnDie)
    { }

    public void AddObserverEvent(Action<float> OnHpChangeRequested)
    { this.OnHpChangeRequested = OnHpChangeRequested; }

    protected virtual void OnDieRequested() { }

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
