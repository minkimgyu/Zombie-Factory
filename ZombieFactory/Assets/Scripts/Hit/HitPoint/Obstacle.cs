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
    public void SpawnEffect(IEffectable.ConditionType effectType, Vector3 hitPosition, Vector3 hitNormal)
    {
        if (_hitEffect.ContainsKey(effectType) == false) return;
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.SpawnEffect, _hitEffect[effectType], hitPosition, hitNormal);
    }
}
