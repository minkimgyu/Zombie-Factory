using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Animations.Rigging;

public class WeaponController : MonoBehaviour
{
    [SerializeField] FirePoint _firePoint;
    [SerializeField] Transform _weaponParent;
    float _weaponThrowPower;

    BaseWeapon _nowEquipedWeapon = null;
    Dictionary<BaseWeapon.Type, BaseWeapon> _weaponsContainer;

    WeaponBlackboard _eventBlackboard;

    public enum State
    {
        Idle,
        Equip,
        LeftAction,
        RightAction,
        Reload,

        Root,
        Drop
    }

    WeaponFSM _weaponFSM;
    public Action<float> OnWeaponWeightChangeRequested;

    Action<BaseItem.Name> OnProfileChangeRequested;

    Action<BaseItem.Name, BaseWeapon.Type> AddPreview;
    Action<BaseWeapon.Type> RemovePreview;

    Rigidbody _rigidbody;
    public float SendMoveDisplacement() { return _rigidbody.velocity.magnitude * 0.01f; }

    bool _applyTPSAnimation; // TPS 모델인 경우

    Animator _animator;

    // Player
    public void Initialize(float weaponThrowPower) // OnShowRounds --> 이거는 이벤트 버스로 구현해주자
    {
        _weaponThrowPower = weaponThrowPower;
        _applyTPSAnimation = false;

        _weaponsContainer = new Dictionary<BaseWeapon.Type, BaseWeapon>();
        _rigidbody = GetComponent<Rigidbody>();

        _animator = GetComponentInChildren<Animator>();
        //ReloadIKComponent reloadIKComponent = GetComponentInChildren<ReloadIKComponent>();
        ZoomComponent zoomComponent = GetComponent<ZoomComponent>();
        zoomComponent.Initialize();

        ViewComponent viewComponent = GetComponent<ViewComponent>();

        _eventBlackboard = new WeaponBlackboard(
            zoomComponent.OnZoomCalled,
            SendMoveDisplacement,
            viewComponent.OnRecoilRequested,
            PlayOwnerAnimation,
            _firePoint
        );

        InitializeFSM();
    }

    void PlayOwnerAnimation(string name, int index, float time)
    {
        Debug.Log(name);
        Debug.Log(index);
        Debug.Log(time);
        _animator.Play(name, index, time);
    }

    //public void RefillAmmo()
    //{
    //    foreach (var weapon in _weaponsContainer)
    //    {
    //        weapon.Value.RefillAmmo();
    //    }
    //}

    public void AddWeapon(BaseWeapon weapon)
    {
        weapon.transform.SetParent(_weaponParent);
        weapon.OnRooting(_eventBlackboard);
        AddWeaponToContainer(weapon);
        _weaponFSM.SetState(State.Equip, weapon.WeaponType, "Equip First Weapon");
    }

    //public bool IsAmmoEmpty() 
    //{
    //    if (_nowEquipedWeapon == null) return false;

    //    return _nowEquipedWeapon.IsAmmoEmpty(); 
    //}

    BaseWeapon ReturnWeapon() { return _nowEquipedWeapon; }

    void ChangeWeapon(BaseWeapon weapon) { _nowEquipedWeapon = weapon; }

    // InteractionController에서 받아온 경우
    public void OnWeaponReceived(BaseWeapon weapon) => _weaponFSM.OnWeaponReceived(weapon);

    void AddWeaponToContainer(BaseWeapon weapon) 
    {
        // 플레이어인 경우 레이어 변경
        if (weapon != null && _applyTPSAnimation == false) weapon.ChangeWeaponLayer(true);

        _weaponsContainer.Add(weapon.WeaponType, weapon);
        AddPreview?.Invoke(weapon.WeaponName, weapon.WeaponType);
    }

    public BaseWeapon ReturnSameTypeWeapon(BaseWeapon.Type type) 
    {
        if (_weaponsContainer.ContainsKey(type) == false) return null;
        return _weaponsContainer[type]; 
    }

    ///
    public void OnHandleEquip(BaseWeapon.Type type)
    {
        _weaponFSM.OnHandleEquip(type);
    }
    public void OnHandleEventStart(BaseWeapon.EventType type)
    {
        _weaponFSM.OnHandleEventStart(type);
    }
    public void OnHandleEventEnd(BaseWeapon.EventType type)
    {
        _weaponFSM.OnHandleEventEnd(type);
    }

    public void OnHandleReload()
    {
        _weaponFSM.OnHandleReload();
    }
    public void OnHandleDrop()
    {
        _weaponFSM.OnHandleDrop();
    }

    void InitializeFSM()
    {
        _weaponFSM = new WeaponFSM();
        Dictionary<State, BaseState<State>> weaponStates = new Dictionary<State, BaseState<State>>
        {
            { State.Idle, new IdleState(_weaponFSM, _weaponsContainer, ReturnWeapon) },
            { State.Equip, new EquipState(_weaponFSM, _weaponsContainer, 
                OnWeaponWeightChangeRequested, OnProfileChangeRequested, ChangeWeapon, ReturnWeapon) 
            },
            { State.Reload, new ReloadState(_weaponFSM, _applyTPSAnimation, ChangeWeapon, ReturnWeapon) },

            { State.LeftAction, new LeftActionState(_weaponFSM, ReturnWeapon) },
            { State.RightAction, new RightActionState(_weaponFSM, ReturnWeapon) },
            { State.Root, new RootState(_weaponFSM, _weaponsContainer, _weaponParent, _eventBlackboard, ReturnWeapon) },
            { State.Drop, new DropState(_weaponFSM, _weaponThrowPower, _weaponsContainer, _eventBlackboard, ReturnWeapon, ChangeWeapon) },
        };

        _weaponFSM.Initialize(weaponStates);
        _weaponFSM.SetState(State.Idle);
    }

    public void OnUpdate()
    {
        _weaponFSM.OnUpdate();
        foreach (var weapon in _weaponsContainer) weapon.Value.OnUpdate(); // 무기 루틴 돌려주기
    }
}