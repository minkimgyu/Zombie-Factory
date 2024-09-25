using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FSM;
using AI;

namespace BehaviorTree.Nodes
{
    public class Follow : Node
    {
        SightComponent _sightComponent;
        PathSeeker _pathSeeker;

        float _moveSpeed;

        TPSViewComponent _viewComponent;
        TPSMoveComponent _moveComponent;

        public Follow(
            SightComponent sightComponent,
            PathSeeker pathSeeker,
            float moveSpeed,
            TPSViewComponent viewComponent,
            TPSMoveComponent moveComponent)
        {
            _sightComponent = sightComponent;
            _pathSeeker = pathSeeker;
            _moveSpeed = moveSpeed;
            _viewComponent = viewComponent;
            _moveComponent = moveComponent;
        }

        public override NodeState Evaluate()
        {
            ITarget target = _sightComponent.ReturnTargetInSight();
            Vector3 dir = _pathSeeker.ReturnDirection(target.ReturnPosition());

            bool isFinish = _pathSeeker.IsFinish();
            if (isFinish) return NodeState.SUCCESS;

            _viewComponent.View(dir);
            _moveComponent.Move(dir, 5);
            return NodeState.SUCCESS;
        }
    }
}