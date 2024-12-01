using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


// WithWeight는 속도 백터의 길이를 받아서 적용시켜주는 인터페이스
//public interface IDisplacement
//{
//    //float DisplacementWeight { get; set; }

//    //float DisplacementDecreaseRatio { get; set; }

//    void OnDisplacementWeightReceived(float displacement);
//}

// 연사가능한 총에는 탄퍼짐이 없어야함 --> 반동으로 처리하기 때문에
public class SingleProjectileAttack : PenetrateAttack
{
    public SingleProjectileAttack(BaseItem.Name weaponName, ISoundControllable.SoundName fireSoundName,  float range, int targetLayer, int fireCountInOnce,
        float penetratePower, float displacementDecreaseRatio, Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary,

        Animator animator, BaseFactory effectFactory, Func<Vector3> ReturnMuzzlePosition, Func<int> ReturnLeftAmmoCount,
        Action<int> DecreaseAmmoCount, Action SpawnMuzzleFlashEffect, Action SpawnEmptyCartridge)

        : base(weaponName, fireSoundName, range, targetLayer, fireCountInOnce, penetratePower, displacementDecreaseRatio,
            damageDictionary, animator, effectFactory, ReturnMuzzlePosition, ReturnLeftAmmoCount,
            DecreaseAmmoCount, SpawnMuzzleFlashEffect, SpawnEmptyCartridge)
    {
    }

    protected Vector3 ReturnOffset(float weight)
    {
        float x = Random.Range(-weight, weight);
        float y = Random.Range(-weight, weight);

        return new Vector3(x, y, 0);
    }

    public override void Execute()
    {
        base.Execute();
        Vector3 multifliedOffset = ReturnOffset(DisplacementWeight);
        Shoot(multifliedOffset);
    }

    //public override void Update()
    //{
    //}
}

// 점사 가능한 총에는 탄 퍼짐 추가

// WithWeight는 탄 퍼짐을 의미함
public class SingleProjectileAttackWithWeight : SingleProjectileAttack // 가중치가 적용되는 공격
{
    WeightApplier _weightApplier;

    public SingleProjectileAttackWithWeight(BaseItem.Name weaponName, ISoundControllable.SoundName fireSoundName, float range, int targetLayer, int fireCountInOnce,
        float penetratePower, float displacementDecreaseRatio, WeightApplier weightApplier, Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary,


        Animator animator, BaseFactory effectFactory, Func<Vector3> ReturnMuzzlePosition, Func<int> ReturnLeftAmmoCount,
        Action<int> DecreaseAmmoCount, Action SpawnMuzzleFlashEffect, Action SpawnEmptyCartridge)

        : base(weaponName, fireSoundName, range, targetLayer, fireCountInOnce, penetratePower, displacementDecreaseRatio,
            damageDictionary, animator, effectFactory, ReturnMuzzlePosition, ReturnLeftAmmoCount,
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
