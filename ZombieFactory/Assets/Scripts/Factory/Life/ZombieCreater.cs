using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

[System.Serializable]
public class ZombieData : LifeData
{
    public float destoryDelay = 5;
    public float stageChangeDuration = 3;

    public float moveSpeed = 5;
    public float viewSpeed = 5;

    public float stopDistance = 1f;
    public float gap = 0.5f;

    public float targetCaptureRadius = 5;

    public float noiseCaptureRadius = 11;
    public int maxNoiseQueueSize = 3;

    public float attackRadius = 1.5f;
    public float attackDamage = 30;

    public float attackPreDelay = 0.5f;
    public float attackAfterDelay = 3;

    public ZombieData(
        float maxHp,

        float destoryDelay,
        float stageChangeDuration,
        float moveSpeed,
        float viewSpeed,
        float stopDistance,
        float gap,
        float targetCaptureRadius,
        float noiseCaptureRadius,
        int maxNoiseQueueSize,
        float attackRadius,
        float attackDamage,
        float attackPreDelay,
        float attackAfterDelay) : base(maxHp)
    {
        this.destoryDelay = destoryDelay;
        this.stageChangeDuration = stageChangeDuration;
        this.moveSpeed = moveSpeed;
        this.viewSpeed = viewSpeed;
        this.stopDistance = stopDistance;
        this.gap = gap;
        this.targetCaptureRadius = targetCaptureRadius;
        this.noiseCaptureRadius = noiseCaptureRadius;
        this.maxNoiseQueueSize = maxNoiseQueueSize;
        this.attackRadius = attackRadius;
        this.attackDamage = attackDamage;
        this.attackPreDelay = attackPreDelay;
        this.attackAfterDelay = attackAfterDelay;
    }
}

public class ZombieCreater : LifeCreater
{
    BaseFactory _effectFactory;
    BaseFactory _ragdollFactory;

    public ZombieCreater(BaseLife lifePrefab, LifeData lifeData, BaseFactory effectFactory, BaseFactory ragdollFactory) : base(lifePrefab, lifeData)
    {
        _effectFactory = effectFactory;
        _ragdollFactory = ragdollFactory;
    }

    public override BaseLife Create()
    {
        BaseLife life = Object.Instantiate(_lifePrefab);
        ZombieData data = _lifeData as ZombieData;

        life.ResetData(data, _effectFactory, _ragdollFactory);
        life.Initialize();
        life.InitializeFSM();
        return life;
    }
}
