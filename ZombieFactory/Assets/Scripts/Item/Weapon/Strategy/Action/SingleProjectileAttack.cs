using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


// WithWeight�� �ӵ� ������ ���̸� �޾Ƽ� ��������ִ� �������̽�
//public interface IDisplacement
//{
//    //float DisplacementWeight { get; set; }

//    //float DisplacementDecreaseRatio { get; set; }

//    void OnDisplacementWeightReceived(float displacement);
//}

// ���簡���� �ѿ��� ź������ ������� --> �ݵ����� ó���ϱ� ������
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

// ���� ������ �ѿ��� ź ���� �߰�

// WithWeight�� ź ������ �ǹ���
public class SingleProjectileAttackWithWeight : SingleProjectileAttack // ����ġ�� ����Ǵ� ����
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
