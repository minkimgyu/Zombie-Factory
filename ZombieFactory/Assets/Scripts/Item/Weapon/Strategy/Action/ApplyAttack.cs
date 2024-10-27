using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ApplyAttack : ActionState
{
    ///// <summary>
    ///// 카메라 위치를 반환해준다.
    ///// </summary>
    //protected Func<Vector3> ReturnRaycastPos;

    ///// <summary>
    ///// 카메라 방향을 반환해준다.
    ///// </summary>
    //protected Func<Vector3> ReturnRaycastDir;

    protected Animator _animator;

    /// <summary>
    /// 공격 적용 위치
    /// </summary>
    protected IPoint _attackPoint;

    protected float _range;
    protected int _targetLayer;
    protected BaseDamageConverter _damageConverter;

    /// <summary>
    /// 무기를 소유한 대상의 애니메이션을 실행시킬 때 호출
    /// </summary>
    /// 
    protected Action<string, int, float> OnPlayOwnerAnimation;

    //bool _isMainAction;
    protected BaseItem.Name _weaponName;

    public ApplyAttack(BaseItem.Name weaponName, float range, int targetLayer, Animator animator)
    {
        _weaponName = weaponName;
        _range = range;
        _targetLayer = targetLayer;
        _animator = animator;
    }

    protected virtual void PlayAnimation(string aniName)
    {
        // 레이어가 2이다.
        _animator.Play(aniName, 0, 0);
        OnPlayOwnerAnimation?.Invoke(_weaponName.ToString() + aniName, 0, 0);
    }

    protected virtual void PlayAnimation(string aniName, int index)
    {
        _animator.Play(aniName + index, 0, 0);
        OnPlayOwnerAnimation?.Invoke(_weaponName.ToString() + aniName + index, 0, 0);
    }


    // 데미지를 적용하는 함수는 여기에 만들어주기 ex) 총기형 데미지 함수, 칼 데미지 함수
    protected virtual float CalculateDamage(IHitable hitable, PenetrateData data, float decreaseRatio) { return default; }

    protected virtual float CalculateDamage(IHitable hitable) { return default; }

    protected virtual void ApplyDamage(IHitable hitable, IEffectable effectable, RaycastHit hit) { }

    protected virtual void ApplyDamage(IHitable hitable, IEffectable effectable, PenetrateData data, float decreaseRatio) { }
}
