using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackComponent
{
    Transform _raycastPoint;
    float _attackRadius;
    float _attackDamage;

    float _attackPreDelay;
    float _attackAfterDelay;

    Animator _animator;

    Timer _attackPreTimer;
    Timer _attackAfterTimer;

    public MeleeAttackComponent(
        Transform raycastPoint,
        float attackDamage,
        float attackPreDelay,
        float attackAfterDelay,
        float attackRadius,
        Animator animator)
    {
        _attackPreTimer = new Timer();
        _attackAfterTimer = new Timer();

        _animator = animator;

        _raycastPoint = raycastPoint;
        _attackPreDelay = attackPreDelay;
        _attackAfterDelay = attackAfterDelay;

        _attackDamage = attackDamage;
        _attackRadius = attackRadius;
    }

    public void Attack(Vector3 dir)
    {
        if (_attackPreTimer.CurrentState == Timer.State.Finish)
        {
            _attackPreTimer.Reset();

            RaycastHit hit;
            Physics.Raycast(_raycastPoint.position, dir, out hit, _attackRadius);
            Debug.DrawRay(_raycastPoint.position, dir * _attackRadius, Color.red, 10);

            if (hit.transform == null) return;

            IHitable hitable = hit.transform.GetComponent<IHitable>();
            if (hitable == null) return;

            hitable.OnHit(_attackDamage, hit.point, hit.normal);
        }

        if (_attackAfterTimer.CurrentState == Timer.State.Running) return;
        if (_attackAfterTimer.CurrentState == Timer.State.Finish) _attackAfterTimer.Reset();

        _animator.SetTrigger("Attack");

        _attackPreTimer.Start(_attackPreDelay);
        _attackAfterTimer.Start(_attackAfterDelay);
    }
}
