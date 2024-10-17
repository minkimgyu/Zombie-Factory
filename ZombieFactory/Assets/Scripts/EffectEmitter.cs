using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEmitter
{
    BaseFactory _effectFactory;

    public EffectEmitter(BaseFactory effectFactory)
    {
        _effectFactory = effectFactory;

        EventBusManager.Instance.ObserverEventBus.Register
        (
            ObserverEventBus.Type.SpawnEffect,
            new SpawnEffectCommand(SpawnEffect)
        );
    }

    // 여기서 바로 이펙트 생성시키기
    public void SpawnEffect(BaseEffect.Name name, Vector3 hitPosition, Vector3 hitNormal)
    {
        BaseEffect effect = _effectFactory.Create(name);
        effect.ResetData(hitPosition, hitNormal, Quaternion.LookRotation(-hitNormal));
        effect.Play();
    }
}
