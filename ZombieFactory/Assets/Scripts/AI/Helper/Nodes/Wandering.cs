using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class Wandering : Node
    {
        WanderingFSM _wanderFSM;

        public Wandering(WanderingFSM wanderFSM)
        {
            _wanderFSM = wanderFSM;
        }

        public override NodeState Evaluate()
        {
            _wanderFSM.OnUpdate();
            return NodeState.SUCCESS;
        }
    }
}