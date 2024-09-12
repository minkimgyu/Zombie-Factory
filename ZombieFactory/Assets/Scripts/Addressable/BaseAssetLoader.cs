using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;
using static AddressableHandler;


abstract public class BaseLoader
{
    public abstract void Load();
    public abstract void Release();
}

abstract public class BaseAssetLoader<Key, Value, Type> : BaseLoader
{
    protected Dictionary<Key, Value> _assetDictionary;

    Label _label;
    Action<Dictionary<Key, Value>> OnComplete;
    int _successCount;
    int _totalCount;

    public BaseAssetLoader(Label label, Action<Dictionary<Key, Value>> OnComplete)
    {
        _label = label;
        this.OnComplete = OnComplete;
        _assetDictionary = new Dictionary<Key, Value>();
        _successCount = 0;
    }


    public override void Load()
    {
        Addressables.LoadResourceLocationsAsync(_label.ToString(), typeof(Type)).Completed +=
        (handle) =>
        {
            IList<IResourceLocation> locationList = handle.Result;
            _totalCount = locationList.Count;

            for (int i = 0; i < locationList.Count; i++)
            {
                LoadAsset(locationList[i], _assetDictionary, OnSuccess);
            };
        };
    }

    void OnSuccess()
    {
        _successCount++;
        //Debug.Log(_successCount);
        if (_successCount == _totalCount)
        {
            Debug.Log("Success");
            OnComplete?.Invoke(_assetDictionary);
        }
    }

    protected abstract void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete);

    public override void Release()
    {
        foreach (var asset in _assetDictionary)
        {
            Addressables.Release(asset.Value);
        }
    }
}