//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//// ���� �׽�ũ ����
//abstract public class ReloadStrategy : BaseStrategy
//{
//    public ReloadStrategy(Action<SoundType, bool> PlaySound) : base() { this.PlaySound = PlaySound; }

//    public virtual void Execute(bool isTPS, int ammoCountInMagazine, int ammoCountInPossession) { }


//    /// <summary>
//    /// Ư���� ������� Reload���� Ż�� ������ ���
//    /// </summary>
//    public virtual bool CanCancelReloadingByLeftClick() { return false; }

//    public virtual bool CanCancelReloadingByRightClick() { return false; }

//    /// <summary>
//    /// ���ε尡 ���� ��� �ش� State���� Ż��
//    /// </summary>
//    public virtual bool IsReloadFinish() { return false; }
//    //public abstract bool IsReloadRunning();

//    protected virtual void CalculateAmmoWhenReload() { }

//    /// <summary>
//    /// ���ε� ��� �� �۵� 
//    /// </summary>
//    public virtual void OnCancelReload() { }

//    //  return _ammoCountsInMagazine == 0 && _ammoCountsInPossession > 0;
//    public virtual bool CanAutoReload(int ammoCountsInMagazine, int ammoCountsInPossession) { return ammoCountsInMagazine == 0 && ammoCountsInPossession > 0; }

//    //  if (_ammoCountsInPossession <= 0 || _maxAmmoCountInMagazine == _ammoCountsInMagazine) return false;
//    //  else return true;
//    public virtual bool CanReload(int ammoCountsInMagazine, int ammoCountsInPossession, int maxAmmoCountInMagazine) 
//    {
//        if (ammoCountsInPossession <= 0 || maxAmmoCountInMagazine == ammoCountsInMagazine) return false;
//        else return true;
//    }

//    //public abstract void OnUnlink();

//    //public abstract void OnLink();

//    //public abstract void OnInintialize();
//}

//public class NoReload : ReloadStrategy
//{
//    public NoReload(Action<SoundType, bool> PlaySound) : base(PlaySound) { }
//}

//abstract public class BaseReload : ReloadStrategy
//{
//    protected Timer _reloadTimer;
//    protected Timer _reloadExitTimer;

//    protected float _reloadExitDuration;

//    protected int _ammoCountInMagazine;
//    protected int _maxAmmoCountInMagazine;
//    protected int _ammoCountsInPossession;

//    protected BaseItem.Name _weaponName;
//    //protected Animator _weaponAnimator;
//    //protected Animator _ownerAnimator;

//    protected Action<string, int, float> OnPlayWeaponAnimation;

//    protected Action<string, int, float> OnPlayOwnerAnimation;

//    // ���� ������ �Ѿ� ���� �޾ƿ��� Func �̺�Ʈ �߰�
//    // �̰ɷ� ���� �������� ���� �Ǵ� ����

//    protected Action<int, int> OnReloadRequested;

//    public BaseReload(BaseItem.Name weaponName, float reloadExitDuration, int maxAmmoCountInMagazine,
//        Action<string, int, float> OnPlayWeaponAnimation, Action<int, int> OnReloadRequested, Action<SoundType, bool> PlaySound) : base(PlaySound)
//    {
//        _reloadTimer = new Timer();
//        _reloadExitTimer = new Timer();

//        _maxAmmoCountInMagazine = maxAmmoCountInMagazine;

//        _reloadExitDuration = reloadExitDuration;

//        _weaponName = weaponName;

//        this.OnPlayWeaponAnimation = OnPlayWeaponAnimation;
//        this.OnReloadRequested = OnReloadRequested;
//    }

//    public override void LinkEvent(WeaponBlackboard blackboard)
//    {
//        OnPlayOwnerAnimation += blackboard.OnPlayOwnerAnimation;
//    }

//    public override void UnlinkEvent(WeaponBlackboard blackboard)
//    {
//        OnPlayOwnerAnimation -= blackboard.OnPlayOwnerAnimation;
//    }

//    protected void PlayAnimation(string aniName)
//    {
//        Debug.Log(aniName);
//        OnPlayWeaponAnimation?.Invoke(aniName, -1, 0);
//        OnPlayOwnerAnimation?.Invoke(_weaponName.ToString() + aniName, -1, 0);
//    }

//    public override bool IsReloadFinish()
//    {
//        return _reloadExitTimer.CurrentState == Timer.State.Finish;
//    }
//}

//    // źâ���� �����ϴ� ���
//public class MagazineReload : BaseReload
//{
//    protected float _reloadDuration;

//    public MagazineReload(BaseItem.Name weaponName, float reloadDuration, float reloadExitDuration, int maxAmmoCountInMagazine,
//         Action<string, int, float> OnPlayWeaponAnimation, Action<int, int> OnReloadRequested, Action<SoundType, bool> PlaySound) 
//        : base(weaponName, reloadExitDuration, maxAmmoCountInMagazine, OnPlayWeaponAnimation, OnReloadRequested, PlaySound)
//    {
//        _reloadDuration = reloadDuration;
//    }

//    public override void Execute(bool isTPS, int ammoCountInMagazine, int ammoCountInPossession) 
//    {
//        _ammoCountInMagazine = ammoCountInMagazine;
//        _ammoCountsInPossession = ammoCountInPossession;


//        _reloadTimer.Start(_reloadDuration);

