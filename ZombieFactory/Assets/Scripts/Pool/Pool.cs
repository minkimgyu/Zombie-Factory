using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool
{
    IPoolable _poolObject;
    protected IObjectPool<IPoolable> _pool;

    public Pool(IPoolable poolObject, int startSize)
    {
        _poolObject = poolObject;
        _pool = new ObjectPool<IPoolable>(
            CreateObject,
            OnGetObject,
            OnReleaseObject,
            OnDestroyObject,
            false,
            startSize,
            1000);

        for (int i = 0; i < startSize; i++)
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
        poolable.SetReturnToPoolEvent(() => _pool.Release(poolable)); // ���� Ǯ�� ������ �� �ְ� �ѱ�.
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