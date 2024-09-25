using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviorTree.Nodes
{
    public class RotateTowardTarget : Node
    {
        Transform _myTransform;
        SightComponent _sightComponent;
        TPSViewComponent _tpsViewComponent;

        public RotateTowardTarget(Transform myTransform, SightComponent sightComponent, TPSViewComponent tPSViewComponent)
        {
            _myTransform = myTransform;
            _sightComponent = sightComponent;
            _tpsViewComponent = tPSViewComponent;
        }

        public override NodeState Evaluate()
        {
            ITarget target = _sightComponent.ReturnTargetInSight();
            Vector3 targetPos = target.ReturnPosition();

            Vector3 dir = (new Vector3(targetPos.x, _myTransform.position.y, targetPos.z) - _myTransform.position).normalized;

            Debug.Log(dir);

            _tpsViewComponent.View(dir);

            return NodeState.SUCCESS;
        }
    }
}