using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public struct DistanceAreaData
{
    public DistanceAreaData(float minDistance, float maxDistance, float damage)
    {
        _minDistance = minDistance;
        _maxDistance = maxDistance;
        _damage = damage;
    }

    public float _minDistance;

    public float _maxDistance;

    public float _damage;

    [JsonIgnore] public float Damage { get { return _damage; } }

    public bool IsInRange(float distance)
    {
        if (_minDistance <= distance && distance <= _maxDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

[System.Serializable]
public struct DirectionData
{
    public float _frontAttackDamage;
    [JsonIgnore] public float FrontAttackDamage { get { return _frontAttackDamage; } }

    public float _backAttackDamage;

    public DirectionData(float frontAttackDamage, float backAttackDamage)
    {
        _frontAttackDamage = frontAttackDamage;
        _backAttackDamage = backAttackDamage;
    }

    [JsonIgnore] public float BackAttackDamage { get { return _backAttackDamage; } }
}

public class BaseDamageConverter
{
    public virtual float ReturnDamage(IHitable.Area area, float distance) { return 0; }
    public virtual float ReturnDamage(Vector3 playerFoward, Vector3 targetFoward) { return 0; }
}

public class DistanceAreaBasedDamageConverter : BaseDamageConverter
{
    Dictionary<IHitable.Area, DistanceAreaData[]> _damageDictionary;

    public DistanceAreaBasedDamageConverter(Dictionary<IHitable.Area, DistanceAreaData[]> damageDictionary)
    {
        _damageDictionary = damageDictionary;
    }

    public override float ReturnDamage(IHitable.Area area, float distance)
    {
        if (_damageDictionary.ContainsKey(area) == false) return 0;

        for (int i = 0; i < _damageDictionary[area].Length; i++)
        {
            if (_damageDictionary[area][i].IsInRange(distance) == true)
            {
                return _damageDictionary[area][i].Damage;
            }
        }

        // ���� �ش��ϴ� ������ �������� �ʴ´ٸ� --> ������ �Ѿ�ٸ�
        int length = _damageDictionary[area].Length - 1;

        return _damageDictionary[area][length].Damage; // ������ ������ ���� �־���
    }
}

public class DirectionBasedDamageConverter : BaseDamageConverter
{
    DirectionData _directionData;
    float _backAngle = 60;

    public DirectionBasedDamageConverter(DirectionData directionData)
    {
        _directionData = directionData;
    }

    public override float ReturnDamage(Vector3 playerFoward, Vector3 targetFoward)
    {
        float angle = Vector3.Angle(playerFoward, targetFoward);

        if (Mathf.Abs(angle) < _backAngle) // �� ��
        {
            return _directionData.BackAttackDamage;
        }
        else
        {
            return _directionData.FrontAttackDamage;
        }
    }
}