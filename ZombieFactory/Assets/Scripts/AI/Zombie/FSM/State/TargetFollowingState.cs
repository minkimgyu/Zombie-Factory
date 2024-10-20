using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using BehaviorTree.Nodes;
using Tree = BehaviorTree.Tree;
using Node = BehaviorTree.Node;

namespace AI.Zombie
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

        Tree _bt;
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

            _moveComponent = moveComponent;
            _viewComponent = viewComponent;

            _myTransform = myTransform;

            _pathSeeker = pathSeeker;
            _sightComponent = sightComponent;

            _bt = new Tree();
            List<Node> _childNodes;
            _childNodes = new List<Node>()
            {
                new Sequencer
                (
                    new List<Node>()
                    {
                        new ViewTarget(_myTransform, _sightComponent, _viewComponent), // 정지 하는 코드 넣기

                        new Selector
                        (
                            new List<Node>()
                            {
                                new Sequencer
                                (
                                    new List<Node>()
                                    {
                                        new NowCloseToTargetInSight(sightComponent, myTransform, _stopDistance, gap),
                                        new Sequencer
                                        (
                                            new List<Node>()
                                            {
                                                new Stop(_moveComponent), // 정지 하는 코드 넣기

                                                new Sequencer
                                                (
                                                    new List<Node>()
                                                    {
                                                        new WaitForNextAttack(attackPreDelay, attackAfterDelay, () =>{ animator.Play("Attack"); }),

                                                        new Attack(
                                                            _myTransform,
                                                            raycastPoint,
                                                            attackDamage,
                                                            attackRadius,
                                                            _sightComponent
                                                        ),
                                                    }
                                                ),
                                            }
                                        ),
                                    }
                                ),
                                new FollowTargetInSight(sightComponent, _pathSeeker, _moveComponent, _moveSpeed)
                            }
                        )
                    }
                )
            };
            Node rootNode = new Selector(_childNodes);
            _bt.SetUp(rootNode);
        }

        public override void OnStateFixedUpdate()
        {
            _viewComponent.RotateRigidbody();
            _moveComponent.MoveRigidbody();
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

            _bt.OnUpdate();
        }
    }
}
