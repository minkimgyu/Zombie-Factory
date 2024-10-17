using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public interface IPoolable
{
    void Initialize();
    void SetReturnToPoolEvent(Action ReturnToPool);
    void SetActive(bool active);

    GameObject ReturnObject();
}
