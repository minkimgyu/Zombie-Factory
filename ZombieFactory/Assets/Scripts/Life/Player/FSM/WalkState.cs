using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MoveState
{
    public WalkState(FSM<ActionComponent.MovementState> fsm, Transform direction, float moveForce, Rigidbody rigidbody)
        : base(fsm, direction, moveForce, rigidbody)
    {
    }

    public override void OnHandleStop()
    {
        _baseFSM.SetState(ActionComponent.MovementState.Stop);
    }

    public override void OnHandleJump()
    {
        _baseFSM.SetState(ActionComponent.MovementState.Jump);
    }
}
