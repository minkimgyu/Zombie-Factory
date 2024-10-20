using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class StopWandering : Node
    {
        WanderingFSM _wandering;

        public StopWandering(WanderingFSM wandering)
        {
            _wandering = wandering;
        }

        public override NodeState Evaluate()
        {
            _wandering.GoToStopState();
            return NodeState.SUCCESS;
        }
    }
}