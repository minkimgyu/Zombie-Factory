using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class NoiseTrackingState : BaseZombieState
    {
        PathSeeker _pathSeeker;
        Queue<Vector3> _noiseQueue;

        float _moveSpeed;

        TPSViewComponent _viewComponent;
        TPSMoveComponent _moveComponent;

        SightComponent _sightComponent;

        public NoiseTrackingState(
            ZombieFSM fsm,

            float moveSpeed,

            TPSViewComponent viewComponent,
            TPSMoveComponent moveComponent,

            SightComponent sightComponent,

            PathSeeker pathSeeker,
            Queue<Vector3> noiseQueue) : base(fsm)
        {
            _moveSpeed = moveSpeed;

            _viewComponent = viewComponent;
            _moveComponent = moveComponent;
            _sightComponent = sightComponent;

            _pathSeeker = pathSeeker;
            _noiseQueue = noiseQueue;
        }

        Vector3 _targetPos;
        Vector3 _dir;

        public override void OnStateExit()
        {
            _noiseQueue.Clear();
        }

        public override void OnTargetEnter()
        {
            _baseFSM.SetState(Zombie.State.TargetFollowing);
        }

        public override void OnStateEnter()
        {
            _targetPos = _noiseQueue.Dequeue();
        }

        public override void OnStateUpdate()
        {
            bool isInSight = _sightComponent.IsTargetInSight();
            if (isInSight)
            {
                _baseFSM.SetState(Zombie.State.TargetFollowing);
                return;
            }

            _dir = _pathSeeker.ReturnDirection(_targetPos);
            _viewComponent.View(_dir);
            _moveComponent.Move(_dir, _moveSpeed);

            bool isFinish = _pathSeeker.IsFinish();
            if(isFinish && _noiseQueue.Count > 0)
            {
                _targetPos = _noiseQueue.Dequeue();
            }
            else if (isFinish && _noiseQueue.Count == 0)
            {
                _baseFSM.SetState(Zombie.State.Idle);
                return;
            }
        }
    }
}