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
    //bool _nowAttachToGround;

    //public Action<bool, string, Vector3> OnViewEventRequest;

    //protected Action<Vector3> OnGenerateNoiseRequest;

    protected void SpawnEmptyCartridge() => _emptyCartridgeSpawner.Play();
    protected void SpawnMuzzleFlashEffect() => _muzzleFlash.Play();

    Action<bool> ActiveAmmoViewer;
    Action<int, int> UpdateAmmoViewer;

    public override void RefillAmmo(int ammoCount) 
    {
        int refillCount = _ammoCountsInPossession + ammoCount;
        if (refillCount > _maxAmmoCountsInPossession) refillCount = _maxAmmoCountsInPossession;

        OnReloadRequested(_ammoCountsInMagazine, refillCount);
    }

    public override void OnRooting(WeaponBlackboard blackboard)
    {
        base.OnRooting(blackboard);
        ActiveAmmoViewer = blackboard.ActiveAmmoViewer;
        UpdateAmmoViewer = blackboard.UpdateAmmoViewer;
    }

    public override bool IsAmmoEmpty()
    {
        return _ammoCountsInMagazine == 0 && _ammoCountsInPossession == 0;
    }

    public void DecreaseAmmoCount(int _fireCountInOnce)
    {
        _ammoCountsInMagazine -= _fireCountInOnce;
        UpdateAmmoViewer?.Invoke(_ammoCountsInMagazine, _ammoCountsInPossession);
        if (_ammoCountsInMagazine < 0) _ammoCountsInMagazine = 0;
    }

    public int ReturnLeftAmmoCount() { return _ammoCountsInMagazine; }

    protected Vector3 ReturnMuzzlePos() { return _muzzle.position; }


    #region Reload

    public void OnReloadRequested(int ammoCountsInMagazine, int ammoCountsInPossession)
    {
        _ammoCountsInMagazine = ammoCountsInMagazine;
        _ammoCountsInPossession = ammoCountsInPossession;
        UpdateAmmoViewer?.Invoke(_ammoCountsInMagazine, _ammoCountsInPossession);
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

    bool _nowDrop = false;

    public override void PositionWeapon(bool nowDrop)
    {
        _nowDrop = nowDrop;

        if (nowDrop)
        {
            _gunCollider.enabled = true;
            _gunRigidbody.isKinematic = false;
            _gunRigidbody.useGravity = true;
        }
        else
        {
            _gunCollider.enabled = false;
            _gunRigidbody.isKinematic = true;
            _gunRigidbody.useGravity = false;
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region Equip

    public override void OnEquip()
    {
        base.OnEquip();
        UpdateAmmoViewer?.Invoke(_ammoCountsInMagazine, _ammoCountsInPossession);
        ActiveAmmoViewer?.Invoke(true);
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

    public void OnSightEnter()
    {
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ActiveInteractableInfo, true, _weaponName.ToString(), _objectMesh.position);
    }

    public void OnSightExit()
    {
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ActiveInteractableInfo, false);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        //_nowAttachToGround = true; // ���� �ε����� �׶����� Interaction ����
    }

    public bool IsInteractable() { return _nowDrop; }

    public void Interact(IInteracter interacter)
    {
        interacter.AddWeapon(this);
    }
}