using BehaviorTree;
using BehaviorTree.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NowFarFromTargetInSight : EvaluatingDistance
{
    SightComponent _sightComponent;

    public NowFarFromTargetInSight(
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

        //Debug.Log($"NowCloseToTarget : {_state}");

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
