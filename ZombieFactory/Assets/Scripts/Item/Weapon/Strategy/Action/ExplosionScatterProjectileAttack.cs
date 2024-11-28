using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScatterProjectileAttack : ScatterProjectileAttack // 산탄은 가중치가 적용되지 않음
{
    string _explosionEffectName;
    float _frontDistance;

    public ExplosionScatterProjectileAttack(BaseItem.Name weaponName, float range, int targetLayer, int fireCountInOnce,
        float penetratePower, float displacementDecreaseRatio, int pelletCount, float spreadOffset, float frontDistance,
        string explosionEffectName, Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary,

        Animator animator, BaseFactory effectFactory, Func<Vector3> ReturnMuzzlePosition, Func<int> ReturnLeftAmmoCount,
        Action<int> DecreaseAmmoCount, Action SpawnMuzzleFlashEffect, Action SpawnEmptyCartridge)

        : base(weaponName, range, targetLayer, fireCountInOnce, penetratePower, displacementDecreaseRatio, pelletCount, spreadOffset,
            damageDictionary, animator, effectFactory, ReturnMuzzlePosition, ReturnLeftAmmoCount,
            DecreaseAmmoCount, SpawnMuzzleFlashEffect, SpawnEmptyCartridge)
    {
        _frontDistance = frontDistance;
        _explosionEffectName = explosionEffectName;
    }

    public override void Execute()
    {
        Vector3 camFowardDir = _attackPoint.forward;
        Vector3 camPos = _attackPoint.position;

        _frontPosition = camPos + (camFowardDir * _frontDistance);

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.Explosion);
        effect.ResetData(_frontPosition);
        base.Execute();
    }
}