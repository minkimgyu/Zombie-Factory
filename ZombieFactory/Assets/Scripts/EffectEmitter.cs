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

    // ���⼭ �ٷ� ����Ʈ ������Ű��
    public void SpawnEffect(BaseEffect.Name name, Vector3 hitPosition, Vector3 hitNormal)
    {
        BaseEffect effect = _effectFactory.Create(name);
        effect.ResetData(hitPosition, hitNormal, Quaternion.LookRotation(-hitNormal));
        effect.Play();
    }
}
