using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace AI
{
    public class TargetFollowingState : BaseZombieState
    {
        PathSeeker _pathSeeker;
        SightComponent _sightComponent;

        Transform _myTransform;

        float _moveSpeed;
        float _stopDistance;
        float _gap;

        TPSMoveComponent _moveComponent;
        TPSViewComponent _viewComponent;

        MeleeAttackComponent _meleeAttackComponent;
        MoveState _state;

        public enum MoveState
        {
            Stop,
            Move,
        }

        public TargetFollowingState(
            ZombieFSM fsm,

            float moveSpeed,
            float stopDistance,
            float gap,

            float attackRadius,
            float attackDamage,

            float attackPreDelay,
            float attackAfterDelay,

            Transform myTransform,
            Transform raycastPoint,
            TPSViewComponent viewComponent,
            TPSMoveComponent moveComponent,

            PathSeeker pathSeeker,
            SightComponent sightComponent,
            Animator animator) : base(fsm)
        {
            _moveSpeed = moveSpeed;
            _stopDistance = stopDistance;
            _gap = gap;

            _meleeAttackComponent = new MeleeAttackComponent(raycastPoint,
                attackDamage,
                attackPreDelay,
                attackAfterDelay,
                attackRadius,
                animator);

            _moveComponent = moveComponent;
            _viewComponent = viewComponent;

            _myTransform = myTransform;

            _pathSeeker = pathSeeker;
            _sightComponent = sightComponent;
            _state = MoveState.Stop;
        }

        public override void OnStateFixedUpdate()
        {
            _viewComponent.RotateRigidbody();
        }

        // bt 넣어주기
        public override void OnStateUpdate()
        {
            bool isInSight = _sightComponent.IsTargetInSight();
            if (isInSight == false)
            {
                _baseFSM.SetState(Zombie.State.Idle);
                return;
            }

            ITarget target = _sightComponent.ReturnTargetInSight();
            Vector3 targetPos = target.ReturnPosition();

            Vector3 viewDirection = (targetPos - _myTransform.position).normalized;
            _viewComponent.View(viewDirection);

            float diatance = Vector3.Distance(_myTransform.position, targetPos);
            Vector3 dir = _pathSeeker.ReturnDirection(targetPos);

            switch (_state)
            {
                case MoveState.Stop:
                    if (diatance <= _stopDistance + _gap)
                    {
                        _moveComponent.Stop();
                        _meleeAttackComponent.Attack(viewDirection); // 공격 기능 추가
                        break;
                    }

                    //Debug.Log(_state);
                    _state = MoveState.Move;
                    break;
                case MoveState.Move:
                    if (diatance < _stopDistance)
                    {
                        //Debug.Log(_state);
                        _state = MoveState.Stop;
                        break;
                    }

                    if (_pathSeeker.IsFinish() == true) return;
                    _moveComponent.Move(dir, _moveSpeed);
                    break;
                default:
                    break;
            }
        }
    }
}
