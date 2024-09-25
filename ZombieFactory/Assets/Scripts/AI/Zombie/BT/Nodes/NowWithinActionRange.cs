using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviorTree.Nodes
{
    public class NowWithinActionRange : Node
    {
        enum State
        {
            WithinRange,
            OutsideOfRange
        }

        State _state;

        float _finalRange = 0;
        float _range;
        float _rangeOffset;
        Transform _myTransform;
        SightComponent _sightComponent;

        public NowWithinActionRange(Transform myTransform, SightComponent sightComponent, float range, float rangeOffset)
        {
            _sightComponent = sightComponent;

            _range = range;
            _rangeOffset = rangeOffset;
            _myTransform = myTransform;
        }

        void SetState(State state)
        {
            if (_state == state) return;
            _state = state;

            switch (_state)
            {
                case State.WithinRange:
                    _finalRange = _range + _rangeOffset;
                    break;
                case State.OutsideOfRange:
                    _finalRange = _range;
                    break;
            }
        }

        public override NodeState Evaluate()
        {
            ITarget target = _sightComponent.ReturnTargetInSight();
            if (target == null) return NodeState.FAILURE;

            float distanceFromTarget = Vector3.Distance(_myTransform.position, target.ReturnPosition());
            if (distanceFromTarget < _finalRange)
            {
                SetState(State.WithinRange);
                return NodeState.SUCCESS;
            }

            SetState(State.OutsideOfRange);
            return NodeState.FAILURE;
        }
    }
}