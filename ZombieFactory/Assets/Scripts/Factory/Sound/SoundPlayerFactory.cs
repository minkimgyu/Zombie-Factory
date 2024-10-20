using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerCreater : Pool
{
    IPoolable _prefab;
    Transform _playerParent;

    public SoundPlayerCreater(SoundPlayer sfxPrefab, int startCount, Transform parent) : base(sfxPrefab, startCount, parent)
    {
    }

    public SoundPlayer Create()
    {
        SoundPlayer ragdoll = (SoundPlayer)_pool.Get();
        return ragdoll;
    }
}

public class SoundPlayerFactory : BaseFactory
{
    SoundPlayerCreater _soundCreater;

    public SoundPlayerFactory(AddressableHandler addressableHandler, Transform parent)
    {
        _soundCreater = new SoundPlayerCreater(addressableHandler.SoundPlayer, 30, parent);
    }

    public override SoundPlayer Create()
    {
        return _soundCreater.Create();
    }
}
