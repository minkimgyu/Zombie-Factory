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
            // ������ ���� �����Ǵ� ȿ�� �ٸ����� �� ����
            {IEffectable.ConditionType.Penetration, BaseEffect.Name.PenetrateBulletHole},
            {IEffectable.ConditionType.NonPenetration, BaseEffect.Name.NonPenetrateBulletHole},
            {IEffectable.ConditionType.Stabbing, BaseEffect.Name.KnifeMark}
        };
    }

    public float ReturnDurability()
    {
        return _durability;
    }

    // ���⼭ �ٷ� ����Ʈ ������Ű��
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
