using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectCreater : Pool
{
    IPoolable _prefab;

    public EffectCreater(BaseEffect prefab) : base(prefab)
    {
    }

    public BaseEffect Create()
    {
        BaseEffect effect = (BaseEffect)_pool.Get();
        effect.Initialize();
        return effect;
    }
}

public class EffectFactory : BaseFactory
{
    Dictionary<BaseEffect.Name, EffectCreater> _effectCreaters;

    public EffectFactory(AddressableHandler addressableHandler)
    {
        _effectCreaters = new Dictionary<BaseEffect.Name, EffectCreater>();

        // 여기서 추가
        _effectCreaters[BaseEffect.Name.PenetrateBulletHole] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.PenetrateBulletHole]);
        _effectCreaters[BaseEffect.Name.NonPenetrateBulletHole] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.NonPenetrateBulletHole]);
        _effectCreaters[BaseEffect.Name.WallFragmentation] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.WallFragmentation]);
        _effectCreaters[BaseEffect.Name.KnifeMark] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.KnifeMark]);
        _effectCreaters[BaseEffect.Name.DamageTxt] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.DamageTxt]);
        _effectCreaters[BaseEffect.Name.Explosion] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.Explosion]);
        _effectCreaters[BaseEffect.Name.TrajectoryLine] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.TrajectoryLine]);
    }

    public override BaseEffect Create(BaseEffect.Name name)
    {
        return _effectCreaters[name].Create();
    }
}
