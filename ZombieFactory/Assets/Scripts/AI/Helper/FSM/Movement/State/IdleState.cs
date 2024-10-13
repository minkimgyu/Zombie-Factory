using BehaviorTree.Nodes;
using BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Tree = BehaviorTree.Tree;
using Node = BehaviorTree.Node;

namespace AI.Swat
{
    public class IdleState : BaseFreeRoleState
    {
        float _moveRange;
        float _stateChangeDuration;
        float _moveSpeed;

        Transform _myTransform;
        SightComponent _sightComponent;

        WanderingFSM _wanderingFSM;

        TPSViewComponent _viewComponent;
        TPSMoveComponent _moveComponent;
        protected Tree _bt;

        NowFarFromTarget _nowFarFromTargetNode; // 상속으로 여러개를 만드는게 더 효율적이다.
        Retreat _retreatNode;

        public IdleState(
            FSM<FreeRoleState.State> fsm,
            float moveSpeed,
            float stateChangeDuration,
            float moveRange,
            float retreatDistance,
            float gap,

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
            _wanderingFSM = new WanderingFSM(moveSpeed, stateChangeDuration, moveRange, viewComponent, moveComponent, myTransform, sightComponent);

            _myTransform = myTransform;
            _sightComponent = sightComponent;


            _moveSpeed = moveSpeed;
            _stateChangeDuration = stateChangeDuration;
            _moveRange = moveRange;

            _nowFarFromTargetNode = new NowFarFromTarget(myTransform, retreatDistance, gap);
            _retreatNode = new Retreat(pathSeeker, moveComponent, moveSpeed); // 플레이어

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
                                _nowFarFromTargetNode,
                                _retreatNode
                            }
                        ),

                        new Wandering(_wanderingFSM)
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