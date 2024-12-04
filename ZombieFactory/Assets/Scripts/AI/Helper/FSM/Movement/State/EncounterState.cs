using BehaviorTree.Nodes;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = BehaviorTree.Tree;
using Node = BehaviorTree.Node;

namespace AI.Swat.Movement
{
    public class EncounterState : BaseFreeRoleState
    {
        protected Tree _bt;
        SightComponent _sightComponent;

        BaseViewComponent _viewComponent;
        BaseMoveComponent _moveComponent;

        public EncounterState(
            FSM<FreeRoleState.State> fsm,

            float moveSpeed,
            float farDistance,
            float closeDistance,
            float retreatDistance,
            float gap,

            FormationData formationData,

            Transform myTransform,
            Transform attackPoint,
            SightComponent sightComponent,

            PathSeeker pathSeeker,


            BaseViewComponent viewComponent,
            BaseMoveComponent moveComponent) : base(fsm)
        {
            // Ÿ���� ���� �ٶ󺸱�
            // Ÿ���� ��������ٸ� �ڷ� ���� --> �÷��̾��� �ֺ� ��ġ�� �̵��Ѵ�.
            // Ÿ���� �־����ٸ� ������ ����.

            _viewComponent = viewComponent;
            _moveComponent = moveComponent;
            _sightComponent = sightComponent;

            _bt = new Tree();
            List<Node> _childNodes;
            _childNodes = new List<Node>()
            {
                new Sequencer
                (
                    new List<Node>()
                    {
                        new ViewTarget(attackPoint, sightComponent, viewComponent),
                        new Selector
                        (
                            new List<Node>()
                            {
                                new Selector
                                (
                                    new List<Node>()
                                    {
                                        // �÷��̾�κ��� �־����� ���� ����
                                        new Sequencer
                                        (
                                            new List<Node>()
                                            {
                                                new NowFarFromPlayer(myTransform, formationData, retreatDistance, gap),
                                                new RetreatToPlayer(pathSeeker, moveComponent, formationData, moveSpeed)
                                            }
                                        ),

                                        // �����κ��� �־����� ���� ����
                                        new Sequencer
                                        (
                                            new List<Node>()
                                            {
                                               new NowFarFromTargetInSight(sightComponent, myTransform, farDistance, gap),
                                               new FollowTargetInSight(sightComponent, pathSeeker, moveComponent, moveSpeed)
                                            }
                                        ),

                                        new Sequencer
                                        (
                                            new List<Node>()
                                            {
                                                new NowCloseToTargetInSight(sightComponent, myTransform, closeDistance, gap),
                                                new RetreatToPlayer(pathSeeker, moveComponent, formationData, moveSpeed)
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

        public override void OnStateUpdate()
        {
            bool isInSight = _sightComponent.IsTargetInSight();
            if (isInSight == false)
            {
                _baseFSM.SetState(FreeRoleState.State.Idle);
                return;
            }

            _bt.OnUpdate();
        }

        public override void OnStateFixedUpdate()
        {
            _viewComponent.RotateRigidbody();
            _moveComponent.MoveRigidbody();
        }
    }
}
