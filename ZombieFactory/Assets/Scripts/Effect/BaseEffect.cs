using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

abstract public class BaseEffect : PoolObject
{
    public enum Name
    {
        PenetrateBulletHole, // 벽 관통 시 총알 자국
        NonPenetrateBulletHole, // 벽 관통 시 총알 자국


        WallFragmentation, // 벽 관통 실패 시 총알 파편화
        KnifeMark, // 칼로 자국이 난 경우

        TrajectoryLine, // 총알 발사 흔적 이펙트
        Explosion, // 샷건 폭발 이펙트

        DamageTxt, // 데미지 텍스트
        ObjectFragmentation // 관통하여 오브젝트가 부서지는 경우
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
