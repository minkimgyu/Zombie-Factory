using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

abstract public class BaseEffect : PoolObject
{
    public enum Name
    {
        PenetrateBulletHole, // �� ���� �� �Ѿ� �ڱ�
        NonPenetrateBulletHole, // �� ���� �� �Ѿ� �ڱ�


        ObjectFragmentation, // �� ���� ���� �� �Ѿ� ����ȭ
        KnifeMark, // Į�� �ڱ��� �� ���

        TrajectoryLine, // �Ѿ� �߻� ���� ����Ʈ
        Explosion, // ���� ���� ����Ʈ

        DamageTxt, // ������ �ؽ�Ʈ
    }

    [SerializeField] protected float _duration = 5;

    public virtual void Play()
    {
        StartTimer(_duration);
    }

    public virtual void ResetData(Vector3 hitPosition) { }

    public virtual void ResetData(Vector3 hitPosition, Vector3 shootPosition) { }

    public virtual void ResetData(Vector3 hitPosition, Vector3 hitNormal, Quaternion holeRotation) { }

    public virtual void ResetData(Vector3 hitPosition, Vector3 hitNormal, Quaternion holeRotation, float damamge) { }
}
