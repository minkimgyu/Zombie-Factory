using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LifeState
{
    Alive,
    Die
}

public class Player1 : BaseLife
{
    //[SerializeField] Transform _sightPoint;
    //[SerializeField] Animator _ownerAnimator;

    //InteractionController _interactionController;
    //ActionController _actionController;
    //WeaponController _weaponController;
    //Commander _commander;

    //public enum EventState
    //{
    //    Enable,
    //    Disable
    //}

    //EventState _state;

    //float _viewYRange = 70;
    //SerializableVector2 _viewSensitivity = new SerializableVector2(1000, 1000);

    //int _helperCount = 3;
    //float _startRange = 5f;

    //float _weaponThrowPower = 5;

    //float _walkSpeed = 80;
    //float _walkSpeedOnAir = 40;
    //float _jumpSpeed = 20;

    //float _postureSwitchDuration = 1f;
    //float _capsuleStandCenter = 1f;
    //float _capsuleCrouchHeight = 1.7f;

    //float _capsuleStandHeight = 2f;
    //float _capsuleCrouchCenter = 1.15f;

    //public override void ResetData(PlayerData data)
    //{
    //    _maxHp = data.maxHp;
    //    _viewYRange = data.viewYRange;
    //    _viewSensitivity = data.viewSensitivity;

    //    _helperCount = data.helperCount;
    //    _startRange = data.startRange;

    //    _weaponThrowPower = data.weaponThrowPower;

    //    _walkSpeed = data.walkSpeed;
    //    _walkSpeedOnAir = data.walkSpeedOnAir;
    //    _jumpSpeed = data.jumpSpeed;

    //    _postureSwitchDuration = data.postureSwitchDuration;
    //    _capsuleStandCenter = data.capsuleStandCenter;
    //    _capsuleStandHeight = data.capsuleStandHeight;

    //    _capsuleCrouchCenter = data.capsuleCrouchCenter;
    //    _capsuleCrouchHeight = data.capsuleCrouchHeight;
    //}



    //public override void Initialize()
    //{
    //    base.Initialize();

    //    _state = EventState.Enable;
    //    HpViewer hpViwer = FindObjectOfType<HpViewer>();
    //    RoundViwer roundViwer = FindObjectOfType<RoundViwer>();

    //    //(state) => {_lifeFsm.SetState(state); }, hpViwer.OnHpChange)

    //    //_lifeFsm.Initialize(
    //    //      new Dictionary<LifeState, BaseState>
    //    //      {
    //    //            {LifeState.Alive, new AliveState(data.maxHp, (state) => {_lifeFsm.SetState(state); }, hpViwer.OnHpChange)},
    //    //            {LifeState.Die, new ExitGameState() },
    //    //      }
    //    //   );
    //    //_lifeFsm.SetState(LifeState.Alive);

    //    _interactionController = GetComponentInChildren<InteractionController>();
    //    _interactionController.Initialize();
    //    InputHandler.AddInputEvent(InputHandler.Type.Interact, new Command(_interactionController.OnHandleInteract));


    //    //Action<BaseWeapon.Name, BaseWeapon.Type> AddWeaponPreview;
    //    //Action<BaseWeapon.Type> RemoveWeaponPreview;

    //    //Shop shop = FindObjectOfType<Shop>();
    //    //shop.AddProfileViewer(data.personName, out AddWeaponPreview, out RemoveWeaponPreview);

    //    WeaponUIController weaponViewer = FindObjectOfType<WeaponUIController>();

    //    ZoomComponent zoomComponent = GetComponent<ZoomComponent>();

    //    //_weaponController = GetComponent<WeaponController>();
    //    //_weaponController.Initialize(
    //    //    _weaponThrowPower,
    //    //    false,
    //    //    zoomComponent.OnZoomCalled,
    //    //    (name, layer, nomalizedTime) => _ownerAnimator.Play(name, layer, nomalizedTime),
    //    //    roundViwer.OnRoundCountChange,
    //    //    null,
    //    //    null,
    //    //    null
    //    //    //(BaseWeapon.Name name, BaseWeapon.Type type) => { weaponViewer.AddPreview(name, type); AddWeaponPreview?.Invoke(name, type); },
    //    //    //(BaseWeapon.Type type) => { weaponViewer.RemovePreview(type); RemoveWeaponPreview?.Invoke(type); }
    //    //);

    //    _weaponController.OnHandleEquip(BaseWeapon.Type.Sub);


    //    _actionController = GetComponent<ActionController>();
    //    _actionController.Initialize(_walkSpeed, _walkSpeedOnAir, _jumpSpeed,
    //        _postureSwitchDuration, _capsuleStandCenter, _capsuleStandHeight,
    //        _capsuleCrouchCenter, _capsuleCrouchHeight, _viewYRange, _viewSensitivity.V2);

