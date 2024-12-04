using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ItemSpawner : BaseSpawner
{
    [SerializeField] Transform[] _spawnPoints;
    BaseFactory _itemFactory;

    List<BaseItem> _storedItems;

    public override void Initialize(BaseFactory itemFactory)
    {
        _storedItems = new List<BaseItem>();
        _itemFactory = itemFactory;
    }

    void CreateItem(int pointIndex)
    {
        int enumCount = Enum.GetNames(typeof(BaseItem.Name)).Length;
        BaseItem.Name itemName = (BaseItem.Name)UnityEngine.Random.Range(0, enumCount);

        if (_noSpawnNames.Contains(itemName) == true || _storedNames.Contains(itemName) == true)
        {
            CreateItem(pointIndex); // 다시 돌린다.
        }
        else
        {
            BaseItem item = _itemFactory.Create(itemName);
            item.transform.position = _spawnPoints[pointIndex].position;
            item.PositionItem(true);

            _storedNames.Add(itemName);
            _storedItems.Add(item);
        }
    }

    List<BaseItem.Name> _storedNames = new List<BaseItem.Name>();
    List<BaseItem.Name> _noSpawnNames = new List<BaseItem.Name>
    {
        BaseItem.Name.Knife,
        BaseItem.Name.Classic,
    };

    public override void Spawn()
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            CreateItem(i);
        }

        _storedNames.Clear();
    }
}
