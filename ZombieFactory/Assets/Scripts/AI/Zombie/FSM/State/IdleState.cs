using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace AI
{
    public class IdleState : BaseZombieState
    {
        public enum State
        {
            Stop,
            Rotate,
            Move
        }

        State _state;

        float _stateChangeDuration;
        float _speed;

        Timer _stateTimer;
        TPSViewComponent _viewComponent;
        TPSMoveComponent _moveComponent;

        SightComponent _sightComponent;

        List<ITarget> _targetList;

        Quaternion _rotation;

        public IdleState(
            ZombieFSM fsm,

            float speed,
            float stateChangeDuration,

            TPSViewComponent viewComponent,
            TPSMoveComponent moveComponent,

            SightComponent sightComponent) : base(fsm)
        {
            _viewComponent = viewComponent;
            _moveComponent = moveComponent;

            _sightComponent = sightComponent;

            _state = State.Stop;
            _stateTimer = new Timer();

            _speed = speed;
            _stateChangeDuration = stateChangeDuration;
        }

        public override void OnNoiseEnter()
        {
            _baseFSM.SetState(Zombie.State.NoiseTracking);
        }

        public override void OnStateEnter()
        {
            _state = State.Stop;
            _stateTimer.Start(_stateChangeDuration);
        }

        public override void OnStateFixedUpdate()
        {
            _viewComponent.RotateRigidbody();
        }

        Vector3 forwardDir;

        public override void OnStateUpdate()
        {
            bool isInSight = _sightComponent.IsTargetInSight();
            if(isInSight)
            {
                _baseFSM.SetState(Zombie.State.TargetFollowing);
                return;
            }

            switch (_state)
            {
                case State.Stop:
                    _moveComponent.Stop();
                    break;
                case State.Rotate:
                    _viewComponent.View(forwardDir);
                    break;
                case State.Move:
                    _viewComponent.View(forwardDir);
                    _moveComponent.Move(forwardDir, 5);
                    break;
            }

            if(_stateTimer.CurrentState == Timer.State.Finish)
            {
                int size = Enum.GetValues(typeof(State)).Length;
                _state = (State)Random.Range(0, size);

                Debug.Log(_state);

                Vector3 dir;
                switch (_state)
                {
                    case State.Rotate:
                        dir = Random.insideUnitCircle;
                        forwardDir = new Vector3(dir.x, 0, dir.y);
                        break;
                    case State.Move:
                        dir = Random.insideUnitCircle;
                        forwardDir = new Vector3(dir.x, 0, dir.y);
                        break;
                }

                _stateTimer.Reset();
                _stateTimer.Start(_stateChangeDuration);
                return;
            }
        }


    }
}