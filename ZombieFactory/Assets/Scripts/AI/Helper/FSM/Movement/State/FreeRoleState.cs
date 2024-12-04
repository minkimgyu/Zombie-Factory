using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Swat.Movement
{
    public class FreeRoleState : BaseMovementState
    {
        public enum State
        {
            Idle,
            Encounter
        }

        FreeRoleFSM _freeRoleFSM;

        public FreeRoleState(
            FSM<Swat.MovementState> fsm,
            float moveSpeed,
            float stateChangeDuration,
            float moveRange,
            float retreatDistance,
            float gap,

            float farDistance,
            float closeDistance,

            FormationData formationData,

            BaseViewComponent viewComponent,
            BaseMoveComponent moveComponent,

            Transform myTransform,
            Transform attackPoint,

            SightComponent sightComponent,
            PathSeeker pathSeeker) : base(fsm)
        {
            _freeRoleFSM = new FreeRoleFSM();
            Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>
            {
                {
                    State.Idle,
                    new IdleState(
                        _freeRoleFSM,
                        moveSpeed,
                        stateChangeDuration,
                        moveRange,
                        retreatDistance,
                        gap,

                        formationData,

                        viewComponent,
                        moveComponent,
                        myTransform,
                        sightComponent,
                        pathSeeker)
                },
                { 
                    State.Encounter,
                    new EncounterState(
                        _freeRoleFSM,
                        moveSpeed,
                        farDistance,
                        closeDistance,
                        retreatDistance,
                        gap,

                        formationData,

                        myTransform,
                        attackPoint,
                        sightComponent,
                        pathSeeker,
                        viewComponent,
                        moveComponent
                    )
                },
            };
            _freeRoleFSM.Initialize(states);
            _freeRoleFSM.SetState(State.Idle);

        }

        public override void OnStateUpdate()
        {
            _freeRoleFSM.OnUpdate();
        }

        public override void OnStateFixedUpdate()
        {
            _freeRoleFSM.OnFixedUpdate();
        }
    }
}