    //    _commander = GetComponent<Commander>();
    //    _commander.Initialize(ReturnPosition, _startRange);

    //    //if (shop == null) return;

    //    //// 상점에 Event를 등록시켜준다.
    //    //shop.AddEvent(Shop.EventType.BuyWeapon, new WeaponCommand(_weaponController.OnWeaponReceived));

    //    //shop.AddEvent(Shop.EventType.BuyWeaponToHelper, new WeaponToHelperCommand(_commander.BuyWeaponToListener));


    //    //// 이 둘은 조력자와 플레이어 모두 적용시켜주기
    //    //shop.AddEvent(Shop.EventType.BuyHealPack, new HealCommand((hp) => { _lifeFsm.OnHeal(hp); _commander.HealListeners(hp); }));
    //    //shop.AddEvent(Shop.EventType.BuyAmmo, new Command(() => { _weaponController.RefillAmmo(); _commander.RefillAmmoToListeners(); }));
    //}

    //void AddInputEvent()
    //{
    //    InputHandler.AddInputEvent(InputHandler.Type.Equip, new EquipCommand(_weaponController.OnHandleEquip));
    //    InputHandler.AddInputEvent(InputHandler.Type.EventStart, new EventCommand(_weaponController.OnHandleEventStart));
    //    InputHandler.AddInputEvent(InputHandler.Type.EventEnd, new Command(_weaponController.OnHandleEventEnd));

    //    InputHandler.AddInputEvent(InputHandler.Type.Reload, new Command(_weaponController.OnHandleReload));
    //    InputHandler.AddInputEvent(InputHandler.Type.Drop, new Command(_weaponController.OnHandleDrop));

    //    InputHandler.AddInputEvent(InputHandler.Type.Sit, new Command(_actionController.OnHandleSit));
    //    InputHandler.AddInputEvent(InputHandler.Type.Stand, new Command(_actionController.OnHandleStand));

    //    InputHandler.AddInputEvent(InputHandler.Type.Jump, new Command(_actionController.OnHandleJump));
    //    InputHandler.AddInputEvent(InputHandler.Type.Stop, new Command(_actionController.OnHandleStop));
    //    InputHandler.AddInputEvent(InputHandler.Type.Walk, new MoveCommand(_actionController.OnHandleMove));



    //    InputHandler.AddInputEvent(InputHandler.Type.FreeRole, new Command(_commander.FreeRole));
    //    InputHandler.AddInputEvent(InputHandler.Type.BuildFormation, new Command(_commander.BuildFormation));

    //    InputHandler.AddInputEvent(InputHandler.Type.PickUpWeapon, new Command(_commander.PickUpWeapon));
    //    InputHandler.AddInputEvent(InputHandler.Type.SetPriorityTarget, new Command(_commander.SetPriorityTarget));

    //    InputHandler.AddInputEvent(InputHandler.Type.TurnOnOffPlayerRoutine, new Command(TurnOnOffRoutine));
    //}


    //public bool _isActive = false;

    //void TurnOnOffRoutine()
    //{
    //    if (_isActive == false) return;

    //    switch (_state)
    //    {
    //        case EventState.Enable:
    //            _state = EventState.Disable;
    //            _actionController.TurnOnOffInput();
    //            _weaponController.TurnOnOffInput();

    //            break;
    //        case EventState.Disable:
    //            _state = EventState.Enable;
    //            _actionController.TurnOnOffInput();
    //            _weaponController.TurnOnOffInput();

    //            break;
    //    }
    //}

    //private void Update()
    //{
    //    if (_state == EventState.Disable) return;

    //    _actionController.OnUpdate();
    //    _weaponController.OnUpdate();
    //}

    //private void FixedUpdate()
    //{
    //    if (_state == EventState.Disable) return;

    //    _actionController.OnFixedUpdate();
    //}

    //private void LateUpdate()
    //{
    //    _actionController.OnLateUpdate();
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    _actionController.OnCollisionEnterRequested(collision);
    //}
    ////public void GetDamage(float damage)
    ////{
    ////    _lifeFsm.OnDamaged(damage);
    ////}

    ////public Vector3 GetFowardVector()
    ////{
    ////    return transform.forward;
    ////}

    ////public Vector3 ReturnPos()
    ////{
    ////    return transform.position;
    ////}
    ////public Transform ReturnTransform()
    ////{
    ////    return transform;
    ////}

    ////public Transform ReturnSightPoint()
    ////{
    ////    return _sightPoint;
    ////}

    ////public bool IsUntrackable()
    ////{
    ////    return false;
    ////}
}
