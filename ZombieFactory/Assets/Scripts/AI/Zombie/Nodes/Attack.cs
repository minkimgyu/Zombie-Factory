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

        SightComponent _sightComponent;

        public Attack(
        Transform myTransform,
        Transform raycastPoint,
        float attackDamage,
        float attackRadius,
        SightComponent sightComponent)
        {
            _myTransform = myTransform;
            _raycastPoint = raycastPoint;

            _sightComponent = sightComponent;

            _attackDamage = attackDamage;
            _attackRadius = attackRadius;
        }

        public override NodeState Evaluate()
        {
            ITarget target = _sightComponent.ReturnTargetInSight();
            Vector3 targetPos = target.ReturnPosition();
            Vector3 dir = (targetPos - _myTransform.position).normalized;

            RaycastHit hit;
            Physics.Raycast(_raycastPoint.position, dir, out hit, _attackRadius, LayerMask.GetMask("Target"));
            Debug.DrawRay(_raycastPoint.position, dir * _attackRadius, Color.red, 10);

            if (hit.transform == null) return NodeState.SUCCESS;

            IHitable hitable = hit.transform.GetComponent<IHitable>();
            if (hitable == null) return NodeState.SUCCESS;

            hitable.OnHit(_attackDamage, hit.point, hit.normal);
            Debug.Log("Attack");

            return NodeState.SUCCESS;
        }
    }
}