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
    //public Action<float> OnWeaponWeightChangeRequested;

    //Action<BaseItem.Name> OnProfileChangeRequested;

    protected Action<bool> ActiveAmmoViewer;
    protected Action<int, int> UpdateAmmoViewer;

    protected Action<BaseItem.Name, BaseWeapon.Type> AddPreview;
    protected Action<BaseWeapon.Type> RemovePreview;

    Rigidbody _rigidbody;
    public float SendMoveDisplacement() { return _rigidbody.velocity.magnitude * 0.01f; }

    bool _applyTPSAnimation; // TPS 모델인 경우

    Animator _animator;

    // OnShowRounds --> 이거는 이벤트 버스로 구현해주자

    public void AddObserverEvent(
        Action<bool> ActiveAmmoViewer,
        Action<int, int> UpdateAmmoViewer,
        Action<BaseItem.Name, BaseWeapon.Type> AddPreview,
        Action<BaseWeapon.Type> RemovePreview)
    {
        this.ActiveAmmoViewer = ActiveAmmoViewer;
        this.UpdateAmmoViewer = UpdateAmmoViewer;
        this.AddPreview = AddPreview;
        this.RemovePreview = RemovePreview;
    }

    public void Initialize(float weaponThrowPower)
    {
        _weaponThrowPower = weaponThrowPower;
        _applyTPSAnimation = false;

        _weaponsContainer = new Dictionary<BaseWeapon.Type, BaseWeapon>();
        _rigidbody = GetComponent<Rigidbody>();

        _animator = GetComponentInChildren<Animator>();
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

        _weaponFSM = new WeaponFSM();
        Dictionary<State, BaseState<State>> weaponStates = new Dictionary<State, BaseState<State>>
        {
            { State.Idle, new IdleState(_weaponFSM, _weaponsContainer, ReturnWeapon) },
            { State.Equip, new EquipState(_weaponFSM, _weaponsContainer, ChangeWeapon, ReturnWeapon)
            },
            { State.Reload, new ReloadState(_weaponFSM, _applyTPSAnimation, ChangeWeapon, ReturnWeapon) },

            { State.LeftAction, new LeftActionState(_weaponFSM, ReturnWeapon) },
            { State.RightAction, new RightActionState(_weaponFSM, ReturnWeapon) },
            { State.Root, new RootState(_weaponFSM, _weaponsContainer, _weaponParent, _eventBlackboard, ReturnWeapon, AddPreview) }, // --> 여기 내부에 이벤트를 넣자
            { State.Drop, new DropState(_weaponFSM, _weaponThrowPower, _weaponsContainer, _eventBlackboard, ReturnWeapon, ChangeWeapon, RemovePreview) },
        };

        _weaponFSM.Initialize(weaponStates);
        _weaponFSM.SetState(State.Idle);
    }

    void PlayOwnerAnimation(string name, int index, float time)
    {
        _animator.Play(name, index, time);
    }

    BaseWeapon ReturnWeapon() { return _nowEquipedWeapon; }

    void ChangeWeapon(BaseWeapon weapon) { _nowEquipedWeapon = weapon; }

    // InteractionController에서 받아온 경우
    public void OnWeaponReceived(BaseWeapon weapon)
    {
        _weaponFSM.OnWeaponReceived(weapon);
    }

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

    public void OnUpdate()
    {
        _weaponFSM.OnUpdate();
        foreach (var weapon in _weaponsContainer) weapon.Value.OnUpdate(); // 무기 루틴 돌려주기
    }
}