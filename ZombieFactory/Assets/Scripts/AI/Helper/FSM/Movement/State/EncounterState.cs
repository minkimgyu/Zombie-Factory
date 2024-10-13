using BehaviorTree.Nodes;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = BehaviorTree.Tree;
using Node = BehaviorTree.Node;
using static UnityEngine.GraphicsBuffer;

namespace AI.Swat
{
    public class EncounterState : BaseFreeRoleState
    {
        protected Tree _bt;
        SightComponent _sightComponent;

        NowFarFromTarget _nowFarFromTargetNode; // 상속으로 여러개를 만드는게 더 효율적이다.
        NowCloseToTarget _nowCloseToTargetNode;

        FollowTarget _followTargetNode;
        Retreat _retreatNode;

        public EncounterState(
            FSM<FreeRoleState.State> fsm,

            float moveSpeed,
            float farDistance,
            float closeDistance,
            float gap,

            Transform myTransform,
            SightComponent sightComponent,

            PathSeeker pathSeeker,


            TPSViewComponent viewComponent,
            TPSMoveComponent moveComponent) : base(fsm)
        {
            // 타겟을 향해 바라보기
            // 타겟이 가까워진다면 뒤로 가고 --> 플레이어의 주변 위치로 이동한다.
            // 타겟이 멀어진다면 가까히 간다.

            _nowFarFromTargetNode = new NowFarFromTarget(myTransform, farDistance, gap);
            _nowCloseToTargetNode = new NowCloseToTarget(myTransform, closeDistance, gap);

            _followTargetNode = new FollowTarget(pathSeeker, moveComponent, moveSpeed); // 적
            _retreatNode = new Retreat(pathSeeker, moveComponent, moveSpeed); // 플레이어

            _sightComponent = sightComponent;

            _bt = new Tree();
            List<Node> _childNodes;
            _childNodes = new List<Node>()
            {
                new Sequencer
                (
                    new List<Node>()
                    {
                        new ViewTarget(myTransform, sightComponent, viewComponent),
                        new Selector
                        (
                            new List<Node>()
                            {
                                new Sequencer
                                (
                                    new List<Node>()
                                    {
                                        new Selector
                                        (
                                            new List<Node>()
                                            {
                                                _nowFarFromTargetNode,
                                                _followTargetNode // 적
                                            }
                                        ),

                                        new Selector
                                        (
                                            new List<Node>()
                                            {
                                                _nowCloseToTargetNode,
                                                _retreatNode // 플레이어
                                            }
                                        ),

                                        new Stop(moveComponent)
                                    }
                                ),
                            }
                        )
                    }
                )
            };

            Node rootNode = new Selector(_childNodes);
            _bt.SetUp(rootNode);
        }

        public void ResetRetreatOffset(Vector3 offset)
        {
            _retreatNode.ResetOffset(offset);
        }

        public void ResetRetreatTarget(ITarget target)
        {
            _retreatNode.ResetTarget(target);
        }

        void ResetTarget(ITarget target)
        {
            _nowFarFromTargetNode.ResetTarget(target);
            _nowCloseToTargetNode.ResetTarget(target);
            _followTargetNode.ResetTarget(target);
        }

        public override void OnStateUpdate()
        {
            bool isInSight = _sightComponent.IsTargetInSight();
            if (isInSight == false)
            {
                _baseFSM.SetState(FreeRoleState.State.Idle);
                return;
            }

            ITarget target = _sightComponent.ReturnTargetInSight();
            ResetTarget(target);

            _bt.OnUpdate();
        }
    }
}
