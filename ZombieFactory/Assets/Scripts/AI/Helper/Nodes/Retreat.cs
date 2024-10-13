using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class Retreat : Node
    {
        ITarget _target;

        // ������ ������ ���� �� �ʿ���
        Vector3 _offset;

        PathSeeker _pathSeeker;
        TPSMoveComponent _moveComponent;
        float _moveSpeed;

        public Retreat(
            PathSeeker pathSeeker,
            TPSMoveComponent moveComponent,
            float moveSpeed)
        {
            _pathSeeker = pathSeeker;
            _moveComponent = moveComponent;
            _moveSpeed = moveSpeed;
            _offset = Vector3.zero;
        }

        public void ResetOffset(Vector3 offset)
        {
            _offset = offset;
        }

        public void ResetTarget(ITarget target)
        {
            _target = target;
        }

        public override NodeState Evaluate()
        {
            if (_target as UnityEngine.Object == null) return NodeState.FAILURE;

            Vector3 dir = _pathSeeker.ReturnDirection(_target.ReturnPosition() + _offset);
            _moveComponent.Move(dir, _moveSpeed);

            return NodeState.SUCCESS;
        }
    }
}
