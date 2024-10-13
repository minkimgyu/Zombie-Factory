using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class AudioAssetLoader : AssetLoader<ISoundPlayable.SoundName, AudioClip, AudioClip>
{
    public AudioAssetLoader(AddressableHandler.Label label, Action<AddressableHandler.Label, Dictionary<ISoundPlayable.SoundName, AudioClip>> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ProfileSpriteAssetLoader : AssetLoader<BaseItem.Name, Sprite, Sprite>
{
    public ProfileSpriteAssetLoader(AddressableHandler.Label label, Action<AddressableHandler.Label, Dictionary<BaseItem.Name, Sprite>> OnComplete) : base(label, OnComplete)
    {
    }
}
public class ItemSpriteAssetLoader : AssetLoader<BaseItem.Name, Sprite, Sprite>
{
    public ItemSpriteAssetLoader(AddressableHandler.Label label, Action<AddressableHandler.Label, Dictionary<BaseItem.Name, Sprite>> OnComplete) : base(label, OnComplete)
    {
    }
}

public class PreviewSpriteAssetLoader : AssetLoader<BaseItem.Name, Sprite, Sprite>
{
    public PreviewSpriteAssetLoader(AddressableHandler.Label label, Action<AddressableHandler.Label, Dictionary<BaseItem.Name, Sprite>> OnComplete) : base(label, OnComplete)
    {
    }
}


abstract public class AssetLoader<Key, Value, Type> : BaseAssetLoader<Key, Value, Type>
{
    protected AssetLoader(AddressableHandler.Label label, Action<AddressableHandler.Label, Dictionary<Key, Value>> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<Value>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Key key = (Key)Enum.Parse(typeof(Key), location.PrimaryKey);

                    //Debug.Log(key);
                    //Debug.Log(handle.Result);

                    dictionary.Add(key, handle.Result);
                    OnComplete?.Invoke();
                    break;

                case AsyncOperationStatus.Failed:
                    break;

                default:
                    break;
            }
        };
    }
}
