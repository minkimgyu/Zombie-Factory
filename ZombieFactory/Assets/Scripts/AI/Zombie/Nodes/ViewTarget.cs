using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class ViewTarget : Node
    {
        Transform _sightPoint;
        SightComponent _sightComponent;
        BaseViewComponent _viewComponent;

        public ViewTarget(Transform sightPoint, SightComponent sightComponent, BaseViewComponent viewComponent)
        {
            _sightPoint = sightPoint;
            _sightComponent = sightComponent;
            _viewComponent = viewComponent;
        }

        public override NodeState Evaluate()
        {
            ITarget target =  _sightComponent.ReturnTargetInSight();
            Vector3 targetPos = target.ReturnSightPoint().position;

            // sightPoint 2개 가져와서 방향 구하기
            Vector3 dir = (targetPos - _sightPoint.position).normalized;
            _viewComponent.View(dir);
            return NodeState.SUCCESS;
        }
    }
}
