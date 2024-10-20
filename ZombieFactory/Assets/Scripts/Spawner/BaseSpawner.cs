using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseSpawner : MonoBehaviour
{
    public virtual void Initialize(BaseFactory factory) { }
    public virtual void Initialize(int spawnCount, BaseFactory lifeFactory, Action OnDie) { }
    public virtual void Initialize(BaseFactory lifeFactory, BaseFactory itemFactory) { }
    public virtual void Initialize(
        BaseFactory lifeFactory,
        BaseFactory itemFactory,

        CameraController cameraController,
        PlayerUIController gameUIController)
    { }

    public virtual void Spawn() { }
}
