using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IPenetrable, IEffectable
{
    protected float _durability = 0;
    Dictionary<IEffectable.ConditionType, BaseEffect.Name> _hitEffect;

    BaseFactory _effectFactory;

    public virtual void Initialize(BaseFactory effectFactory)
    {
        _effectFactory = effectFactory;

        _hitEffect = new Dictionary<IEffectable.ConditionType, BaseEffect.Name>()
        {
            // 재질에 따라서 스폰되는 효과 다르게할 수 있음
            {IEffectable.ConditionType.Penetration, BaseEffect.Name.PenetrateBulletHole},
            {IEffectable.ConditionType.NonPenetration, BaseEffect.Name.NonPenetrateBulletHole},
            {IEffectable.ConditionType.Stabbing, BaseEffect.Name.KnifeMark}
        };
    }

    public float ReturnDurability()
    {
        return _durability;
    }

    // 여기서 바로 이펙트 생성시키기
    public void effectFactory(IEffectable.ConditionType effectType, Vector3 hitPosition, Vector3 hitNormal)
    {
        BaseEffect effect = _effectFactory.Create(_hitEffect[effectType]);
        effect.ResetData(hitPosition, hitNormal, Quaternion.LookRotation(-hitNormal));
        effect.Play();
    }

    public void effectFactory(IEffectable.ConditionType effectType) { }
    public void effectFactory(IEffectable.ConditionType effectType, Vector3 hitPosition, Vector3 shootPosition, Quaternion holeRotation) { }
    public void effectFactory(IEffectable.ConditionType effectType, float damage, Vector3 hitPosition, Vector3 hitNormal) { }
}
