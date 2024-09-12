using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class AudioAssetLoader : PrefabAssetLoader<ISoundPlayable.SoundName, AudioSource>
{
    public AudioAssetLoader(AddressableHandler.Label label, Action<Dictionary<ISoundPlayable.SoundName, AudioSource>> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ProfileSpriteAssetLoader : PrefabAssetLoader<BaseItem.Name, Sprite>
{
    public ProfileSpriteAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseItem.Name, Sprite>> OnComplete) : base(label, OnComplete)
    {
    }
}
public class ItemSpriteAssetLoader : PrefabAssetLoader<BaseItem.Name, Sprite>
{
    public ItemSpriteAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseItem.Name, Sprite>> OnComplete) : base(label, OnComplete)
    {
    }
}

public class PreviewSpriteAssetLoader : PrefabAssetLoader<BaseItem.Name, Sprite>
{
    public PreviewSpriteAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseItem.Name, Sprite>> OnComplete) : base(label, OnComplete)
    {
    }
}


abstract public class AssetLoader<Key, Value, Type> : BaseAssetLoader<Key, Value, Type>
{
    protected AssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>> OnComplete) : base(label, OnComplete)
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
