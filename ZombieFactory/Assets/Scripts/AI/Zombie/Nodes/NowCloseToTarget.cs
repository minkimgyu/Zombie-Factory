using BehaviorTree;
using BehaviorTree.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowCloseToTarget : EvaluatingDistance
{
    ITarget _target;

    public NowCloseToTarget(Transform myTransform, float distance, float gap) : base(myTransform, distance, gap)
    {
    }

    public void ResetTarget(ITarget target)
    {
        _target = target;
    }

    public override NodeState Evaluate()
    {
        // fake null ÆÄ¾Ç
        if (_target as UnityEngine.Object == null) return NodeState.FAILURE;

        SwitchState(_target.ReturnPosition());

        switch (_state)
        {
            case State.WithinRange:
                return NodeState.SUCCESS;

            case State.OutOfRange:
                return NodeState.FAILURE;

            default:
                return NodeState.FAILURE;
        }
    }
}
