using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class LifeJsonAssetLoader : JsonAssetLoader<BaseLife.Name, LifeData>
{
    public LifeJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseLife.Name, LifeData>> OnComplete) : base(label, OnComplete)
    {
    }
}
public class ItemJsonAssetLoader : JsonAssetLoader<BaseItem.Name, ItemData>
{
    public ItemJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseItem.Name, ItemData>> OnComplete) : base(label, OnComplete)
    {
    }
}
public class RecoilJsonAssetLoader : JsonAssetLoader<BaseItem.Name, BaseRecoilData>
{
    public RecoilJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseItem.Name, BaseRecoilData>> OnComplete) : base(label, OnComplete)
    {
    }
}

abstract public class JsonAssetLoader<Key, Value> : BaseAssetLoader<Key, Value, TextAsset>
{
    JsonParser _parser;
    protected JsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>> OnComplete) : base(label, OnComplete)
    {
        _parser = new JsonParser();
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<TextAsset>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Key key = (Key)Enum.Parse(typeof(Key), location.PrimaryKey);
                    Value value = _parser.JsonToObject<Value>(handle.Result);

                    Debug.Log(key);
                    Debug.Log(handle.Result);

                    Debug.Log(value);

                    dictionary.Add(key, value);
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
