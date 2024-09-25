using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class WalkState : MoveState
{
    public WalkState(FSM<MovementState> fsm, BaseMoveComponent moveComponent, float moveForce)
        : base(fsm, moveComponent, moveForce)
    {
    }

    public override void OnHandleRunStart()
    {
        _baseFSM.SetState(MovementState.Run);
    }

    public override void OnHandleMove(Vector3 input)
    {
        base.OnHandleMove(input);

        if (input.magnitude == 0)
        {
            _baseFSM.SetState(MovementState.Stop);
            return;
        }
    }

    public override void OnHandleJump()
    {
        _baseFSM.SetState(MovementState.Jump);
    }
}
