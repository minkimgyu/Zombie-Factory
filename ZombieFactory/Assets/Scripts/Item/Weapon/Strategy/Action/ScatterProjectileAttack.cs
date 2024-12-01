using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScatterProjectileAttack : PenetrateAttack // 산탄은 가중치가 적용되지 않음
{
    float _spreadOffset;

    int _storedFireCount;
    int _pelletCount;

    protected Vector3 _frontPosition = Vector3.zero;

    public ScatterProjectileAttack(BaseItem.Name weaponName, ISoundControllable.SoundName fireSoundName, float range, int targetLayer, int fireCountInOnce,
        float penetratePower, float displacementDecreaseRatio, int pelletCount, float spreadOffset, //int nextFireCount,
        Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary,

        Animator weaponAnimator, BaseFactory effectFactory, Func<Vector3> ReturnMuzzlePosition, Func<int> ReturnLeftAmmoCount,
        Action<int> DecreaseAmmoCount, Action SpawnMuzzleFlashEffect, Action SpawnEmptyCartridge)

        : base(weaponName, fireSoundName, range, targetLayer, fireCountInOnce, penetratePower, displacementDecreaseRatio,
            damageDictionary, weaponAnimator, effectFactory, ReturnMuzzlePosition, ReturnLeftAmmoCount,
            DecreaseAmmoCount, SpawnMuzzleFlashEffect, SpawnEmptyCartridge)
    {
        _pelletCount = pelletCount;
        _spreadOffset = spreadOffset;
        _storedFireCount = _fireCountInOnce; // _storedFireCount에 저장해둔다.
    }

    List<Vector3> ReturnOffsetDistance(float weight, int fireCount)
    {
        List<Vector3> offsetDistance = new List<Vector3>();

        for (int i = 0; i < fireCount; i++)
        {
            float x = Random.Range(-_spreadOffset - weight, _spreadOffset + weight);
            float y = Random.Range(-_spreadOffset - weight, _spreadOffset + weight);
            float z = Random.Range(-_spreadOffset - weight, _spreadOffset + weight);

            offsetDistance.Add(new Vector3(x, y, z));
        }


        return offsetDistance;
    }

    public override void Execute()
    {
        int leftAmmoCount = ReturnLeftAmmoCount();
        if (_storedFireCount > leftAmmoCount) _fireCountInOnce = leftAmmoCount; //  _fireCountInOnce 재지정
        else _fireCountInOnce = _storedFireCount;

        base.Execute();

        List<Vector3> offsetDistances = ReturnOffsetDistance(DisplacementWeight, _pelletCount);
        for (int i = 0; i < offsetDistances.Count; i++)
        {
            Shoot(offsetDistances[i], _frontPosition);
        }
    }
}

public class ScatterProjectileAttackWithWeight : ScatterProjectileAttack
{
    WeightApplier _weightApplier;

    public ScatterProjectileAttackWithWeight(BaseItem.Name weaponName, ISoundControllable.SoundName fireSoundName, float range, int targetLayer, int fireCountInOnce,
        float penetratePower, float displacementDecreaseRatio, int pelletCount, float spreadOffset, WeightApplier weightApplier,
        Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary,

        Animator weaponAnimator, BaseFactory effectFactory, Func<Vector3> ReturnMuzzlePosition, Func<int> ReturnLeftAmmoCount,
        Action<int> DecreaseAmmoCount, Action SpawnMuzzleFlashEffect, Action SpawnEmptyCartridge)

        : base(weaponName, fireSoundName, range, targetLayer, fireCountInOnce, penetratePower, displacementDecreaseRatio, pelletCount,
            spreadOffset, damageDictionary, weaponAnimator, effectFactory, ReturnMuzzlePosition, ReturnLeftAmmoCount,
            DecreaseAmmoCount, SpawnMuzzleFlashEffect, SpawnEmptyCartridge)
    {
        _weightApplier = weightApplier;
    }

    public override void Execute()
    {
        _additionalWeight = _weightApplier.StoredWeight;
        base.Execute();
        _weightApplier.MultiplyWeight();
    }

    public override void OnUpdate()
    {
        _weightApplier.OnUpdate();
    }
}
