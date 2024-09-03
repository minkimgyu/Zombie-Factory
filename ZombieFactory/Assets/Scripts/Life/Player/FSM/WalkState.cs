using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class WalkState : MoveState
{
    public WalkState(FSM<ActionController.MovementState> fsm, MoveComponent moveComponent, float moveForce)
        : base(fsm, moveComponent, moveForce)
    {
    }

    public override void OnHandleRunStart()
    {
        _baseFSM.SetState(ActionController.MovementState.Run);
    }

    public override void OnHandleMove(Vector3 input)
    {
        base.OnHandleMove(input);

        if (input.magnitude == 0)
        {
            _baseFSM.SetState(ActionController.MovementState.Stop);
            return;
        }
    }

    public override void OnHandleJump()
    {
        _baseFSM.SetState(ActionController.MovementState.Jump);
    }
}
