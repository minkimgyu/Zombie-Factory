using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class ViewTarget : Node
    {
        Transform _myTransform;
        SightComponent _sightComponent;
        TPSViewComponent _viewComponent;

        public ViewTarget(Transform myTransform, SightComponent sightComponent, TPSViewComponent viewComponent)
        {
            _myTransform = myTransform;
            _sightComponent = sightComponent;
            _viewComponent = viewComponent;
        }

        public override NodeState Evaluate()
        {
            ITarget target =  _sightComponent.ReturnTargetInSight();
            Vector3 targetPos = target.ReturnPosition();

            Vector3 dir = (targetPos - _myTransform.position).normalized;
            dir = new Vector3(dir.x, 0, dir.z);

            _viewComponent.View(dir);
            return NodeState.SUCCESS;
        }
    }
}
