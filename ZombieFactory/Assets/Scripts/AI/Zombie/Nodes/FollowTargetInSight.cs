using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class FollowTargetInSight : Node
    {
        SightComponent _sightComponent;

        PathSeeker _pathSeeker;
        TPSMoveComponent _moveComponent;
        float _moveSpeed;

        public FollowTargetInSight(
            SightComponent sightComponent,
            PathSeeker pathSeeker,
            TPSMoveComponent moveComponent,
            float moveSpeed)
        {
            _sightComponent = sightComponent;
            _pathSeeker = pathSeeker;
            _moveComponent = moveComponent;
            _moveSpeed = moveSpeed;
        }

        public override NodeState Evaluate()
        {
            bool nowTargetInSight = _sightComponent.IsTargetInSight();
            if (nowTargetInSight == false) return NodeState.FAILURE;

            ITarget target = _sightComponent.ReturnTargetInSight();
            Vector3 dir = _pathSeeker.ReturnDirection(target.ReturnPosition());

            if (_pathSeeker.NowFinish() == true)
            {
                _moveComponent.Stop();
            }
            else
            {
                _moveComponent.Move(dir, _moveSpeed);
            }

            return NodeState.SUCCESS;
        }
    }
}