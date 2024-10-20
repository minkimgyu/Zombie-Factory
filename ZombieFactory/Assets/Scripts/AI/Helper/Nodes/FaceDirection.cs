using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Swat;

namespace BehaviorTree.Nodes
{
    public class FaceDirection : Node
    {
        Transform _myTransform;
        TPSViewComponent _viewComponent;
        FormationData _formationData;

        Vector3 _direction;

        public FaceDirection(Transform myTransform, FormationData formationData, TPSViewComponent viewComponent)
        {
            _myTransform = myTransform;
            _formationData = formationData;
            _viewComponent = viewComponent;
        }

        public override NodeState Evaluate()
        {
            _viewComponent.View(_formationData.Offset.normalized);
            return NodeState.SUCCESS;
        }
    }
}