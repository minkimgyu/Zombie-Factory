using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool
{
    IPoolable _poolObject;
    protected IObjectPool<IPoolable> _pool;
    Transform _parent;

    public Pool(IPoolable poolObject, int startCount, Transform parent)
    {
        _poolObject = poolObject;
        _pool = new ObjectPool<IPoolable>(
            CreateObject,
            OnGetObject,
            OnReleaseObject,
            OnDestroyObject);

        _parent = parent;

        for (int i = 0; i < startCount; i++)
        {
            CreateObject();
        }
    }
    // ������Ʈ�� ������ �� ����
    private IPoolable CreateObject()
    {
        GameObject poolObject = Object.Instantiate(_poolObject.ReturnObject());
        IPoolable poolable = poolObject.GetComponent<IPoolable>();

        poolable.Initialize();
        poolable.SetParent(_parent);
        poolable.SetReturnToPoolEvent(() => _pool.Release(poolable)); // ���� Ǯ�� ������ �� �ְ� �ѱ�.
        poolable.SetActive(false);
        return poolable;
    }

    // Ǯ���� ������Ʈ�� ���� �� ����
    private void OnGetObject(IPoolable poolable)
    {
        poolable.SetActive(true);
    }

    // ������Ʈ�� Ǯ�� ȸ���� �� ����
    private void OnReleaseObject(IPoolable poolable)
    {
        poolable.SetActive(false);
    }

    // ������Ʈ�� Ǯ���� ������ �� ����
    private void OnDestroyObject(IPoolable poolable)
    {
        Object.Destroy(poolable.ReturnObject());
    }
}