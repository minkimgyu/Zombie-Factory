using BehaviorTree;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tree = BehaviorTree.Tree;
using Node = BehaviorTree.Node;
using BehaviorTree.Nodes;

namespace AI.Swat.Movement
{
    public class BuildFormationState : BaseMovementState
    {
        // bt 넣어주기
        Tree _bt;
        BaseViewComponent _viewComponent;
        BaseMoveComponent _moveComponent;

        public BuildFormationState(
            FSM<Swat.MovementState> fsm,
            float moveSpeed,
            float targetCaptureRadius,
            float gap,

            FormationData formationData,

            BaseViewComponent viewComponent,
            BaseMoveComponent moveComponent,

            Transform myTransform,
            Transform attackPoint,
            SightComponent sightComponent,
            PathSeeker pathSeeker
            ) : base(fsm)
        {
            _viewComponent = viewComponent;
            _moveComponent = moveComponent;

            _bt = new Tree();
            List<Node> _childNodes;
            _childNodes = new List<Node>()
            {
                new Sequencer
                (
                    new List<Node>()
                    {
                        new RetreatToPlayer(pathSeeker, moveComponent, formationData, moveSpeed), // 플레이어

                        new Selector
                        (
                            new List<Node>()
                            {
                                new Sequencer
                                (
                                    new List<Node>()
                                    {
                                        new NowCloseToTargetInSight(sightComponent, myTransform, targetCaptureRadius, gap),
                                        new ViewTarget(attackPoint, sightComponent, viewComponent)
                                    }
                                ),
                                new FaceDirection(myTransform, formationData, viewComponent)
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

        public override void OnStateUpdate()
        {
            _bt.OnUpdate();
        }
    }
}
