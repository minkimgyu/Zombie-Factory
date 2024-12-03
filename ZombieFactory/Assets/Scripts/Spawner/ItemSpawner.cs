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

        if (itemName == BaseItem.Name.Knife)
        {
            CreateItem(pointIndex); // 다시 돌린다.
        }
        else
        {
            BaseItem item = _itemFactory.Create(itemName);
            item.transform.position = _spawnPoints[pointIndex].position;
            item.PositionItem(true);

            _storedItems.Add(item);
        }
    }

    public override void Spawn()
    {
        BaseItem item = _itemFactory.Create(BaseItem.Name.Stinger);
        item.transform.position = _spawnPoints[0].position;
        item.PositionItem(true);

        BaseItem item1 = _itemFactory.Create(BaseItem.Name.Bucky);
        item1.transform.position = _spawnPoints[1].position;
        item1.PositionItem(true);

        BaseItem item2 = _itemFactory.Create(BaseItem.Name.Guardian);
        item2.transform.position = _spawnPoints[2].position;
        item2.PositionItem(true);

        //for (int i = 0; i < _spawnPoints.Length; i++)
        //{
        //    CreateItem(i);
        //}
    }
}
