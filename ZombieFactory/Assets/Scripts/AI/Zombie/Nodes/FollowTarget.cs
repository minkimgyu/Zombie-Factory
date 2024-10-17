using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class FollowTarget : Node
    {
        ITarget _target;

        PathSeeker _pathSeeker;
        TPSMoveComponent _moveComponent;
        float _moveSpeed;

        public FollowTarget(
            PathSeeker pathSeeker,
            TPSMoveComponent moveComponent,
            float moveSpeed)
        {
            _pathSeeker = pathSeeker;
            _moveComponent = moveComponent;
            _moveSpeed = moveSpeed;
        }

        public void ResetTarget(ITarget target)
        {
            _target = target;
        }

        public override NodeState Evaluate()
        {
            if (_target as UnityEngine.Object == null) return NodeState.FAILURE;

            Vector3 dir = _pathSeeker.ReturnDirection(_target.ReturnPosition());

            if (_pathSeeker.IsFinish() == true)
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