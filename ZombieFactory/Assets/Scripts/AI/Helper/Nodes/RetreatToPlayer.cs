using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Swat;

namespace BehaviorTree.Nodes
{
    public class RetreatToPlayer : Node
    {
        ITarget _target;

        // 오프셋 포지션 값이 더 필요함
        Vector3 _offset;

        PathSeeker _pathSeeker;
        TPSMoveComponent _moveComponent;
        float _moveSpeed;

        FormationData _formationData;

        public RetreatToPlayer(
            PathSeeker pathSeeker,
            TPSMoveComponent moveComponent,

            FormationData formationData,

            float moveSpeed)
        {
            _pathSeeker = pathSeeker;
            _moveComponent = moveComponent;
            _moveSpeed = moveSpeed;
            _formationData = formationData;
        }

        public override NodeState Evaluate()
        {
            ITarget target = _formationData.Target;
            if (target as UnityEngine.Object == null) return NodeState.FAILURE;

            Vector3 dir = _pathSeeker.ReturnDirection(target.ReturnPosition() + _formationData.Offset);
            bool nowFinish = _pathSeeker.NowFinish();
            if (nowFinish == true)
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
