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

    // 오브젝트가 생성될 때 실행
    private IPoolable CreateObject()
    {
        GameObject poolObject = Object.Instantiate(_poolObject.ReturnObject());
        IPoolable poolable = poolObject.GetComponent<IPoolable>();

        poolable.Initialize();
        poolable.SetReturnToPoolEvent(() => _pool.Release(poolable)); // 관리 풀을 참조할 수 있게 넘김.
        return poolable;
    }

    // 풀에서 오브젝트를 꺼낼 때 실행
    private void OnGetObject(IPoolable poolable)
    {
        poolable.SetActive(true);
    }

    // 오브젝트를 풀에 회수할 때 실행
    private void OnReleaseObject(IPoolable poolable)
    {
        poolable.SetActive(false);
    }

    // 오브젝트를 풀에서 제거할 때 실행
    private void OnDestroyObject(IPoolable poolable)
    {
        Object.Destroy(poolable.ReturnObject());
    }
}