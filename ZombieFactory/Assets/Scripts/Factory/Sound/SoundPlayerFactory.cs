using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerCreater : Pool
{
    IPoolable _prefab;

    public SoundPlayerCreater(SoundPlayer sfxPrefab, int startSize) : base(sfxPrefab, startSize)
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

    public SoundPlayerFactory(AddressableHandler addressableHandler)
    {
        _soundCreater = new SoundPlayerCreater(addressableHandler.SoundPlayer, 30);
    }

    public override SoundPlayer Create()
    {
        return _soundCreater.Create();
    }
}
