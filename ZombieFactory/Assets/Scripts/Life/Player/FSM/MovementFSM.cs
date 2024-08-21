using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseMovementState : BaseState<ActionComponent.MovementState>
{
    public BaseMovementState(FSM<ActionComponent.MovementState> fsm) : base(fsm)
    {
    }

    public override void OnStateEnter() { }
    public override void OnStateExit() { }
    public override void OnStateUpdate() { }
}

public class MovementFSM : FSM<ActionComponent.MovementState>
{
    public void OnStateFixedUpdate() => _currentState.OnStateFixedUpdate();
    public void OnCollisionEnter(Collision collision) => _currentState.OnCollisionEnter(collision);

    public void OnHandleJump() => _currentState.OnHandleJump();
    public void OnHandleStop() => _currentState.OnHandleStop();
    public void OnHandleMove(Vector3 input) => _currentState.OnHandleMove(input);
}