using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviorTree.Nodes
{
    public class Attack : Node
    {
        Transform _attackPoint;
        float _attackRadius;
        float _attackDamage;

        float _attackPreDelay;
        float _attackAfterDelay;

        SightComponent _sightComponent;
        Animator _animator;
        //Action<string> ResetAnimatorValue;

        Timer _attackPreTimer;
        Timer _attackAfterTimer;

        //Func<bool> IsTargetInSight;
        //Func<ISightTarget> ReturnTargetInSight;
        //Action<SoundType, bool> PlaySFX;

        public Attack(
            Transform attackPoint,
            float attackDamage,
            float attackPreDelay,
            float attackAfterDelay,
            float attackRadius,
            Animator animator,
            SightComponent sightComponent)
        {
            _attackPreTimer = new Timer();
            _attackAfterTimer = new Timer();

            _animator = animator;
            _sightComponent = sightComponent;

            _attackPoint = attackPoint;
            _attackPreDelay = attackPreDelay;
            _attackAfterDelay = attackAfterDelay;

            _attackDamage = attackDamage;
            _attackRadius = attackRadius;
        }


        public override NodeState Evaluate()
        {
            if (_attackPreTimer.CurrentState == Timer.State.Finish)
            {
                bool isIn = _sightComponent.IsTargetInSight();
                if (isIn == false) return NodeState.FAILURE;

                ITarget target = _sightComponent.ReturnTargetInSight();

                RaycastHit hit;
                Vector3 dir = (target.ReturnPosition() - _attackPoint.position).normalized;
                Physics.Raycast(_attackPoint.position, dir, out hit, _attackRadius);
                Debug.DrawRay(_attackPoint.position, dir * _attackRadius, Color.red, 10);

                if (hit.transform == null) return NodeState.FAILURE;

                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                if (damageable == null) return NodeState.FAILURE;

                damageable.GetDamage(_attackDamage);

                _attackPreTimer.Reset();
            }

            if (_attackAfterTimer.CurrentState == Timer.State.Running) return NodeState.FAILURE;
            if (_attackAfterTimer.CurrentState == Timer.State.Finish) _attackAfterTimer.Reset();

            _animator.SetTrigger("Attack");

            _attackPreTimer.Start(_attackPreDelay);
            _attackAfterTimer.Start(_attackAfterDelay);

            return NodeState.SUCCESS;
        }
    }
}