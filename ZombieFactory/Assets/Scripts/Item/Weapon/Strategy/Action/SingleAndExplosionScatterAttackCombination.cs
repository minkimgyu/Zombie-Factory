using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAndExplosionScatterAttackCombination : ActionStrategy // Attack�� �ƴ϶� �ٸ� ������� ����ؾ��� ��
{
    SingleProjectileAttack singleProjectileAttack;
    ScatterProjectileAttack scatterProjectileGunAttack;
    // �� �� ���� �����ڿ��� �ʱ�ȭ
    // ����, Ÿ�Կ� �°� ����

    //Func<Vector3> ReturnRaycastPos;
    //Func<Vector3> ReturnRaycastDir;

    int _targetLayer;

    IPoint _attackPoint;
    float _findRange;
    bool _isInFront;

    public SingleAndExplosionScatterAttackCombination(

        BaseItem.Name weaponName, float range, int targetLayer,

        int singleBulletCountsInOneShoot, float singlePenetratePower, float singleDisplacementDecreaseRatio,
        int scatterBulletCountsInOneShoot, float scatterPenetratePower, float scatterDisplacementDecreaseRatio, int pelletCount, float spreadOffset,

        float frontDistance, string explosionEffectName,

        Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary,
        Dictionary<IHitable.Area, DistanceAreaData[]> scatterDamageDictionary, float findRange,


        Animator animator, BaseFactory effectFactory, Func<Vector3> ReturnMuzzlePosition, Func<int> ReturnLeftAmmoCount,
        Action<int> DecreaseAmmoCount, Action SpawnMuzzleFlashEffect, Action SpawnEmptyCartridge)
    {
        _targetLayer = targetLayer;
        _findRange = findRange;

        singleProjectileAttack = new SingleProjectileAttack(weaponName, range, targetLayer, singleBulletCountsInOneShoot, singlePenetratePower, singleDisplacementDecreaseRatio,
            damageDictionary, animator, effectFactory, ReturnMuzzlePosition, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge);

        scatterProjectileGunAttack = new ExplosionScatterProjectileAttack(weaponName, range, targetLayer, scatterBulletCountsInOneShoot, scatterPenetratePower, scatterDisplacementDecreaseRatio,
            pelletCount, spreadOffset, frontDistance, explosionEffectName, scatterDamageDictionary, animator, effectFactory, ReturnMuzzlePosition, ReturnLeftAmmoCount, DecreaseAmmoCount,
            SpawnMuzzleFlashEffect, SpawnEmptyCartridge);
    }

    //public override void ResetLeftBulletCount(int leftBulletCount) 
    //{
    //    singleProjectileAttack.ResetLeftBulletCount(leftBulletCount);
    //    scatterProjectileGunAttack.ResetLeftBulletCount(leftBulletCount);
    //}

    public override bool CanExecute() { return singleProjectileAttack.CanExecute() && scatterProjectileGunAttack.CanExecute(); }

    protected bool IsTargetPlacedInFront()
    {
        Vector3 camPos = _attackPoint.ReturnPosition();
        Vector3 camFowardDir = _attackPoint.ReturnDirection();

        RaycastHit hit;
        Physics.Raycast(camPos, camFowardDir, out hit, _findRange, _targetLayer);
        if (hit.collider == null) return false;

        return true;
    }

    //public override void TurnOffZoomWhenOtherExecute() { }

    public override void Execute()
    {
        _isInFront = IsTargetPlacedInFront();
        if (_isInFront) singleProjectileAttack.Execute();
        else scatterProjectileGunAttack.Execute();
    }

    public override void LinkEvent(WeaponBlackboard blackboard)
    {
        //ReturnRaycastPos = blackboard.ReturnRaycastPos;
        //ReturnRaycastDir = blackboard.ReturnRaycastDir;

        singleProjectileAttack.LinkEvent(blackboard);
        scatterProjectileGunAttack.LinkEvent(blackboard);
    }

    public override void UnlinkEvent(WeaponBlackboard blackboard)
    {
        //ReturnRaycastPos -= blackboard.ReturnRaycastPos;
        //ReturnRaycastDir -= blackboard.ReturnRaycastDir;

        singleProjectileAttack.UnlinkEvent(blackboard);
        scatterProjectileGunAttack.UnlinkEvent(blackboard);
    }
}