using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseWeapon : BaseItem
{
    public enum Type
    {
        Main,
        Sub,
        Melee
    }

    public enum EventType
    {
        Main,
        Sub
    }

    protected float _range;

    [SerializeField] protected Name _weaponName;
    [SerializeField] protected Type _weaponType;

    public Name WeaponName { get { return _weaponName; } }
    public Type WeaponType { get { return _weaponType; } }

    protected int _targetLayer; // ���� ��� ���̾�
    protected Transform _attackPoint; // ���� Raycast ���� ��ġ

    protected Dictionary<EventType, EventState> _eventStates = new Dictionary<EventType, EventState>();
    protected Dictionary<EventType, ActionState> _actionStates = new Dictionary<EventType, ActionState>();
    protected Dictionary<EventType, BaseRecoilState> _recoilStates = new Dictionary<EventType, BaseRecoilState>();
    protected BaseReloadState _reloadState;

    protected float _equipFinishTime;
    public float EquipFinishTime { get { return _equipFinishTime; } }

    ///// <summary>
    ///// ������ �ִϸ��̼��� �����ų �� ȣ��
    ///// </summary>
    //protected Action<string, int, float> OnPlayWeaponAnimation;
    protected Animator _animator;

    protected float _weaponWeight = 1;
    public float SlowDownRatioByWeaponWeight { get { return 1.0f / _weaponWeight; } }

    protected Action<string, int, float> OnPlayOwnerAnimation;

    public virtual void RefillAmmo(int ammoCount) { }

    void ChangeChildLayer(Transform child, int layer)
    {
        //child.gameObject.layer = layer;

        for (int i = 0; i < child.childCount; i++)
        {
            Transform childTransform = child.GetChild(i);
            ChangeChildLayer(childTransform, layer);
        }
    }

    public void ChangeWeaponLayer(bool changeLayer)
    {
        int layer;
        if (changeLayer) layer = LayerMask.NameToLayer("Weapons");
        else layer = LayerMask.NameToLayer("Default");

        ChangeChildLayer(transform, layer);
    }

    public override void Initialize()
    {
        _animator = GetComponent<Animator>();
        _targetLayer = LayerMask.GetMask("Penetratable", "Block", "NonPass"); // ���̾� �Ҵ����ش�.
    }

    public virtual void MatchState() { }

    // ���� �� ������ֱ�


    public void OnUpdate() // --> WeaponFSM�� �����������
    {
        foreach (var events in _eventStates) events.Value.OnUpdate();
        foreach (var actions in _actionStates) actions.Value.OnUpdate();
        foreach (var recoils in _recoilStates) recoils.Value.OnUpdate();
        _reloadState.OnUpdate();
    }

    protected virtual void OnCollisionEnter(Collision collision) { }

    public virtual void OnEquip()
    {
        PlayAnimation("Equip");
    }

    protected virtual void PlayAnimation(string name)
    {
        _animator.Play(name, 0, 0);
        OnPlayOwnerAnimation?.Invoke(_weaponName + name, 0, 0);
    }

    public virtual void OnUnEquip()
    {
        foreach (var action in _actionStates) action.Value.TurnOffZoomDirectly();
    }


    public virtual void OnRooting(WeaponBlackboard blackboard)
    {
        _attackPoint = blackboard.AttackPoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        gameObject.SetActive(false);

        OnPlayOwnerAnimation = blackboard.OnPlayOwnerAnimation;
        foreach (var actions in _actionStates) actions.Value.LinkEvent(blackboard);
        foreach (var recoils in _recoilStates) recoils.Value.LinkEvent(blackboard);
        _reloadState.LinkEvent(blackboard);
    }

    public virtual bool CanDrop() { return false; }

    public virtual void ThrowWeapon(float force) { }

    /// <summary>
    /// ���⿡�� ���� ���� ��, �ʿ� ���� ������ �̺�Ʈ�� �������ش�.
    /// </summary>
    public virtual void OnDrop(WeaponBlackboard blackboard)
    {
        OnPlayOwnerAnimation = null;
        foreach (var action in _actionStates) action.Value.UnlinkEvent(blackboard);
        foreach (var recoil in _recoilStates) recoil.Value.UnlinkEvent(blackboard);
        _reloadState.UnlinkEvent(blackboard);
        transform.SetParent(null);
    }

    public virtual bool IsAmmoEmpty() { return true; }

    public virtual bool CanAutoReload() { return false; }

    // ���ο� �������� �־ �������ֱ�
    public virtual bool CanReload() { return false; }

    // ���� ���� �� ȣ��
    public virtual void OnReloadStart(bool isTPS) { }

    public virtual void OnReloadEnd() { }

    //public virtual void RefillAmmo() { }

    // ���� �ϴ� ���߿� ���콺 �Է��� ���� ���� ĵ��
    public virtual bool CanCancelReloadAndGoToMainAction() { return default; }

    // ���� �ϴ� ���߿� ���콺 �Է��� ���� ���� ĵ��
    public virtual bool CanCancelReloadAndGoToSubAction() { return default; }

    public virtual bool IsReloadFinish() { return default; } // �������� ���� ���

    public virtual void ResetReload() { } // �������� ����� ���



    protected virtual bool CanAttack(EventType type) { return _actionStates[type].CanExecute(); }


    // WeaponController���� ȣ��Ǵ� �Է� �̺�Ʈ
    public void OnLeftClickStart() => _eventStates[EventType.Main].OnMouseClickStart();
    public void OnLeftClickProcess() => _eventStates[EventType.Main].OnMouseClickProcess();
    public void OnLeftClickEnd() => _eventStates[EventType.Main].OnMouseClickEnd();

    public void OnRightClickStart() => _eventStates[EventType.Sub].OnMouseClickStart();
    public void OnRightClickProgress() => _eventStates[EventType.Sub].OnMouseClickProcess();
    public void OnRightClickEnd() => _eventStates[EventType.Sub].OnMouseClickEnd();

    /// <summary>
    /// ���콺 Ŭ�� �̺�Ʈ�� ���۵� �� ȣ���
    /// </summary>
    protected virtual void OnEventStart(EventType type) { }

    /// <summary>
    /// ���콺 Ŭ�� �̺�Ʈ ���� ��� ȣ���
    /// </summary>
    protected virtual void OnEventUpdate(EventType type) { }

    /// <summary>
    /// ���콺 Ŭ�� �̺�Ʈ�� ������ ��� ȣ���
    /// </summary>
    protected virtual void OnEventEnd(EventType type) { }

    /// <summary>
    /// Action �̺�Ʈ�� ȣ���
    /// </summary>
    protected virtual void OnAction(EventType type)
    {
        if (CanAttack(type) == false) return;

        _actionStates[type].Execute();
        _recoilStates[type].Execute();
    }
}