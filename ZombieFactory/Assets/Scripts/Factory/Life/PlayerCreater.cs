using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using System;

[System.Serializable]
public class CharacterData
{
    public float maxHp;
}

[System.Serializable]
public class PlayerData : LifeData
{
    public float viewYRange;
    public SerializableVector2 viewSensitivity;

    public float weaponThrowPower;

    public float walkSpeed;
    public float runSpeed;
    public float walkSpeedOnAir;
    public float jumpSpeed;

    public float postureSwitchDuration;
    public float capsuleStandCenter;
    public float capsuleCrouchHeight;

    public float capsuleStandHeight;
    public float capsuleCrouchCenter;

    public PlayerData(
        float maxHp, 

        float viewYRange, 
        SerializableVector2 viewSensitivity, 
        float weaponThrowPower, 
        float walkSpeed, 
        float runSpeed, 
        float walkSpeedOnAir, 
        float jumpSpeed, 
        float postureSwitchDuration, 
        float capsuleStandCenter, 
        float capsuleCrouchHeight, 
        float capsuleStandHeight, 
        float capsuleCrouchCenter) : base(maxHp)
    {
        this.viewYRange = viewYRange;
        this.viewSensitivity = viewSensitivity;
        this.weaponThrowPower = weaponThrowPower;
        this.walkSpeed = walkSpeed;
        this.runSpeed = runSpeed;
        this.walkSpeedOnAir = walkSpeedOnAir;
        this.jumpSpeed = jumpSpeed;
        this.postureSwitchDuration = postureSwitchDuration;
        this.capsuleStandCenter = capsuleStandCenter;
        this.capsuleCrouchHeight = capsuleCrouchHeight;
        this.capsuleStandHeight = capsuleStandHeight;
        this.capsuleCrouchCenter = capsuleCrouchCenter;
    }
}

public class PlayerCreater : LifeCreater
{
    BaseFactory _effectFactory;
    HelperMediator _mediator;

    public PlayerCreater(
        BaseLife lifePrefab,
        HelperMediator mediator,
        LifeData lifeData,
        BaseFactory effectFactory) : base(lifePrefab, lifeData) 
    {
        _mediator = mediator;
        _effectFactory = effectFactory;
    }

    public override BaseLife Create(List<BaseItem.Name> weaponNames)
    {
        BaseLife life = Object.Instantiate(_lifePrefab);

        PlayerData playerData = _lifeData as PlayerData;
        life.ResetData(playerData, _mediator, _effectFactory);
        life.Initialize();
        life.InitializeFSM();
        return life;
    }
}
