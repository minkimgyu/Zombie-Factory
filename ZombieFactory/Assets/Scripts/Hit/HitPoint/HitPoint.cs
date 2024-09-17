using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour, IPenetrable, IEffectable, IHitable
{
    protected float _durability = 0;
    Dictionary<IEffectable.ConditionType, BaseEffect.Name> _hitEffect;

    protected IHitable.Area _area;

    IDamageable _damageable;
    IPoint _bodyPoint;
    BaseFactory _effectFactory;

    public virtual void Initialize(IDamageable parentDamageable, IPoint parentBody, BaseFactory effectFactory)
    {
        _damageable = parentDamageable;
        _bodyPoint = parentBody;
        _effectFactory = effectFactory;

        _hitEffect = new Dictionary<IEffectable.ConditionType, BaseEffect.Name>()
        {
            // 재질에 따라서 스폰되는 효과 다르게할 수 있음
            {IEffectable.ConditionType.Penetration, BaseEffect.Name.PenetrateBulletHole},
            {IEffectable.ConditionType.NonPenetration, BaseEffect.Name.NonPenetrateBulletHole},
            {IEffectable.ConditionType.Stabbing, BaseEffect.Name.KnifeMark}
        };
    }


    public Vector3 ReturnParentBodyDirection()
    {
        return _bodyPoint.ReturnDirection();
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
    public void effectFactory(IEffectable.ConditionType effectType)
    {
        BaseEffect effect = _effectFactory.Create(_hitEffect[effectType]);
        effect.Play();
    }

    public void effectFactory(IEffectable.ConditionType effectType, Vector3 hitPosition, Vector3 shootPosition, Quaternion holeRotation)
    {
        BaseEffect effect = _effectFactory.Create(_hitEffect[effectType]);
        effect.ResetData(hitPosition, shootPosition, holeRotation);
        effect.Play();
    }

    public void effectFactory(IEffectable.ConditionType effectType, float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        BaseEffect effect = _effectFactory.Create(_hitEffect[effectType]);
        effect.ResetData(hitPosition, hitNormal, Quaternion.LookRotation(-hitNormal), damage);
        effect.Play();
    }

    public void effectFactory(IEffectable.ConditionType effectType, Vector3 hitPosition, Vector3 hitNormal)
    {
        BaseEffect effect = _effectFactory.Create(_hitEffect[effectType]);
        effect.ResetData(hitPosition, hitNormal, Quaternion.LookRotation(-hitNormal));
        effect.Play();
    }
}
