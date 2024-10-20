using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PoolObject : MonoBehaviour, IPoolable
{
    protected Timer _timer = new Timer();
    Action ReturnToPool;

    public virtual void Initialize() { }

    protected void StartTimer(float duration)
    {
        _timer.Start(duration);
    }

    protected virtual void Update()
    {
        if (_timer.CurrentState == Timer.State.Finish)
        {
            DisableObject();
        }
    }

    protected virtual void OnDisable()
    {
        transform.rotation = Quaternion.identity;
        //transform.position = Vector3.zero;
        _timer.Reset();
        ReturnToPool?.Invoke();
    }

    protected void DisableObject() => gameObject.SetActive(false);

    public GameObject ReturnObject()
    {
        return gameObject;
    }

    public void SetReturnToPoolEvent(Action ReturnToPool)
    {
        this.ReturnToPool = ReturnToPool;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
}
