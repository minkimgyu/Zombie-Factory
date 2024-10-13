using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Swat
{
    public class FreeRoleState : BaseMovementState
    {
        public enum State
        {
            Idle,
            Encounter
        }

        FreeRoleFSM _freeRoleFSM;

        IdleState _idleState;
        EncounterState _encounterState;

        public FreeRoleState(
            FSM<Swat.State> fsm,
            float moveSpeed,
            float stateChangeDuration,
            float moveRange,
            float retreatDistance,
            float gap,

            float farDistance,
            float closeDistance,

            TPSViewComponent viewComponent,
            TPSMoveComponent moveComponent,
            Animator animator,

            Transform myTransform,
            SightComponent sightComponent,
            PathSeeker pathSeeker) : base(fsm)
        {
            _freeRoleFSM = new FreeRoleFSM();

            _idleState = new IdleState(
                _freeRoleFSM,
                moveSpeed,
                stateChangeDuration,
                moveRange,
                retreatDistance,
                gap,

                viewComponent,
                moveComponent,
                myTransform,
                sightComponent,
                pathSeeker);

            _encounterState = new EncounterState(
                _freeRoleFSM,
                moveSpeed,
                farDistance,
                closeDistance,
                gap,
                myTransform,
                sightComponent,
                pathSeeker,
                viewComponent,
                moveComponent
            );

            Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>
            {
                {
                    State.Idle,
                    _idleState
                },
                { 
                    State.Encounter,
                    _encounterState
                },
            };
            _freeRoleFSM.Initialize(states);
            _freeRoleFSM.SetState(State.Idle);

        }

        public void ResetRetreatOffset(Vector3 offset)
        {
            _idleState.ResetRetreatOffset(offset);
            _encounterState.ResetRetreatOffset(offset);
        }

        public void ResetRetreatTarget(ITarget target)
        {
            _idleState.ResetRetreatTarget(target);
            _encounterState.ResetRetreatTarget(target);
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