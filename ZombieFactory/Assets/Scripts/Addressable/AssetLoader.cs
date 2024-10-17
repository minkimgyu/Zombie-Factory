using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

abstract public class AssetLoader<Value, Type> : BaseLoader
{
    Action<AddressableHandler.Label, Value> OnComplete;
    protected Value _asset;
    protected AddressableHandler.Label _label;

    public AssetLoader(AddressableHandler.Label label, Action<AddressableHandler.Label, Value> OnComplete)
    {
        _label = label;
        this.OnComplete = OnComplete;
    }

    public override void Load()
    {
        Addressables.LoadAssetAsync<Type>(_label.ToString()).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    LoadAsset(handle.Result);
                    OnComplete?.Invoke(_label, _asset);
                    break;
                case AsyncOperationStatus.Failed:
                    break;
                default:
                    break;
            }
        };
    }

    abstract protected void LoadAsset(Type value);

    public override void Release()
    {
        Addressables.Release(_asset);
    }
}

public class SoundPlayerAssetLoader : AssetLoader<SoundPlayer, GameObject>
{

    public SoundPlayerAssetLoader(AddressableHandler.Label label, Action<AddressableHandler.Label, SoundPlayer> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(GameObject value)
    {
        _asset = value.GetComponent<SoundPlayer>();
    }
}