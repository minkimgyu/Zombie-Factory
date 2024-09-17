using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseLife : MonoBehaviour, IDamageable, IHealable, ITarget, IWeaponAddable
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

    HitPoint[] _hitPoints;
    protected float _maxHp;
    protected float _hp;

    protected Action<float> OnHpChangeRequested;
    protected IIdentifiable.Type _myType;
    protected LifeState _lifeState;

    protected BaseFactory _effectFactory;

    public virtual void Initialize() 
    {
        _lifeState = LifeState.Alive;
        _hitPoints = GetComponentsInChildren<HitPoint>();
        for (int i = 0; i < _hitPoints.Length; i++)
        {
            _hitPoints[i].Initialize(this, this, _effectFactory);
        }
    }

    public virtual void ResetData(PlayerData data, BaseFactory effectFactory) { }
    public virtual void ResetData(HelperData data) { }
    public virtual void ResetData(ZombieData data) { }

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

    public virtual void AddWeapon(BaseWeapon weapon) { }
}
