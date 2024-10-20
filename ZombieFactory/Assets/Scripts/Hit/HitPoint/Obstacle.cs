using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IPenetrable, IEffectable
{
    protected float _durability = 0;
    Dictionary<IEffectable.ConditionType, BaseEffect.Name> _hitEffect;

    private void Start()
    {
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
    public void SpawnEffect(IEffectable.ConditionType effectType, Vector3 hitPosition, Vector3 hitNormal)
    {
        if (_hitEffect.ContainsKey(effectType) == false) return;
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.SpawnEffect, _hitEffect[effectType], hitPosition, hitNormal);
    }
}
