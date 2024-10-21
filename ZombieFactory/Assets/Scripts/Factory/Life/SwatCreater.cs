using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

[System.Serializable]
public class SwatData : LifeData
{
    public float moveRange;
    public float moveSpeed;
    public float stateChangeDuration;

    public float retreatDistance;

    public float stopDistance;
    public float gap;


    public float farDistance;
    public float closeDistance;

    public float attackDuration;
    public float attackDelay;

    public float weaponThrowPower;

    public float captureRadius;
    public float captureAngle;

    public SwatData(
        float maxHp,

        float moveRange,
        float moveSpeed,
        float stateChangeDuration,
        float retreatDistance,
        float stopDistance,
        float gap,
        float farDistance,
        float closeDistance,
        float attackDuration,
        float attackDelay,
        float weaponThrowPower,

        float captureRadius,
        float captureAngle) : base(maxHp)
    {
        this.moveRange = moveRange;
        this.moveSpeed = moveSpeed;
        this.stateChangeDuration = stateChangeDuration;
        this.retreatDistance = retreatDistance;
        this.stopDistance = stopDistance;
        this.gap = gap;
        this.farDistance = farDistance;
        this.closeDistance = closeDistance;
        this.attackDuration = attackDuration;
        this.attackDelay = attackDelay;
        this.weaponThrowPower = weaponThrowPower;

        this.captureRadius = captureRadius;
        this.captureAngle = captureAngle;
    }
}

public class SwatCreater : LifeCreater
{
    BaseFactory _effectFactory;
    BaseFactory _itemFactory;
    BaseFactory _ragdollFactory;

    HelperMediator _mediator;

    public SwatCreater(
        BaseLife lifePrefab,
        HelperMediator mediator,
        LifeData lifeData,
        BaseFactory itemFactory,
        BaseFactory ragdollFactory,
        BaseFactory effectFactory) : base(lifePrefab, lifeData)
    {
        _effectFactory = effectFactory;
        _itemFactory = itemFactory;
        _ragdollFactory = ragdollFactory;
        _mediator = mediator;
    }

    public override BaseLife Create(List<BaseItem.Name> weaponNames) // list로 사용 가능한 무기 받기
    {
        BaseLife life = Object.Instantiate(_lifePrefab);
        SwatData data = _lifeData as SwatData;

        life.ResetData(data, _effectFactory, _ragdollFactory);
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
