using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class Attack : Node
    {
        Transform _myTransform;
        Transform _raycastPoint;
        float _attackRadius;
        float _attackDamage;

        float _attackPreDelay;
        float _attackAfterDelay;

        SightComponent _sightComponent;
        Animator _animator;

        Timer _attackPreTimer;
        Timer _attackAfterTimer;

        public Attack(
        Transform myTransform,
        Transform raycastPoint,
        float attackDamage,
        float attackPreDelay,
        float attackAfterDelay,
        float attackRadius,
        SightComponent sightComponent,
        Animator animator)
        {
            _myTransform = myTransform;
            _raycastPoint = raycastPoint;

            _attackPreTimer = new Timer();
            _attackAfterTimer = new Timer();

            _sightComponent = sightComponent;
            _animator = animator;

            _attackPreDelay = attackPreDelay;
            _attackAfterDelay = attackAfterDelay;

            _attackDamage = attackDamage;
            _attackRadius = attackRadius;
        }

        public override NodeState Evaluate()
        {
            ITarget target = _sightComponent.ReturnTargetInSight();
            Vector3 targetPos = target.ReturnPosition();
            Vector3 dir = (targetPos - _myTransform.position).normalized;

            _animator.SetTrigger("Attack");
            _attackPreTimer.Start(_attackPreDelay);

            if (_attackPreTimer.CurrentState == Timer.State.Finish)
            {
                _attackPreTimer.Reset();

                RaycastHit hit;
                Physics.Raycast(_raycastPoint.position, dir, out hit, _attackRadius);
                Debug.DrawRay(_raycastPoint.position, dir * _attackRadius, Color.red, 10);

                if (hit.transform == null) return NodeState.SUCCESS;

                IHitable hitable = hit.transform.GetComponent<IHitable>();
                if (hitable == null) return NodeState.SUCCESS;

                hitable.OnHit(_attackDamage, hit.point, hit.normal);
            }

            return NodeState.SUCCESS;
        }
    }
}