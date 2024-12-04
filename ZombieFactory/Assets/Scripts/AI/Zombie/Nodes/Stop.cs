using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class Stop : Node
    {
        BaseMoveComponent _moveComponent;

        public Stop(BaseMoveComponent moveComponent)
        {
            _moveComponent = moveComponent;
        }

        public override NodeState Evaluate()
        {
            _moveComponent.Stop();
            return NodeState.SUCCESS;
        }
    }
}
