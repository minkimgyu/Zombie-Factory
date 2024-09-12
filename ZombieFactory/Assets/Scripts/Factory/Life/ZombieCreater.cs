using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

[System.Serializable]
public class ZombieData : LifeData
{
    public int moneyPerOne = 30;
    public float maxArmor = 50;

    public float destoryDelay = 5;

    public float angleOffset = 30;
    public float angleChangeAmount = 0.1f;
    public float stateChangeDelay = 7;

    public float moveSpeed = 3;
    public float viewSpeed = 5;
    public float attackDamage = 20;
    public int wanderOffset = 7;

    public float targetCaptureRadius = 8;
    public float targetCaptureAdditiveRadius = 1f;
    public float targetCaptureAngle = 90;

    public float noiseCaptureRadius = 11;
    public int maxNoiseQueueSize = 3;

    public float additiveAttackRadius = 0.3f;
    public float attackRange = 1.2f;
    public float attackCircleRadius = 1.5f;

    public float preAttackDelay = 0.5f;
    public float delayForNextAttack = 3;

    public float pathFindDelay = 0.5f;

    public ZombieData(
        float maxHp, 
        IIdentifiable.Type type,

        int moneyPerOne,
        float maxArmor,
        float destoryDelay,
        float angleOffset,
        float angleChangeAmount,
        float stateChangeDelay,
        float moveSpeed,
        float viewSpeed,
        float attackDamage,
        int wanderOffset,
        float targetCaptureRadius,
        float targetCaptureAdditiveRadius,
        float targetCaptureAngle,
        float noiseCaptureRadius,
        int maxNoiseQueueSize,
        float additiveAttackRadius,
        float attackRange,
        float attackCircleRadius,
        float preAttackDelay,
        float delayForNextAttack,
        float pathFindDelay) : base(maxHp, type)
    {
        this.moneyPerOne = moneyPerOne;
        this.maxArmor = maxArmor;
        this.destoryDelay = destoryDelay;
        this.angleOffset = angleOffset;
        this.angleChangeAmount = angleChangeAmount;
        this.stateChangeDelay = stateChangeDelay;
        this.moveSpeed = moveSpeed;
        this.viewSpeed = viewSpeed;
        this.attackDamage = attackDamage;
        this.wanderOffset = wanderOffset;
        this.targetCaptureRadius = targetCaptureRadius;
        this.targetCaptureAdditiveRadius = targetCaptureAdditiveRadius;
        this.targetCaptureAngle = targetCaptureAngle;
        this.noiseCaptureRadius = noiseCaptureRadius;
        this.maxNoiseQueueSize = maxNoiseQueueSize;
        this.additiveAttackRadius = additiveAttackRadius;
        this.attackRange = attackRange;
        this.attackCircleRadius = attackCircleRadius;
        this.preAttackDelay = preAttackDelay;
        this.delayForNextAttack = delayForNextAttack;
        this.pathFindDelay = pathFindDelay;
    }
}

public class ZombieCreater : LifeCreater
{
    public ZombieCreater(BaseLife lifePrefab, LifeData lifeData) : base(lifePrefab, lifeData)
    {
    }

    public override BaseLife Create()
    {
        BaseLife life = Object.Instantiate(_lifePrefab);
        ZombieData data = _lifeData as ZombieData;
        life.ResetData(data);
        life.Initialize();
        return life;
    }
}
