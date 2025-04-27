using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class Gun : BaseWeapon, IInteractable
{
    // 드랍 시 아이템을 버리는 방향
    protected Func<Vector3> ReturnRaycastDir;

    [SerializeField] protected ParticleSystem _muzzleFlash;
    [SerializeField] protected ParticleSystem _emptyCartridgeSpawner;

    protected float _trajectoryLineOffset = 1.3f;

    [SerializeField] protected Transform _muzzle;


    protected int _maxAmmoCountInMagazine;
    protected int _maxAmmoCountsInPossession;

    [SerializeField] protected int _ammoCountsInMagazine;
    [SerializeField] protected int _ammoCountsInPossession;

    [SerializeField] Transform _objectMesh;

    BoxCollider _gunCollider;
    Rigidbody _gunRigidbody;

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
        for (int i = 0; i < _actionStrategy.Count; i++) _actionStrategy[(EventType)i].TurnOffZoomDirectly();
        _reloadStrategy.Execute(isTPS, _ammoCountsInMagazine, _ammoCountsInPossession);
    }

    // 장전 하는 도중에 마우스 입력을 통한 장전 캔슬
    public override bool CanCancelReloadAndGoToMainAction() { return _reloadStrategy.CanCancelReloadingByLeftClick(); }

    // 장전 하는 도중에 마우스 입력을 통한 장전 캔슬
    public override bool CanCancelReloadAndGoToSubAction() { return _reloadStrategy.CanCancelReloadingByLeftClick(); }

    public override bool IsReloadFinish() // 재장전이 끝난 경우
    {
        return _reloadStrategy.IsReloadFinish();
    }

    public override void ResetReload() // 재장전을 취소할 경우
    {
        _reloadStrategy.OnCancelReload();
    }

    #endregion

    #region Drop

    public override bool CanDrop() { return true; }

    public override void ThrowWeapon(float force)
    {
        PositionItem(true);
        Vector3 direction = _attackPoint.forward;
        _gunRigidbody.AddForce(direction * force, ForceMode.Impulse);
    }

    bool _nowDrop = false;

    public override void PositionItem(bool nowDrop)
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

    public override bool NowDrop() { return _nowDrop; }

    #endregion

    #region Equip

    public override void OnEquip()
    {
        base.OnEquip();
        UpdateAmmoViewer?.Invoke(_ammoCountsInMagazine, _ammoCountsInPossession);
        ActiveAmmoViewer?.Invoke(true);
    }

    #endregion;

    public override void Initialize()
    {
        base.Initialize();

        _gunCollider = GetComponent<BoxCollider>();
        _gunRigidbody = GetComponent<Rigidbody>();

        _gunCollider.enabled = false;
        _gunRigidbody.isKinematic = true;
    }

    public void OnSightEnter()
    {
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ActiveInteractableInfo, true, _weaponName.ToString(), _objectMesh.position);
    }

    public void OnSightExit()
    {
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ActiveInteractableInfo, false);
    }

    public bool IsInteractable() { return _nowDrop; }

    public void Interact(IInteracter interacter)
    {
        interacter.AddWeapon(this);
    }

    Dictionary<Name, ISoundControllable.SoundName> _equipSoundName = new Dictionary<Name, ISoundControllable.SoundName>
    {
        { Name.Vandal, ISoundControllable.SoundName.EquipMagazineGun },
        { Name.Phantom, ISoundControllable.SoundName.EquipMagazineGun },
        { Name.Odin, ISoundControllable.SoundName.EquipMagazineGun },

        { Name.Judge, ISoundControllable.SoundName.EquipMagazineGun },
        { Name.Stinger, ISoundControllable.SoundName.EquipMagazineGun },
        { Name.Guardian, ISoundControllable.SoundName.EquipMagazineGun },

        { Name.Operator, ISoundControllable.SoundName.EquipMagazineGun },
        { Name.Classic, ISoundControllable.SoundName.EquipMagazineGun },

        { Name.Knife, ISoundControllable.SoundName.EquipKnife },
        { Name.Bucky, ISoundControllable.SoundName.EquipShotgun },
    };

    public virtual void PlayEquipSound()
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(_equipSoundName[_weaponName], 0.15f);
    }

    public virtual void PlayMagazineInSound()
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundControllable.SoundName.MagIn, 0.15f);
    }

    public virtual void PlayMagazineOutSound()
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundControllable.SoundName.MagOut, 0.15f);
    }

    public virtual void PlayPullHandleSound()
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundControllable.SoundName.PullHandle, 0.15f);
    }

    public virtual void PlayPushHandleSound()
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundControllable.SoundName.PushHandle, 0.15f);
    }
}