using BehaviorTree;
using BehaviorTree.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowCloseToTargetInSight : EvaluatingDistance
{
    SightComponent _sightComponent;

    public NowCloseToTargetInSight(
        SightComponent sightComponent,
        Transform myTransform,
        float distance,
        float gap) : base(myTransform, distance, gap)
    {
        _sightComponent = sightComponent;
    }

    public override NodeState Evaluate()
    {
        bool nowTargetInSight = _sightComponent.IsTargetInSight();
        if (nowTargetInSight == false) return NodeState.FAILURE;

        ITarget target = _sightComponent.ReturnTargetInSight();
        SwitchState(target.ReturnTargetPoint().position);

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
