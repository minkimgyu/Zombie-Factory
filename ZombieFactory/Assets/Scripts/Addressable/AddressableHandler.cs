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

    HashSet<BaseLoader> _assetLoaders;

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
        _assetLoaders = new HashSet<BaseLoader>();

        _assetLoaders.Add(new ItemAssetLoader(Label.Item, (label, value) => { ItemPrefabs = value; OnSuccess(label); }));
        _assetLoaders.Add(new RagdollAssetLoader(Label.Ragdoll, (label, value) => { RagdollPrefabs = value; OnSuccess(label); }));
        _assetLoaders.Add(new LifeAssetLoader(Label.Life, (label, value) => { LifePrefabs = value; OnSuccess(label); }));
        _assetLoaders.Add(new EffectAssetLoader(Label.Effect, (label, value) => { EffectPrefabs = value; OnSuccess(label); }));
        _assetLoaders.Add(new ItemJsonAssetLoader(Label.ItemData, (label, value) => { ItemDataDictionary = value; OnSuccess(label); }));
        _assetLoaders.Add(new LifeJsonAssetLoader(Label.LifeData, (label, value) => { LifeDataDictionary = value; OnSuccess(label); }));
        _assetLoaders.Add(new RecoilJsonAssetLoader(Label.LeftRecoilData, (label, value) => { LeftRecoilDataDictionary = value; OnSuccess(label); }));
        _assetLoaders.Add(new RecoilJsonAssetLoader(Label.RightRecoilData, (label, value) => { RightRecoilDataDictionary = value; OnSuccess(label); }));

        this.OnCompleted = OnCompleted;
        _totalCount = _assetLoaders.Count;
        foreach (var loader in _assetLoaders)
        {
            loader.Load();
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
            loader.Release();
        }
    }
}