using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    abstract public class EvaluatingDistance : Node
    {
        protected enum State
        {
            WithinRange,
            OutOfRange,
        }

        protected Transform _myTransform;
        protected float _distance;
        protected float _gap;

        protected State _state;

        public EvaluatingDistance(Transform myTransform, float distance, float gap)
        {
            _state = State.OutOfRange;

            _myTransform = myTransform;
            _distance = distance;
            _gap = gap;
        }

        // 상황에 따라서 손 봐야할 듯
        protected void SwitchState(Vector3 targetPos)
        {
            float distance = Vector3.Distance(_myTransform.position, targetPos);

            switch (_state)
            {
                case State.WithinRange:
                    if (distance <= _distance) break;
                    _distance -= _gap;

                    _state = State.OutOfRange;

                    break;
                case State.OutOfRange:
                    if (distance > _distance) break;
                    _distance += _gap;

                    _state = State.WithinRange;
                    break;
            }
        }
    }
}