using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM.Movement;

public class StopState : BaseMovementState
{
    BaseMoveComponent _moveComponent;

    public StopState(FSM<ActionController.MovementState> fsm, BaseMoveComponent moveComponent) : base(fsm)
    {
        _moveComponent = moveComponent;
    }

    public override void OnStateUpdate()
    {
        _moveComponent.Stop();
    }

    public override void OnStateFixedUpdate()
    {
        _moveComponent.MoveRigidbody();
    }

    public override void OnHandleMove(Vector3 input)
    {
        if (input.magnitude > 0)
        {
            _baseFSM.SetState(ActionController.MovementState.Walk);
            return;
        }
    }

    public override void OnHandleJump()
    {
        _baseFSM.SetState(ActionController.MovementState.Jump);
    }
}