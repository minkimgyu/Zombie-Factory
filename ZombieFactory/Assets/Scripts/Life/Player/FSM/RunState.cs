using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class RunState : MoveState
{
    public RunState(FSM<MovementState> fsm, BaseMoveComponent moveComponent, float moveForce)
         : base(fsm, moveComponent, moveForce)
    {
    }

    public override void OnHandleRunEnd()
    {
        _baseFSM.SetState(MovementState.Walk);
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
