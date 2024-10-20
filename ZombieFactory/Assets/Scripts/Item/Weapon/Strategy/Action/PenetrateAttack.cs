using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PenetrateAttack : ApplyAttack //, IDisplacement
{
    /// <summary>
    /// 무기의 애니메이션을 실행시킬 때 호출
    /// </summary>
    Animator _weaponAnimator;

    /// <summary>
    /// 이팩트 생성 팩토리
    /// </summary>
    protected BaseFactory _effectFactory;

    /// <summary>
    /// 머즐의 위치를 반환한다.
    /// </summary>
    Func<Vector3> ReturnMuzzlePosition;

    /// <summary>
    /// 남은 총알 수를 반환한다.
    /// </summary>
    protected Func<int> ReturnLeftAmmoCount;

    /// <summary>
    /// fireCountInOnce 만큼 총알을 감소시킨다.
    /// </summary>
    Action<int> DecreaseAmmoCount;

    /// <summary>
    /// 총구 화염을 생성시킨다.
    /// </summary>
    Action SpawnMuzzleFlashEffect;

    /// <summary>
    /// 탄피를 생성시킨다.
    /// </summary>
    Action SpawnEmptyCartridge;

    /// <summary>
    /// 이동 변위 수치를 받아온다.
    /// </summary>
    protected Func<float> ReceiveMoveDisplacement;

    protected float _penetratePower;

    protected float _minDecreaseRatio = 0.05f;
    protected float _trajectoryLineOffset = 1.3f;

    /// <summary>
    /// 액션 1번에 소모되는 총알의 개수
    /// </summary>
    protected int _fireCountInOnce;

    protected float _displacementWeight = 0;
    protected float _displacementDecreaseRatio;

    protected float _additionalWeight;

    public float DisplacementWeight { get { return ReceiveMoveDisplacement() + _additionalWeight; } }


    public PenetrateAttack(BaseItem.Name weaponName, float range, int targetLayer, int fireCountInOnce,
        float penetratePower, float displacementDecreaseRatio, Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary,

        Animator weaponAnimator, BaseFactory effectFactory, Func<Vector3> ReturnMuzzlePosition, Func<int> ReturnLeftAmmoCount,
        Action<int> DecreaseAmmoCount, Action SpawnMuzzleFlashEffect, Action SpawnEmptyCartridge)

        : base(weaponName, range, targetLayer, weaponAnimator)
    {
        _fireCountInOnce = fireCountInOnce;
        _penetratePower = penetratePower;
        _displacementDecreaseRatio = displacementDecreaseRatio;
        _damageConverter = new DistanceAreaBasedDamageConverter(damageDictionary);

        _weaponAnimator = weaponAnimator;
        _effectFactory = effectFactory;


        this.ReturnMuzzlePosition = ReturnMuzzlePosition;
        this.ReturnLeftAmmoCount = ReturnLeftAmmoCount;
        this.DecreaseAmmoCount = DecreaseAmmoCount;

        this.SpawnMuzzleFlashEffect = SpawnMuzzleFlashEffect;
        this.SpawnEmptyCartridge = SpawnEmptyCartridge;
    }

    protected override void PlayAnimation(string aniName)
    {
        base.PlayAnimation(aniName);
        _weaponAnimator.Play(aniName, -1, 0);
    }

    protected override void PlayAnimation(string aniName, int index)
    {
        base.PlayAnimation(aniName);
        _weaponAnimator.Play(aniName + index, -1, 0);
    }


    RaycastHit[] DetectCollider(Vector3 origin, Vector3 direction, float maxDistance, int layer)
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin, direction, maxDistance, layer);

        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance)); // 맞은 객체들을 거리 기반으로 정렬해준다.

        return hits;
    }

    protected List<PenetrateData> ReturnPenetrateData(Vector3 directionOffset = default(Vector3), Vector3 positionOffset = default(Vector3))
    {
        //Vector3 camPos = Vector3.zero;
        //Vector3 camFowardDir = Vector3.zero;

        Vector3 attackPosition = _attackPoint.ReturnPosition();
        Vector3 attackDirection = _attackPoint.ReturnDirection();

        RaycastHit[] entryHits = DetectCollider(attackPosition, attackDirection + directionOffset, _range, _targetLayer);

        if (entryHits.Length == 0) return null; // 만약 아무도 맞지 않았다면 리턴

        Vector3 endPoint = attackPosition + (attackDirection + directionOffset) * _range; // 레이캐스트가 닫는 가장 마지막 위치

        RaycastHit[] exitHits = DetectCollider(endPoint, -(attackDirection + directionOffset), _range, _targetLayer);

        List<PenetrateData> penetrateDatas = new List<PenetrateData>();

        for (int i = 0; i < entryHits.Length; i++) // 정렬 순서로 
        {
            for (int j = 0; j < exitHits.Length; j++)
            {
                if (entryHits[i].collider == exitHits[j].collider)
                {
                    GameObject target = entryHits[i].collider.gameObject;

                    IPenetrable penetrable = entryHits[i].collider.GetComponent<IPenetrable>();
                    if (penetrable == null) continue;

                    float distanceFromStartPoint = Vector3.Distance(attackPosition, entryHits[i].point);

                    penetrateDatas.Add(new PenetrateData(distanceFromStartPoint, entryHits[i].point, exitHits[j].point, entryHits[i].normal, exitHits[j].normal, target));
                    break;
                }
            }
        }

        return penetrateDatas;
    }

    protected override float CalculateDamage(IHitable hit, PenetrateData data, float decreaseRatio)
    {
        float damage = _damageConverter.ReturnDamage(hit.ReturnArea(), data.DistanceFromStartPoint);

        if (decreaseRatio > _minDecreaseRatio) // decreaseRatio가 5% 이상인 경우만 해당
        {
            damage -= Mathf.Round(damage * decreaseRatio);
        }

        return damage;
    }

    protected override void ApplyDamage(IHitable hit, PenetrateData data, float decreaseRatio)
    {
        float damage = CalculateDamage(hit, data, decreaseRatio);
        hit.OnHit(damage, data.EntryPoint, data.EntryNormal); // 데미지 적용
    }

    //bool CheckIsAlreadyDamaged(IHitable hit, List<IDamageable> alreadyDamagedObjects)
    //{
    //    if (alreadyDamagedObjects.Contains(hit.IDamage))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        alreadyDamagedObjects.Add(hit.IDamage);
    //        return false;
    //    }
    //}

    void DrawPenetrateDebugLine(Vector3 hitPoint)
    {
        Vector3 attackPosition = _attackPoint.ReturnPosition();
        float diatance = Vector3.Distance(attackPosition, hitPoint);
        Vector3 direction = (hitPoint - attackPosition).normalized;

        Debug.DrawRay(attackPosition, direction * diatance, Color.green, 10); // 디버그 레이를 카메라가 바라보고 있는 방향으로 발사한다.
    }

    void DrawTrajectoryLine(Vector3 hitPosition)
    {
        Vector3 muzzlePos = ReturnMuzzlePosition();
        BaseEffect trajectoryLineEffect = _effectFactory.Create(BaseEffect.Name.TrajectoryLine);

        trajectoryLineEffect.ResetData(hitPosition, muzzlePos);
        trajectoryLineEffect.Play();
    }

    void DrawPenetrateLine(PenetrateData penetrateData, bool canPenetrate)
    {
        if(canPenetrate)
        {
            Vector3 muzzlePos = ReturnMuzzlePosition();

            Vector3 offsetDir = (penetrateData.ExitPoint - muzzlePos).normalized * _trajectoryLineOffset;
            DrawTrajectoryLine(penetrateData.ExitPoint + offsetDir);
            DrawPenetrateDebugLine(penetrateData.ExitPoint);
        }
        else
        {
            DrawTrajectoryLine(penetrateData.EntryPoint);
            DrawPenetrateDebugLine(penetrateData.EntryPoint);
        }
    }

    protected void Shoot(Vector3 directionOffset = default(Vector3), Vector3 positionOffset = default(Vector3))
    {
        List<PenetrateData> penetrateDatas = ReturnPenetrateData(directionOffset, positionOffset);
        if (penetrateDatas == null) return;

        float tmpPenetratePower = _penetratePower;

        for (int i = 0; i < penetrateDatas.Count; i++)
        {
            // 관통된 오브젝트를 가져옴
            GameObject target = penetrateDatas[i].Target;
            
            IPenetrable penetrable = target.GetComponent<IPenetrable>(); // IPenetrable 가져옴
            if(penetrable != null)
            {
                // 오브젝트의 내구도를 가져옴
                float finalDurability = penetrateDatas[i].ReturnDistance() * penetrable.ReturnDurability();

                bool IsLastContact = false;
                if (i == penetrateDatas.Count - 1) IsLastContact = true; // 마지막 충돌 시

                IEffectable effectable = target.GetComponent<IEffectable>(); //IEffectable 가져옴
                tmpPenetratePower = CalculatePenetratePower(tmpPenetratePower, finalDurability, effectable, penetrateDatas[i], IsLastContact);

                // 관통 수치가 0이면 break
                if (tmpPenetratePower == 0) break;
            }

            // IHitable 가져옴
            IHitable hitable = target.GetComponent<IHitable>();
            if (hitable != null)
            {
                // 데미지 감소 적용
                float decreasePowerRatio = (_penetratePower - tmpPenetratePower) / _penetratePower;
                if (hitable != null) ApplyDamage(hitable, penetrateDatas[i], decreasePowerRatio);
            }

            if (penetrateDatas.Count <= i + 1) continue; // 뒤에 관통 정보가 없다면 진행하지 않음


            float distanceBetweenExitAndEntryPoint = Vector3.Distance(penetrateDatas[i].ExitPoint, penetrateDatas[i + 1].EntryPoint);
            float finalDistanceBetweenExitAndEntryPoint = distanceBetweenExitAndEntryPoint * PenetrateData.AirDurability;
            tmpPenetratePower -= finalDistanceBetweenExitAndEntryPoint;
        }
    }

    float CalculatePenetratePower(float penetratePower, float durability, IEffectable effectableTarget, PenetrateData penetrateData, bool isLastContact)
    {
        if (penetratePower - durability >= 0)
        {
            effectableTarget?.SpawnEffect(IEffectable.ConditionType.Penetration, penetrateData.EntryPoint, penetrateData.EntryNormal); // null일 수 있기 때문에 다음과 같이 작성
            effectableTarget?.SpawnEffect(IEffectable.ConditionType.Penetration, penetrateData.ExitPoint, penetrateData.ExitNormal);

            penetratePower -= durability;
            if (isLastContact == true) DrawPenetrateLine(penetrateData, true);

            return penetratePower;
        }

        effectableTarget?.SpawnEffect(IEffectable.ConditionType.NonPenetration, penetrateData.EntryPoint, penetrateData.EntryNormal);
        DrawPenetrateLine(penetrateData, false);
        return 0;
    }

    public override void Execute()
    {
        Vector3 muzzlePosition = ReturnMuzzlePosition();
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundControllable.SoundName.Fire, muzzlePosition);
        DecreaseAmmoCount?.Invoke(_fireCountInOnce); // 총알 감소

        //Vector3 muzzlePos = ReturnMuzzlePos();
        // _muzzle 사용
        //OnNoiseGenerateRequested?.Invoke(muzzlePos);
        //PlaySound?.Invoke(SoundType.Attack, true);

        // 여기에 사운드 발생 기능 추가
        PlayAnimation("Fire");
        SpawnMuzzleFlashEffect?.Invoke();
        SpawnEmptyCartridge?.Invoke();
    }

    public override void UnlinkEvent(WeaponBlackboard blackboard)
    {
        //ReturnRaycastPos -= blackboard.ReturnRaycastPos;
        //ReturnRaycastDir -= blackboard.ReturnRaycastDir;
        _attackPoint = null;
        OnPlayOwnerAnimation = null;
        ReceiveMoveDisplacement = null;
    }

    public override void LinkEvent(WeaponBlackboard blackboard)
    {
        //ReturnRaycastPos += blackboard.ReturnRaycastPos;
        //ReturnRaycastDir += blackboard.ReturnRaycastDir;
        _attackPoint = blackboard.AttackPoint;
        OnPlayOwnerAnimation = blackboard.OnPlayOwnerAnimation;
        ReceiveMoveDisplacement = blackboard.SendMoveDisplacement;
    }

    //public void OnDisplacementWeightReceived(float displacement)
    //{
    //    _displacementWeight = displacement * _displacementDecreaseRatio;
    //}

    //public override void ResetLeftBulletCount(int leftBulletCount) { _leftBulletCount = leftBulletCount; }

    public override bool CanExecute() { return ReturnLeftAmmoCount() > 0; } // _leftBulletCount > 0

    //public override void TurnOffZoomWhenOtherExecute() { }
}