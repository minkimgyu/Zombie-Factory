using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class WaitForNextAttack : Node
    {
        Timer _timer;
        float _delayDuration;

        public WaitForNextAttack(float delayDuration)
        {
            _timer = new Timer();
            _delayDuration = delayDuration;
        }

        public override NodeState Evaluate()
        {
            switch (_timer.CurrentState)
            {
                case Timer.State.Ready:

                    _timer.Start(_delayDuration);
                    return NodeState.SUCCESS;
                case Timer.State.Running:

                    return NodeState.RUNNING;
                case Timer.State.Finish:

                    _timer.Reset();
                    _timer.Start(_delayDuration);
                    return NodeState.SUCCESS;
            }

            return NodeState.FAILURE;
        }

        public override void OnDisableRequested()
        {
            _timer.Reset();
        }
    }
}