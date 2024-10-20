using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PenetrateAttack : ApplyAttack //, IDisplacement
{
    /// <summary>
    /// ������ �ִϸ��̼��� �����ų �� ȣ��
    /// </summary>
    Animator _weaponAnimator;

    /// <summary>
    /// ����Ʈ ���� ���丮
    /// </summary>
    protected BaseFactory _effectFactory;

    /// <summary>
    /// ������ ��ġ�� ��ȯ�Ѵ�.
    /// </summary>
    Func<Vector3> ReturnMuzzlePosition;

    /// <summary>
    /// ���� �Ѿ� ���� ��ȯ�Ѵ�.
    /// </summary>
    protected Func<int> ReturnLeftAmmoCount;

    /// <summary>
    /// fireCountInOnce ��ŭ �Ѿ��� ���ҽ�Ų��.
    /// </summary>
    Action<int> DecreaseAmmoCount;

    /// <summary>
    /// �ѱ� ȭ���� ������Ų��.
    /// </summary>
    Action SpawnMuzzleFlashEffect;

    /// <summary>
    /// ź�Ǹ� ������Ų��.
    /// </summary>
    Action SpawnEmptyCartridge;

    /// <summary>
    /// �̵� ���� ��ġ�� �޾ƿ´�.
    /// </summary>
    protected Func<float> ReceiveMoveDisplacement;

    protected float _penetratePower;

    protected float _minDecreaseRatio = 0.05f;
    protected float _trajectoryLineOffset = 1.3f;

    /// <summary>
    /// �׼� 1���� �Ҹ�Ǵ� �Ѿ��� ����
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

        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance)); // ���� ��ü���� �Ÿ� ������� �������ش�.

        return hits;
    }

    protected List<PenetrateData> ReturnPenetrateData(Vector3 directionOffset = default(Vector3), Vector3 positionOffset = default(Vector3))
    {
        //Vector3 camPos = Vector3.zero;
        //Vector3 camFowardDir = Vector3.zero;

        Vector3 attackPosition = _attackPoint.ReturnPosition();
        Vector3 attackDirection = _attackPoint.ReturnDirection();

        RaycastHit[] entryHits = DetectCollider(attackPosition, attackDirection + directionOffset, _range, _targetLayer);

        if (entryHits.Length == 0) return null; // ���� �ƹ��� ���� �ʾҴٸ� ����

        Vector3 endPoint = attackPosition + (attackDirection + directionOffset) * _range; // ����ĳ��Ʈ�� �ݴ� ���� ������ ��ġ

        RaycastHit[] exitHits = DetectCollider(endPoint, -(attackDirection + directionOffset), _range, _targetLayer);

        List<PenetrateData> penetrateDatas = new List<PenetrateData>();

        for (int i = 0; i < entryHits.Length; i++) // ���� ������ 
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

        if (decreaseRatio > _minDecreaseRatio) // decreaseRatio�� 5% �̻��� ��츸 �ش�
        {
            damage -= Mathf.Round(damage * decreaseRatio);
        }

        return damage;
    }

    protected override void ApplyDamage(IHitable hit, PenetrateData data, float decreaseRatio)
    {
        float damage = CalculateDamage(hit, data, decreaseRatio);
        hit.OnHit(damage, data.EntryPoint, data.EntryNormal); // ������ ����
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

        Debug.DrawRay(attackPosition, direction * diatance, Color.green, 10); // ����� ���̸� ī�޶� �ٶ󺸰� �ִ� �������� �߻��Ѵ�.
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
            // ����� ������Ʈ�� ������
            GameObject target = penetrateDatas[i].Target;
            
            IPenetrable penetrable = target.GetComponent<IPenetrable>(); // IPenetrable ������
            if(penetrable != null)
            {
                // ������Ʈ�� �������� ������
                float finalDurability = penetrateDatas[i].ReturnDistance() * penetrable.ReturnDurability();

                bool IsLastContact = false;
                if (i == penetrateDatas.Count - 1) IsLastContact = true; // ������ �浹 ��

                IEffectable effectable = target.GetComponent<IEffectable>(); //IEffectable ������
                tmpPenetratePower = CalculatePenetratePower(tmpPenetratePower, finalDurability, effectable, penetrateDatas[i], IsLastContact);

                // ���� ��ġ�� 0�̸� break
                if (tmpPenetratePower == 0) break;
            }

            // IHitable ������
            IHitable hitable = target.GetComponent<IHitable>();
            if (hitable != null)
            {
                // ������ ���� ����
                float decreasePowerRatio = (_penetratePower - tmpPenetratePower) / _penetratePower;
                if (hitable != null) ApplyDamage(hitable, penetrateDatas[i], decreasePowerRatio);
            }

            if (penetrateDatas.Count <= i + 1) continue; // �ڿ� ���� ������ ���ٸ� �������� ����


            float distanceBetweenExitAndEntryPoint = Vector3.Distance(penetrateDatas[i].ExitPoint, penetrateDatas[i + 1].EntryPoint);
            float finalDistanceBetweenExitAndEntryPoint = distanceBetweenExitAndEntryPoint * PenetrateData.AirDurability;
            tmpPenetratePower -= finalDistanceBetweenExitAndEntryPoint;
        }
    }

    float CalculatePenetratePower(float penetratePower, float durability, IEffectable effectableTarget, PenetrateData penetrateData, bool isLastContact)
    {
        if (penetratePower - durability >= 0)
        {
            effectableTarget?.SpawnEffect(IEffectable.ConditionType.Penetration, penetrateData.EntryPoint, penetrateData.EntryNormal); // null�� �� �ֱ� ������ ������ ���� �ۼ�
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
        DecreaseAmmoCount?.Invoke(_fireCountInOnce); // �Ѿ� ����

        //Vector3 muzzlePos = ReturnMuzzlePos();
        // _muzzle ���
        //OnNoiseGenerateRequested?.Invoke(muzzlePos);
        //PlaySound?.Invoke(SoundType.Attack, true);

        // ���⿡ ���� �߻� ��� �߰�
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