using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class LifeAssetLoader : PrefabAssetLoader<BaseLife.Name, BaseLife>
{
    public LifeAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseLife.Name, BaseLife>> OnComplete) : base(label, OnComplete)
    {
    }
}
public class ItemAssetLoader : PrefabAssetLoader<BaseItem.Name, BaseItem>
{
    public ItemAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseItem.Name, BaseItem>> OnComplete) : base(label, OnComplete)
    {
    }
}
public class EffectAssetLoader : PrefabAssetLoader<BaseEffect.Name, BaseEffect>
{
    public EffectAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseEffect.Name, BaseEffect>> OnComplete) : base(label, OnComplete)
    {
    }
}
public class ViewerAssetLoader : PrefabAssetLoader<BaseViewer.Name, BaseViewer>
{
    public ViewerAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseViewer.Name, BaseViewer>> OnComplete) : base(label, OnComplete)
    {
    }
}


abstract public class PrefabAssetLoader<Key, Value> : BaseAssetLoader<Key, Value, GameObject>
{
    protected PrefabAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<GameObject>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Key key = (Key)Enum.Parse(typeof(Key), location.PrimaryKey);
                    Value value = handle.Result.GetComponent<Value>();

                    //Debug.Log(key);
                    //Debug.Log(handle.Result);

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