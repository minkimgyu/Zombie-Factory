using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePart : HitPoint
{
    public override void Initialize(IDamageable parentDamageable, IPoint parentBody, BaseFactory effectFactory)
    {
        base.Initialize(parentDamageable, parentBody, effectFactory);
        _hitEffects = new Dictionary<IEffectable.ConditionType, BaseEffect.Name>()
        {
            // ������ ���� �����Ǵ� ȿ�� �ٸ����� �� ����
            {IEffectable.ConditionType.Penetration, BaseEffect.Name.ObjectFragmentation},
            {IEffectable.ConditionType.NonPenetration, BaseEffect.Name.ObjectFragmentation},
            {IEffectable.ConditionType.Stabbing, BaseEffect.Name.ObjectFragmentation}
        };
    }

    public override void SpawnEffect(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.DamageTxt);
        effect.ResetData(hitPosition, hitNormal, Quaternion.LookRotation(-hitNormal), damage);
        effect.Play();
    }
}
