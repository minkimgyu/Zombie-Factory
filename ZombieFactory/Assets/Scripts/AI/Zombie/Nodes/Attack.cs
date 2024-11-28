using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace BehaviorTree.Nodes
{
    public class Attack : Node
    {
        Transform _sightPoint;
        float _attackRadius;
        float _attackDamage;

        SightComponent _sightComponent;

        public Attack(
        Transform sightPoint,
        float attackDamage,
        float attackRadius,
        SightComponent sightComponent)
        {
            _sightPoint = sightPoint;

            _sightComponent = sightComponent;

            _attackDamage = attackDamage;
            _attackRadius = attackRadius;
        }

        public override NodeState Evaluate()
        {
            ITarget target = _sightComponent.ReturnTargetInSight();
            Vector3 targetPos = target.ReturnSightPoint().position;
            Vector3 dir = (targetPos - _sightPoint.position).normalized;

            // 시아 지점에서 시아 지점으로 공격 진행

            RaycastHit hit;
            Physics.Raycast(_sightPoint.position, dir, out hit, _attackRadius, LayerMask.GetMask("Target"));
            //Debug.DrawRay(_raycastPoint.position, dir * _attackRadius, Color.red, 10);

            if (hit.transform == null) return NodeState.SUCCESS;

            IHitable hitable = hit.transform.GetComponent<IHitable>();
            if (hitable == null) return NodeState.SUCCESS;

            hitable.OnHit(_attackDamage, hit.point, hit.normal);
            Debug.Log("Attack");

            return NodeState.SUCCESS;
        }
    }
}