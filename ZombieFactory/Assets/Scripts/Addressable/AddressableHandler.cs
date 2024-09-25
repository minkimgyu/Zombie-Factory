using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime;

public class AddressableHandler
{
    public enum Label
    {
        Life,
        Item,
        Effect,
        Viewer,
        Sound,

        Ragdoll,

        ProfileSprite,
        itemSprite,
        PreviewSprite,

        LifeData,
        ItemData,

        LeftRecoilData,
        RightRecoilData,
    }

    Dictionary<Label, BaseLoader> _assetLoaders;

    int _successCount;
    int _totalCount;
    Action OnCompleted;

    public AddressableHandler()
    {
        _successCount = 0;
        _totalCount = 0;
    }

    public Dictionary<BaseEffect.Name, BaseEffect> EffectPrefabs { get; private set; }
    public Dictionary<BaseLife.Name, BaseLife> LifePrefabs { get; private set; }
    public Dictionary<BaseLife.Name, Ragdoll> RagdollPrefabs { get; private set; }
    public Dictionary<BaseItem.Name, BaseItem> ItemPrefabs { get; private set; }

    public Dictionary<BaseLife.Name, LifeData> LifeDataDictionary { get; private set; }
    public Dictionary<BaseItem.Name, ItemData> ItemDataDictionary { get; private set; }
    public Dictionary<BaseItem.Name, BaseRecoilData> LeftRecoilDataDictionary { get; private set; }
    public Dictionary<BaseItem.Name, BaseRecoilData> RightRecoilDataDictionary { get; private set; }

    public void Load(Action OnCompleted)
    {
        _assetLoaders = new Dictionary<Label, BaseLoader>();

        _assetLoaders.Add(Label.Item, new ItemAssetLoader(Label.Item, (value) => { ItemPrefabs = value; OnSuccess(Label.Item); }));

        _assetLoaders.Add(Label.Ragdoll, new RagdollAssetLoader(Label.Ragdoll, (value) => { RagdollPrefabs = value; OnSuccess(Label.Ragdoll); }));

        _assetLoaders.Add(Label.Life, new LifeAssetLoader(Label.Life, (value) => { LifePrefabs = value; OnSuccess(Label.Life); }));

        _assetLoaders.Add(Label.Effect, new EffectAssetLoader(Label.Effect, (value) => { EffectPrefabs = value; OnSuccess(Label.Effect); }));

        _assetLoaders.Add(Label.ItemData, new ItemJsonAssetLoader(Label.ItemData, (value) => { ItemDataDictionary = value; OnSuccess(Label.ItemData); }));

        _assetLoaders.Add(Label.LifeData, new LifeJsonAssetLoader(Label.LifeData, (value) => { LifeDataDictionary = value; OnSuccess(Label.LifeData); }));

        _assetLoaders.Add(Label.LeftRecoilData, new RecoilJsonAssetLoader(Label.LeftRecoilData, (value) => { LeftRecoilDataDictionary = value; OnSuccess(Label.LeftRecoilData); }));

        _assetLoaders.Add(Label.RightRecoilData, new RecoilJsonAssetLoader(Label.RightRecoilData, (value) => { RightRecoilDataDictionary = value; OnSuccess(Label.RightRecoilData); }));

        this.OnCompleted = OnCompleted;
        _totalCount = _assetLoaders.Count;
        foreach (var loader in _assetLoaders)
        {
            loader.Value.Load();
        }
    }

    void OnSuccess(Label label)
    {
        _successCount++;
        Debug.Log(_successCount);
        Debug.Log(label.ToString() + "Success");

        if (_successCount == _totalCount)
        {
            Debug.Log("Complete!");
            OnCompleted?.Invoke();
        }
    }

    public void Release()
    {
        foreach (var loader in _assetLoaders)
        {
            loader.Value.Release();
        }
    }
}