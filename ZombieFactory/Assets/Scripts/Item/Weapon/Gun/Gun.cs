using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class Gun : BaseWeapon, IInteractable
{
    // ��� �� �������� ������ ����
    protected Func<Vector3> ReturnRaycastDir;

    [SerializeField] protected ParticleSystem _muzzleFlash;
    [SerializeField] protected ParticleSystem _emptyCartridgeSpawner;

    [SerializeField] Transform _magOriginPoint; // �ѱ⿡ źâ�� �����ϴ� ���� ��ġ
    [SerializeField] Transform _grapPoint; // �޼��� źâ�� ��� ��ġ
    [SerializeField] Transform _magazineObject; // źâ ������Ʈ

    [SerializeField] Transform _leftHandGripPoint; // �޼� �׸� ����Ʈ

    //protected float _penetratePower = 15;
    protected float _trajectoryLineOffset = 1.3f;

    [SerializeField] protected Transform _muzzle;


    protected int _maxAmmoCountInMagazine;
    protected int _maxAmmoCountsInPossession;

    [SerializeField] protected int _ammoCountsInMagazine;
    [SerializeField] protected int _ammoCountsInPossession;

    [SerializeField] Transform _objectMesh;

    BoxCollider _gunCollider;
    Rigidbody _gunRigidbody;
    bool _nowAttachToGround;

    public Action<bool, string, Vector3> OnViewEventRequest;

    protected Action<Vector3> OnGenerateNoiseRequest;

    protected void SpawnEmptyCartridge() => _emptyCartridgeSpawner.Play();
    protected void SpawnMuzzleFlashEffect() => _muzzleFlash.Play();

    public override bool IsAmmoEmpty()
    {
        return _ammoCountsInMagazine == 0 && _ammoCountsInPossession == 0;
    }

    public void DecreaseAmmoCount(int _fireCountInOnce)
    {
        _ammoCountsInMagazine -= _fireCountInOnce;
        if (_ammoCountsInMagazine < 0) _ammoCountsInMagazine = 0;
    }

    public int ReturnLeftAmmoCount() { return _ammoCountsInMagazine; }

    protected Vector3 ReturnMuzzlePos() { return _muzzle.position; }


    #region Reload

    public void OnReloadRequested(int ammoCountsInMagazine, int ammoCountsInPossession)
    {
        _ammoCountsInMagazine = ammoCountsInMagazine;
        _ammoCountsInPossession = ammoCountsInPossession;
    }

    public override bool CanAutoReload() { return _ammoCountsInMagazine == 0 && _ammoCountsInPossession > 0; }

    public override bool CanReload()
    {
        if (_ammoCountsInPossession <= 0 || _maxAmmoCountInMagazine == _ammoCountsInMagazine) return false;
        else return true;
    }

    public override void OnReloadStart(bool isTPS)
    {
        for (int i = 0; i < _actionStrategies.Count; i++) _actionStrategies[(EventType)i].TurnOffZoomDirectly();
        _reloadStrategy.Execute(isTPS, _ammoCountsInMagazine, _ammoCountsInPossession);
    }

    // ������ ������ ���� �̺�Ʈ ȣ���
    public override void OnReloadEnd()
    {
        OnShowRounds?.Invoke(true, _ammoCountsInMagazine, _ammoCountsInPossession);
    }

    // ���� �ϴ� ���߿� ���콺 �Է��� ���� ���� ĵ��
    public override bool CanCancelReloadAndGoToMainAction() { return _reloadStrategy.CanCancelReloadingByLeftClick(); }

    // ���� �ϴ� ���߿� ���콺 �Է��� ���� ���� ĵ��
    public override bool CanCancelReloadAndGoToSubAction() { return _reloadStrategy.CanCancelReloadingByLeftClick(); }

    public override bool IsReloadFinish() // �������� ���� ���
    {
        return _reloadStrategy.IsReloadFinish();
    }

    public override void ResetReload() // �������� ����� ���
    {
        _reloadStrategy.OnCancelReload();
    }

    #endregion

    #region Drop

    public override bool CanDrop() { return true; }

    public override void ThrowWeapon(float force)
    {
        PositionWeapon(true);
        Vector3 direction = _attackPoint.ReturnDirection();
        _gunRigidbody.AddForce(direction * force, ForceMode.Impulse);
    }

    public override void PositionWeapon(bool nowDrop)
    {
        if (nowDrop)
        {
            _gunCollider.enabled = true;
            _gunRigidbody.isKinematic = false;
        }
        else
        {
            _nowAttachToGround = false;
            _gunCollider.enabled = false;
            _gunRigidbody.isKinematic = true;
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region Equip

    public override void OnEquip()
    {
        base.OnEquip();
        OnShowRounds?.Invoke(true, _ammoCountsInMagazine, _ammoCountsInPossession);

    }

    #endregion;

    //public override void OnRooting(WeaponBlackboard blackboard)
    //{
    //    base.OnRooting(blackboard);
    //    blackboard.AssignIKPoints?.Invoke(_leftHandGripPoint, _magOriginPoint, _grapPoint, _magazineObject); // �ѱ� ������ �ִϸ��̼ǿ� �ʿ��� ���� �Ҵ�
    //}

    public override void Initialize()
    {
        base.Initialize();

        _gunCollider = GetComponent<BoxCollider>();
        _gunRigidbody = GetComponent<Rigidbody>();

        _gunCollider.enabled = false;
        _gunRigidbody.isKinematic = true;

        //WeaponInfoViwer weaponInfoViwer = FindObjectOfType<WeaponInfoViwer>();
        //if (weaponInfoViwer == null) return;
        //OnViewEventRequest = weaponInfoViwer.OnViewEventReceived; // ��� �� ���� �ʿ�
        //NoiseGenerator noiseGenerator = FindObjectOfType<NoiseGenerator>();
        //OnGenerateNoiseRequest = noiseGenerator.GenerateNoise;
    }

    //public override void RefillAmmo() 
    //{
    //    _ammoCountsInMagazine = _maxAmmoCountInMagazine;
    //    _ammoCountsInPossession = _maxAmmoCountsInPossession;

    //    OnShowRounds?.Invoke(true, _ammoCountsInMagazine, _ammoCountsInPossession);
    //}

    protected override void OnAction(EventType type)
    {
        base.OnAction(type);
        OnShowRounds?.Invoke(true, _ammoCountsInMagazine, _ammoCountsInPossession);
    }

    public void OnSightEnter()
    {
        OnViewEventRequest?.Invoke(true, _weaponName.ToString(), _objectMesh.position);
    }

    public void OnSightExit()
    {
        OnViewEventRequest?.Invoke(false, _weaponName.ToString(), _objectMesh.position);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        _nowAttachToGround = true; // ���� �΋H���� �׶����� Interaction ����
    }

    public bool IsInteractable() { return _nowAttachToGround; }

    public T ReturnComponent<T>() { return GetComponent<T>(); }
}