using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FSM;
using UnityEditor;

namespace AI.Swat.Battle
{
    public class AttackState : BaseBattleState
    {
        SightComponent _sightComponent;
        WeaponController _weaponController;

        Timer _attackTimer;
        Timer _attackDelayTimer;

        enum State
        {
            Idle,
            Delay,
            Action
        }

        State _state;

        float _attackDuration;
        float _attackDelay;

        public AttackState(
            FSM<Swat.BattleState> fsm,
            WeaponController weaponController,
            SightComponent sightComponent,
            float attackDuration,
            float attackDelay) : base(fsm)
        {
            _sightComponent = sightComponent;
            _weaponController = weaponController;
            _attackDuration = attackDuration;
            _attackDelay = attackDelay;

            _state = State.Idle;

            _attackTimer = new Timer();
            _attackDelayTimer = new Timer();
        }

        public override void OnStateUpdate()
        {
            bool isAmmoEmpty = _weaponController.IsAmmoEmpty();
            bool isInSight = _sightComponent.IsTargetInSight();
            if (isAmmoEmpty == true || isInSight == false)
            {
                _baseFSM.SetState(Swat.BattleState.Idle);
                return;
            }

            switch (_state)
            {
                case State.Idle:
                    _attackTimer.Start(_attackDuration);
                    _state = State.Action;

                    _weaponController.OnHandleEventStart(BaseWeapon.EventType.Main);
                    break;

                case State.Action:

                    if (_attackTimer.CurrentState != Timer.State.Finish) break;

                    _attackTimer.Reset();
                    _attackDelayTimer.Start(_attackDelay);
                    _state = State.Delay;

                    _weaponController.OnHandleEventEnd(BaseWeapon.EventType.Main);
                    break;

                case State.Delay:

                    if (_attackDelayTimer.CurrentState != Timer.State.Finish) break;

                    _attackDelayTimer.Reset();
                    _state = State.Idle;
                    break;
                default:
                    break;
            }

        }

        public override void OnStateExit()
        {
            if (_state == State.Action)
            {
                _weaponController.OnHandleEventEnd(BaseWeapon.EventType.Main);
            }

            _state = State.Idle;
            _attackTimer.Reset();
            _attackDelayTimer.Reset();
        }
    }
}