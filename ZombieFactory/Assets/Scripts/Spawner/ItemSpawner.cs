using System;
using System.Collections;
using System.Collections.Generic;
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

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            Spawn(i);
        }
    }

    public void DestroyItems()
    {
        //for (int i = 0; i < _storedItems.Count; i++)
        //{
        //    Destroy(_storedItems[i].gameObject);
        //}
    }

    void Spawn(int index)
    {
        int enumCount = Enum.GetNames(typeof(BaseItem.Name)).Length;
        BaseItem.Name itemName = (BaseItem.Name)UnityEngine.Random.Range(0, enumCount);

        if(itemName == BaseItem.Name.Knife)
        {
            Spawn(index); // 다시 돌린다.
        }
        else
        {
            BaseItem item = _itemFactory.Create(itemName);
            item.transform.position = _spawnPoints[index].position;
            item.PositionWeapon(true);

            _storedItems.Add(item);
        }
    }
}
