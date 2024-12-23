using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AI.Zombie
{
    public class IdleState : BaseZombieState
    {
        float _stateChangeDuration;
        float _speed;

        SightComponent _sightComponent;
        Animator _animator; // 인터페이스로 처리하기

        BaseViewComponent _viewComponent;
        BaseMoveComponent _moveComponent;

        WanderingFSM _wanderingFSM;

        public IdleState(
            ZombieFSM fsm,

            float speed,
            float stateChangeDuration,
            float moveRange,

            BaseViewComponent viewComponent,
            BaseMoveComponent moveComponent,
            Animator animator,
            PathSeeker pathSeeker,

            Transform myTransform,
            SightComponent sightComponent) : base(fsm)
        {
            _viewComponent = viewComponent;
            _moveComponent = moveComponent;

            _wanderingFSM = new WanderingFSM(speed, stateChangeDuration, moveRange, viewComponent, moveComponent, pathSeeker, myTransform, sightComponent);
            _animator = animator;
            _sightComponent = sightComponent;

            _speed = speed;
            _stateChangeDuration = stateChangeDuration;
        }

        public override void OnNoiseEnter()
        {
            _baseFSM.SetState(Zombie.State.NoiseTracking);
        }

        Vector3 forwardDir;

        public override void OnStateFixedUpdate()
        {
            _viewComponent.RotateRigidbody();
            _moveComponent.MoveRigidbody();
        }

        public override void OnStateUpdate()
        {
            bool isInSight = _sightComponent.IsTargetInSight();
            if(isInSight)
            {
                _baseFSM.SetState(Zombie.State.TargetFollowing);
                return;
            }

            _wanderingFSM.OnUpdate();
        }
    }
}