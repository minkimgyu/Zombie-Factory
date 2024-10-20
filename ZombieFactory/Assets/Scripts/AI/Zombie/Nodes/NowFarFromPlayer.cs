using BehaviorTree;
using BehaviorTree.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Swat;

public class NowFarFromPlayer : EvaluatingDistance
{
    FormationData _formationData;

    public NowFarFromPlayer(
        Transform myTransform,
        FormationData formationData,
        float distance,
        float gap) : base(myTransform, distance, gap)
    {
        _formationData = formationData;
    }

    public override NodeState Evaluate()
    {
        ITarget target = _formationData.Target;
        if (target as UnityEngine.Object == null) return NodeState.FAILURE;

        SwitchState(target.ReturnPosition());

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
