using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolable
{
    [SerializeField] protected float _duration = 5;
    protected Timer _timer = new Timer();
    Action ReturnToPool;

    protected void StartTimer()
    {
        _timer.Start(_duration);
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
        transform.position = Vector3.zero;
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
}
