using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Swat;

namespace FSM.Movement
{
    abstract public class BaseMovementState : BaseState<ActionController.MovementState>
    {
        public BaseMovementState(FSM<ActionController.MovementState> fsm) : base(fsm)
        {
        }

        public override void OnStateEnter() { }
        public override void OnStateExit() { }
        public override void OnStateUpdate() { }
    }

    public class MovementFSM : FSM<ActionController.MovementState>
    {
        public void OnStateFixedUpdate() => _currentState.OnStateFixedUpdate();


        public void OnCollisionEnter(Collision collision) => _currentState.OnCollisionEnter(collision);

        public void OnHandleRunStart() => _currentState.OnHandleRunStart();
        public void OnHandleRunEnd() => _currentState.OnHandleRunEnd();

        public void OnHandleJump() => _currentState.OnHandleJump();
        public void OnHandleMove(Vector3 input) => _currentState.OnHandleMove(input);
    }
}