//        _reloadExitTimer.Reset(); // ���� �� ���½����ֱ�
//        _reloadExitTimer.Start(_reloadExitDuration);

//        string reloadString = "Reload";
//        if (isTPS) reloadString = "TPS" + reloadString;
//        PlayAnimation(reloadString);
//    }

//    public override void OnUpdate() 
//    {
//        if(_reloadTimer.CurrentState == Timer.State.Finish)
//        {
//            PlaySound(SoundType.Reload, true);
//            CalculateAmmoWhenReload();
//            _reloadTimer.Reset();
//        }
//    }

//    public override void OnCancelReload()
//    {
//        _reloadTimer.Reset();
//        _reloadExitTimer.Reset();
//    }

//    protected override void CalculateAmmoWhenReload()
//    {
//        int canLoadBulletCount = _maxAmmoCountInMagazine - _ammoCountInMagazine;

//        if (_ammoCountsInPossession >= canLoadBulletCount)
//        {
//            _ammoCountInMagazine = _maxAmmoCountInMagazine;
//            _ammoCountsInPossession -= canLoadBulletCount;
//        }
//        else
//        {
//            _ammoCountInMagazine += _ammoCountsInPossession;
//            _ammoCountsInPossession = 0;
//        }

//        OnReloadRequested?.Invoke(_ammoCountInMagazine, _ammoCountsInPossession);
//    }
//}

////// �� �߾� �����ϴ� ���
//public class RoundByRoundReload : BaseReload
//{
//    Timer _reloadBeforeTimer;
//    float _reloadBeforeDuration;

//    float _reloadDurationPerRound;
//    float _storedReloadRatio;
//    float _reloadRatio;

//    public RoundByRoundReload(BaseItem.Name weaponName, float reloadBeforeDuration, float reloadDurationPerRound, 
//        float reloadExitDuration, int maxAmmoCountInMagazine, 
//        Action<string, int, float> OnPlayWeaponAnimation, Action<int, int> OnReloadRequested, Action<SoundType, bool> PlaySound)
//        : base(weaponName, reloadExitDuration, maxAmmoCountInMagazine, OnPlayWeaponAnimation, OnReloadRequested, PlaySound)
//    {
//        _reloadBeforeDuration = reloadBeforeDuration;
//        _reloadDurationPerRound = reloadDurationPerRound;

//        _reloadBeforeTimer = new Timer();
//        _storedReloadRatio = 0;
//    }

//    public override bool CanCancelReloadingByLeftClick() { return Input.GetMouseButtonDown(0); }
//    public override bool CanCancelReloadingByRightClick() { return Input.GetMouseButtonDown(1); }

//    public override void Execute(bool isTPS, int ammoCountInMagazine, int ammoCountInPossession) // maxAmmoCountInMagazine �̰� �����ڿ��� �ޱ�
//    {
//        _ammoCountInMagazine = ammoCountInMagazine;
//        _ammoCountsInPossession = ammoCountInPossession;

//        int roundCount = ReturnCanReloadRoundCount();
//        _reloadRatio = 1.0f / roundCount;

//        _reloadBeforeTimer.Start(_reloadBeforeDuration);
//    }

//    int ReturnCanReloadRoundCount()
//    {
//        int canReloadRoundCount = _maxAmmoCountInMagazine - _ammoCountInMagazine;
//        int reloadRoundCount;

//        if (canReloadRoundCount > _ammoCountsInPossession) reloadRoundCount = _ammoCountsInPossession;
//        else reloadRoundCount = canReloadRoundCount;

//        return reloadRoundCount;
//    }

//    public override void OnUpdate()
//    {
//        if (_reloadBeforeTimer.CurrentState == Timer.State.Finish)
//        {
//            float reloadDuration = ReturnCanReloadRoundCount() * _reloadDurationPerRound;

//            _reloadTimer.Start(reloadDuration);
//            _reloadExitTimer.Start(reloadDuration + 0.16f);

//            PlayAnimation("FirstReload");
//            _reloadBeforeTimer.Reset();
//        }

//        if (_reloadBeforeTimer.CurrentState == Timer.State.Running) return;

//        if (_reloadTimer.CurrentState == Timer.State.Finish)
//        {
//            PlayAnimation("EndReload");

//            _storedReloadRatio = 0; // ���� ��, �ʱ�ȭ ��������
//            _reloadTimer.Reset();
//        }

//        if(_reloadTimer.CurrentState == Timer.State.Running)
//        {
//            // --> Ratio�� �����ؼ� 7���� �����ؾ��ϴ� ��� 1 / 7 ���� 1�߾� �߰�����
//            if (_storedReloadRatio < _reloadTimer.Ratio)
//            {
//                CalculateAmmoWhenReload();
//                _storedReloadRatio += _reloadRatio;
//                PlayAnimation("AfterReload");
//            }
//        }
//    }

//    public override void OnCancelReload()
//    {
//        PlayAnimation("EndReload");

//        _storedReloadRatio = 0;
//        _reloadBeforeTimer.Reset();
//        _reloadTimer.Reset();
//        _reloadExitTimer.Reset();
//    }

//    protected override void CalculateAmmoWhenReload()
//    {
//        _ammoCountsInPossession -= 1;
//        _ammoCountInMagazine += 1;

//        OnReloadRequested?.Invoke(_ammoCountInMagazine, _ammoCountsInPossession);
//    }
//}