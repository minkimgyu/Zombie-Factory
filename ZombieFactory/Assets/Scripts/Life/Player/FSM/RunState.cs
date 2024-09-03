using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class RunState : MoveState
{
    Animator _animator;

    public RunState(FSM<ActionController.MovementState> fsm, MoveComponent moveComponent, Animator animator,  float moveForce)
         : base(fsm, moveComponent, moveForce)
    {
        _animator = animator;
    }

    public override void OnStateExit()
    {
        _animator.SetBool("Run", false);
    }

    public override void OnStateEnter()
    {
        _animator.SetBool("Run", true);
    }

    public override void OnHandleRunEnd()
    {
        _baseFSM.SetState(ActionController.MovementState.Walk);
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
