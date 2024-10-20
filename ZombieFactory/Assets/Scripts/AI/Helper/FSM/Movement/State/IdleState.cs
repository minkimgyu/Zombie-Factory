using BehaviorTree.Nodes;
using BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = BehaviorTree.Tree;
using Node = BehaviorTree.Node;

namespace AI.Swat.Movement
{
    public class IdleState : BaseFreeRoleState
    {
        Transform _myTransform;
        SightComponent _sightComponent;

        TPSViewComponent _viewComponent;
        TPSMoveComponent _moveComponent;
        Tree _bt;

        NowFarFromPlayer _nowFarFromTargetNode; // 상속으로 여러개를 만드는게 더 효율적이다.
        StopWandering _stopWandering;
        RetreatToPlayer _retreatNode;

        FormationData _formationData; // 상위 클레스에서 상황에 따라 수정하여 사용

        public IdleState(
            FSM<FreeRoleState.State> fsm,
            float moveSpeed,
            float stateChangeDuration,
            float moveRange,
            float retreatDistance,
            float gap,

            FormationData formationData,

            TPSViewComponent viewComponent,
            TPSMoveComponent moveComponent,

            Transform myTransform,
            SightComponent sightComponent,
            PathSeeker pathSeeker) : base(fsm)
        {
            // 주변 좌표를 받아서 해당 위치로 이동
            // 주변 좌표를 받아서 해당 방향을 보기
            // 정지
            _viewComponent = viewComponent;
            _moveComponent = moveComponent;
            WanderingFSM wanderingFSM = new WanderingFSM(moveSpeed, stateChangeDuration, moveRange, viewComponent, moveComponent, pathSeeker, myTransform, sightComponent);

            _myTransform = myTransform;
            _sightComponent = sightComponent;

            _bt = new Tree();
            List<Node> _childNodes;
            _childNodes = new List<Node>()
            {
                new Selector
                (
                    new List<Node>()
                    {
                        new Sequencer
                        (
                            new List<Node>()
                            {
                                new NowFarFromPlayer(myTransform, formationData, retreatDistance, gap),

                                new StopWandering(wanderingFSM),
                                new RetreatToPlayer(pathSeeker, moveComponent, formationData, moveSpeed)
                            }
                        ),

                        new Wandering(wanderingFSM)
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

        Vector3 _randomdDir;
        Vector3 _randomPos;

        public override void OnStateUpdate()
        {
            bool isInSight = _sightComponent.IsTargetInSight();
            if (isInSight)
            {
                _baseFSM.SetState(FreeRoleState.State.Encounter);
                return;
            }

            _bt.OnUpdate();
        }
    }
}