using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
using UnityEditor;
using AI.Swat;

[System.Serializable]
public class SwatData : LifeData
{
    public BaseLife.Name name;
    public float destoryDelay = 5;

    public float angleOffset = 90;
    public float angleChangeAmount = 0.01f;

    public float weaponThrowPower = 5;

    public int wanderOffset = 5;
    public float stateChangeDelay = 5;

    public float moveSpeed = 3;
    public float viewSpeed = 5;

    public float largeTargetCaptureRadius = 8;
    public float largeTargetCaptureAngle = 180;

    public float smallTargetCaptureRadius = 3;
    public float smallTargetCaptureAngle = 360; // 이건 전 범위로 해줘야 한다.

    public float pathFindDelay = 0.5f;

    public float farFromPlayerDistance = 9;
    public float farFromPlayerDistanceOffset = 6;

    public float farFromTargetDistance = 6;
    public float farFromTargetDistanceOffset = 1;

    public float closeDistance = 2.5f;
    public float closeDistanceOffset = 0.5f;

    public float attackDelay = 3;
    public float attackDuration = 1;

    public float formationRadius = 10;

    public float formationOffset = 3;
    public float formationOffsetChangeDuration = 5;

    public string ragdollName;

    public SwatData(
        float maxHp, 

        BaseLife.Name name, 
        float destoryDelay, 
        float angleOffset, 
        float angleChangeAmount, 
        float weaponThrowPower, 
        int wanderOffset, 
        float stateChangeDelay, 
        float moveSpeed, 
        float viewSpeed, 
        float largeTargetCaptureRadius, 
        float largeTargetCaptureAngle, 
        float smallTargetCaptureRadius, 
        float smallTargetCaptureAngle, 
        float pathFindDelay, 
        float farFromPlayerDistance, 
        float farFromPlayerDistanceOffset, 
        float farFromTargetDistance, 
        float farFromTargetDistanceOffset, 
        float closeDistance, 
        float closeDistanceOffset, 
        float attackDelay, 
        float attackDuration, 
        float formationRadius, 
        float formationOffset, 
        float formationOffsetChangeDuration, 
        string ragdollName) : base(maxHp)
    {
        this.name = name;
        this.destoryDelay = destoryDelay;
        this.angleOffset = angleOffset;
        this.angleChangeAmount = angleChangeAmount;
        this.weaponThrowPower = weaponThrowPower;
        this.wanderOffset = wanderOffset;
        this.stateChangeDelay = stateChangeDelay;
        this.moveSpeed = moveSpeed;
        this.viewSpeed = viewSpeed;
        this.largeTargetCaptureRadius = largeTargetCaptureRadius;
        this.largeTargetCaptureAngle = largeTargetCaptureAngle;
        this.smallTargetCaptureRadius = smallTargetCaptureRadius;
        this.smallTargetCaptureAngle = smallTargetCaptureAngle;
        this.pathFindDelay = pathFindDelay;
        this.farFromPlayerDistance = farFromPlayerDistance;
        this.farFromPlayerDistanceOffset = farFromPlayerDistanceOffset;
        this.farFromTargetDistance = farFromTargetDistance;
        this.farFromTargetDistanceOffset = farFromTargetDistanceOffset;
        this.closeDistance = closeDistance;
        this.closeDistanceOffset = closeDistanceOffset;
        this.attackDelay = attackDelay;
        this.attackDuration = attackDuration;
        this.formationRadius = formationRadius;
        this.formationOffset = formationOffset;
        this.formationOffsetChangeDuration = formationOffsetChangeDuration;
        this.ragdollName = ragdollName;
    }
}

public class HelperCreater : LifeCreater
{
    BaseFactory _effectFactory;
    BaseFactory _itemFactory;
    HelperMediator _mediator;

    public HelperCreater(BaseLife lifePrefab, HelperMediator mediator, BaseFactory itemFactory, BaseFactory effectFactory) : base(lifePrefab, null)
    {
        _effectFactory = effectFactory;
        _itemFactory = itemFactory;
        _mediator = mediator;
    }

    public override BaseLife Create(List<BaseItem.Name> weaponNames) // list로 사용 가능한 무기 받기
    {
        BaseLife life = Object.Instantiate(_lifePrefab);
        life.Initialize();

        IWeaponEquipable equipable = life.GetComponent<IWeaponEquipable>();
        for (int j = 0; j < weaponNames.Count; j++)
        {
            BaseItem item = _itemFactory.Create(weaponNames[j]); // 랜덤한 무기 생성 후 추가
            equipable.AddWeapon(item as BaseWeapon);
        }

        IHelper helper = life.GetComponent<IHelper>();
        _mediator.AddHelper(helper);

        life.InitializeFSM();
        return life;
    }
}
