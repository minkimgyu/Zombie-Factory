using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseLife
{
    float _viewYRange;
    SerializableVector2 _viewSensitivity;

    float _weaponThrowPower;

    float _walkSpeed;
    float _runSpeed;
    float _walkSpeedOnAir;
    float _jumpSpeed;

    float _postureSwitchDuration;
    float _capsuleStandCenter;
    float _capsuleCrouchHeight;

    float _capsuleStandHeight;
    float _capsuleCrouchCenter;

    ActionController _actionController;
    WeaponController _weaponController;
    InteractionController _interactionController;

    public override void AddObserverEvent
    (
        Action<Vector3, Vector3> MoveCamera,
        Action<float, float> OnFieldOfViewChange,

        Action<float> OnHpChangeRequested,
        Action<bool> SwitchCrosshair,

        Action<bool> ActiveAmmoViewer,
        Action<int, int> UpdateAmmoViewer,
        Action<BaseItem.Name, BaseWeapon.Type> AddPreview,
        Action<BaseWeapon.Type> RemovePreview)
    {
        this.OnHpChangeRequested = OnHpChangeRequested;
        _actionController.AddObserverEvent(MoveCamera, OnFieldOfViewChange, SwitchCrosshair);
        _weaponController.AddObserverEvent(ActiveAmmoViewer, UpdateAmmoViewer, AddPreview, RemovePreview);
    }

    public override void ResetData(PlayerData data, BaseFactory effectFactory)
    {
        _effectFactory = effectFactory;
        _maxHp = data.maxHp;
        _myType = data.type;

        _viewYRange = data.viewYRange;
        _viewSensitivity = data.viewSensitivity;

        _weaponThrowPower = data.weaponThrowPower;
        _walkSpeed = data.walkSpeed;
        _walkSpeedOnAir= data.walkSpeedOnAir;
        _jumpSpeed = data.jumpSpeed;

        _postureSwitchDuration = data.postureSwitchDuration;

        _capsuleCrouchCenter = data.capsuleCrouchCenter;
        _capsuleCrouchHeight = data.capsuleCrouchHeight;

        _capsuleStandCenter = data.capsuleStandCenter;
        _capsuleStandHeight = data.capsuleStandHeight;
    }

    public override void AddWeapon(BaseWeapon weapon)
    {
        _weaponController.OnWeaponReceived(weapon);
    }

    public override void Initialize()
    {
        _actionController = GetComponent<ActionController>();
        _actionController.Initialize(_walkSpeed, _runSpeed, _walkSpeedOnAir, _jumpSpeed, _postureSwitchDuration,
            _capsuleStandCenter, _capsuleStandHeight, _capsuleCrouchCenter, _capsuleCrouchHeight, _viewYRange, _viewSensitivity.V2);

        _weaponController = GetComponent<WeaponController>();
        _weaponController.Initialize(_weaponThrowPower);


        _interactionController = GetComponent<InteractionController>();
        _interactionController.Initialize();

        IInputable inputable = ServiceLocater.ReturnInputHandler();

        inputable.AddEvent(IInputable.Type.View, new ViewCommand(_actionController.OnHandleView));

        inputable.AddEvent(IInputable.Type.Jump, new KeyCommand(_actionController.OnHandleJump));
        inputable.AddEvent(IInputable.Type.Move, new MoveCommand(_actionController.OnHandleMove));

        inputable.AddEvent(IInputable.Type.CrouchStart, new KeyCommand(_actionController.OnHandleSit));
        inputable.AddEvent(IInputable.Type.CrouchEnd, new KeyCommand(_actionController.OnHandleStand));

        inputable.AddEvent(IInputable.Type.RunStart, new KeyCommand(_actionController.OnHandleRunStart));
        inputable.AddEvent(IInputable.Type.RunEnd, new KeyCommand(_actionController.OnHandleRunEnd));

        inputable.AddEvent(IInputable.Type.Equip, new EquipCommand(_weaponController.OnHandleEquip));
        inputable.AddEvent(IInputable.Type.Reload, new KeyCommand(_weaponController.OnHandleReload));
        inputable.AddEvent(IInputable.Type.Drop, new KeyCommand(_weaponController.OnHandleDrop));

        inputable.AddEvent(IInputable.Type.EventStart, new InputEventCommand(_weaponController.OnHandleEventStart));
        inputable.AddEvent(IInputable.Type.EventEnd, new InputEventCommand(_weaponController.OnHandleEventEnd));
    }

    private void OnCollisionEnter(Collision collision)
    {
        _actionController.OnCollisionEnterRequested(collision);
    }

    private void Update()
    {
        _actionController.OnUpdate();
        _weaponController.OnUpdate();
    }

    private void FixedUpdate()
    {
        _actionController.OnFixedUpdate();
    }

    private void LateUpdate()
    {
        _actionController.OnLateUpdate();
    }
}
