using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponController : MonoBehaviour
{
    [SerializeField] FirePoint _firePoint;
    [SerializeField] Transform _weaponParent;
    float _weaponThrowPower;

    BaseWeapon _nowEquipedWeapon = null;
    Dictionary<BaseWeapon.Type, BaseWeapon> _weaponsContainer;

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

    Rigidbody _rigidbody;
    public float SendMoveDisplacement() { return _rigidbody.velocity.magnitude * 0.01f; }

    bool _applyTPSAnimation; // TPS ���� ���

    Animator _animator;
    WeaponBlackboard _weaponBlackboard;

    public void AddObserverEvent(
        Action<bool> ActiveAmmoViewer,
        Action<int, int> UpdateAmmoViewer,
        Action<BaseItem.Name, BaseWeapon.Type> AddPreview,
        Action<BaseWeapon.Type> RemovePreview)
    {
        _weaponBlackboard = new WeaponBlackboard.Builder(_weaponBlackboard)
        .SetActiveAmmoViewer(ActiveAmmoViewer)
        .SetUpdateAmmoViewer(UpdateAmmoViewer)
        .SetAddPreview(AddPreview)
        .SetRemovePreview(RemovePreview)
        .Build();
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

        FPSViewComponent viewComponent = GetComponent<FPSViewComponent>();

        _weaponBlackboard = new WeaponBlackboard.Builder()
        .SetOnZoomRequested(zoomComponent.OnZoomCalled)
        .SetSendMoveDisplacement(SendMoveDisplacement)
        .SetOnPlayOwnerAnimation(PlayOwnerAnimation)
        .SetOnRecoilRequested(viewComponent.OnRecoilRequested)
        .SetAttackPoint(_firePoint)
        .Build();

        _weaponFSM = new WeaponFSM();
        Dictionary<State, BaseState<State>> weaponStates = new Dictionary<State, BaseState<State>>
        {
            { State.Idle, new IdleState(_weaponFSM, _weaponsContainer, ReturnWeapon) },
            { State.Equip, new EquipState(_weaponFSM, _weaponsContainer, ChangeWeapon, ReturnWeapon)
            },
            { State.Reload, new ReloadState(_weaponFSM, _applyTPSAnimation, ChangeWeapon, ReturnWeapon) },

            { State.LeftAction, new LeftActionState(_weaponFSM, ReturnWeapon) },
            { State.RightAction, new RightActionState(_weaponFSM, ReturnWeapon) },
            { State.Root, new RootState(_weaponFSM, _weaponsContainer, _weaponParent, _weaponBlackboard, ReturnWeapon) }, // --> ���� ���ο� �̺�Ʈ�� ����
            { State.Drop, new DropState(_weaponFSM, _weaponThrowPower, _weaponsContainer, _weaponBlackboard, ReturnWeapon, ChangeWeapon) },
        };

        _weaponFSM.Initialize(weaponStates);
        _weaponFSM.SetState(State.Idle);
    }

    public void RefillAmmo(int ammoCount) 
    {
        _nowEquipedWeapon.RefillAmmo(ammoCount);
    }

    void PlayOwnerAnimation(string name, int index, float time)
    {
        _animator.Play(name, index, time);
    }

    BaseWeapon ReturnWeapon() { return _nowEquipedWeapon; }

    void ChangeWeapon(BaseWeapon weapon) { _nowEquipedWeapon = weapon; }

    // InteractionController���� �޾ƿ� ���
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
        foreach (var weapon in _weaponsContainer) weapon.Value.OnUpdate(); // ���� ��ƾ �����ֱ�
    }
}