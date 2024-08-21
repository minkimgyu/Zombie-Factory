using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopState : BaseMovementState
{
    public StopState(FSM<ActionComponent.MovementState> fsm) : base(fsm)
    {
    }

    public override void OnHandleMove(Vector3 input)
    {
        _baseFSM.SetState(ActionComponent.MovementState.Walk);
    }

    public override void OnHandleJump()
    {
        _baseFSM.SetState(ActionComponent.MovementState.Jump);
    }
}