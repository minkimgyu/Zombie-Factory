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


    protected string _trajectoryLineEffect = "TrajectoryLine";

    protected int _maxAmmoCountInMagazine;
    protected int _maxAmmoCountsInPossession;

    [SerializeField] protected int _ammoCountsInMagazine;
    [SerializeField] protected int _ammoCountsInPossession;

    protected float _reloadDuration;
    protected float _reloadExitDuration;

    [SerializeField] Transform _objectMesh;

    BoxCollider _gunCollider;
    Rigidbody _gunRigidbody;
    bool _nowAttachToGround;

    protected Timer _reloadTimer;
    protected Timer _reloadExitTimer;

    public Action<bool, string, Vector3> OnViewEventRequest;

    protected Action<Vector3> OnGenerateNoiseRequest;

    protected void SpawnEmptyCartridge() => _emptyCartridgeSpawner.Play();
    protected void SpawnMuzzleFlashEffect() => _muzzleFlash.Play();

    protected override void PlayAnimation(string name)
    {
        base.PlayAnimation(name);
        _animator?.Play(name, -1, 0);
    }

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

    public override bool CanAutoReload() { return _ammoCountsInMagazine == 0 && _ammoCountsInPossession > 0; }

    public override bool CanReload()
    {
        if (_ammoCountsInPossession <= 0 || _maxAmmoCountInMagazine == _ammoCountsInMagazine) return false;
        else return true;
    }

    public override void OnReloadStart()
    {
        for (int i = 0; i < _actionStrategies.Count; i++) _actionStrategies[(EventType)i].TurnOffZoomDirectly();

        _reloadTimer.Start(_reloadDuration);

        _reloadExitTimer.Reset(); // ���� �� ���½����ֱ�
        _reloadExitTimer.Start(_reloadExitDuration);

        string reloadString = "Reload";
        PlayAnimation(reloadString);
    }

    // ������ ������ ���� �̺�Ʈ ȣ���
    public override void OnReloadEnd()
    {
        int leftAmmoCountInPossession = _ammoCountsInPossession - _ammoCountsInMagazine;
        _ammoCountsInMagazine = _maxAmmoCountInMagazine;
        _ammoCountsInPossession = leftAmmoCountInPossession;
        OnShowRounds?.Invoke(true, _ammoCountsInMagazine, _ammoCountsInPossession);
    }

    //// ���� �ϴ� ���߿� ���콺 �Է��� ���� ���� ĵ��
    //public override bool CanCancelReloadAndGoToMainAction() { return default; }

    //// ���� �ϴ� ���߿� ���콺 �Է��� ���� ���� ĵ��
    //public override bool CanCancelReloadAndGoToSubAction() { return default; }

    public override bool IsReloadFinish() // �������� ���� ���
    {
        return _reloadExitTimer.CurrentState == Timer.State.Finish;
    }

    public override void ResetReload() // �������� ����� ���
    {
        _reloadTimer.Reset();
        _reloadExitTimer.Reset();
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
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
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

        _animator = GetComponent<Animator>();
        _gunCollider = GetComponent<BoxCollider>();
        _gunRigidbody = GetComponent<Rigidbody>();

        _gunCollider.enabled = false;
        _gunRigidbody.isKinematic = true;

        //WeaponInfoViwer weaponInfoViwer = FindObjectOfType<WeaponInfoViwer>();
        //if (weaponInfoViwer == null) return;
        //OnViewEventRequest = weaponInfoViwer.OnViewEventReceived; // ��� �� ���� �ʿ�

        _reloadTimer = new Timer();
        _reloadExitTimer = new Timer();
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