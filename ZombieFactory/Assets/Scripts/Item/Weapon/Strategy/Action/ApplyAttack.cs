using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ApplyAttack : ActionState
{
    ///// <summary>
    ///// ī�޶� ��ġ�� ��ȯ���ش�.
    ///// </summary>
    //protected Func<Vector3> ReturnRaycastPos;

    ///// <summary>
    ///// ī�޶� ������ ��ȯ���ش�.
    ///// </summary>
    //protected Func<Vector3> ReturnRaycastDir;

    protected Animator _animator;

    /// <summary>
    /// ���� ���� ��ġ
    /// </summary>
    protected IPoint _attackPoint;

    protected float _range;
    protected int _targetLayer;
    protected BaseDamageConverter _damageConverter;

    /// <summary>
    /// ���⸦ ������ ����� �ִϸ��̼��� �����ų �� ȣ��
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
        // ���̾ 2�̴�.
        _animator.Play(aniName, 0, 0);
        OnPlayOwnerAnimation?.Invoke(_weaponName.ToString() + aniName, 0, 0);
    }

    protected virtual void PlayAnimation(string aniName, int index)
    {
        _animator.Play(aniName + index, 0, 0);
        OnPlayOwnerAnimation?.Invoke(_weaponName.ToString() + aniName + index, 0, 0);
    }


    // �������� �����ϴ� �Լ��� ���⿡ ������ֱ� ex) �ѱ��� ������ �Լ�, Į ������ �Լ�
    protected virtual float CalculateDamage(IHitable hitable, PenetrateData data, float decreaseRatio) { return default; }

    protected virtual float CalculateDamage(IHitable hitable) { return default; }

    protected virtual void ApplyDamage(IHitable hitable, IEffectable effectable, RaycastHit hit) { }

    protected virtual void ApplyDamage(IHitable hitable, IEffectable effectable, PenetrateData data, float decreaseRatio) { }
}
