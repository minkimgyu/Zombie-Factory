using BehaviorTree;
using BehaviorTree.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowFarFromTarget : EvaluatingDistance
{
    ITarget _target;

    public NowFarFromTarget(Transform myTransform, float distance, float gap) : base(myTransform, distance, gap)
    {
    }

    public void ResetTarget(ITarget target)
    {
        _target = target;
    }

    public override NodeState Evaluate()
    {
        SwitchState(_target.ReturnPosition());

        switch (_state)
        {
            case State.OutOfRange:
                return NodeState.SUCCESS;

            case State.WithinRange:
                return NodeState.FAILURE;

            default:
                return NodeState.FAILURE;
        }
    }
}
