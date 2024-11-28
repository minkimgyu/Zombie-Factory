using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour, IPenetrable, IEffectable, IHitable
{
    protected float _durability = 0;
    protected Dictionary<IEffectable.ConditionType, BaseEffect.Name> _hitEffects;

    protected IHitable.Area _area;

    IDamageable _damageable;
    ITarget _bodyPoint;
    protected BaseFactory _effectFactory;

    public virtual void Initialize(IDamageable parentDamageable, ITarget parentBody, BaseFactory effectFactory)
    {
        _damageable = parentDamageable;
        _bodyPoint = parentBody;
        _effectFactory = effectFactory;
    }

    public Vector3 ReturnParentBodyDirection()
    {
        return _bodyPoint.ReturnTargetPoint().forward;
    }

    public float ReturnDurability()
    {
        return _durability;
    }

    public void OnHit(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        _damageable.GetDamage(damage);
    }

    public IHitable.Area ReturnArea()
    {
        return _area;
    }

    // 여기서 바로 이펙트 생성시키기
    public void SpawnEffect(IEffectable.ConditionType effectType)
    {
        BaseEffect effect = _effectFactory.Create(_hitEffects[effectType]);
        effect.Play();
    }

    public void SpawnEffect(IEffectable.ConditionType effectType, Vector3 hitPosition, Vector3 shootPosition, Quaternion holeRotation)
    {
        BaseEffect effect = _effectFactory.Create(_hitEffects[effectType]);
        effect.ResetData(hitPosition, shootPosition, holeRotation);
        effect.Play();
    }

    public virtual void SpawnEffect(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
    }

    public void SpawnEffect(IEffectable.ConditionType effectType, Vector3 hitPosition, Vector3 hitNormal)
    {
        BaseEffect effect = _effectFactory.Create(_hitEffects[effectType]);
        effect.ResetData(hitPosition, hitNormal, Quaternion.LookRotation(-hitNormal));
        effect.Play();
    }
}
