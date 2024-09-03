using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseMovementState
{
    float _jumpForce;
    Animator _animator;
    MoveComponent _moveComponent;

    public JumpState(FSM<ActionController.MovementState> fsm, MoveComponent moveComponent, Animator animator, float jumpForce) : base(fsm)
    {
        _moveComponent = moveComponent;
        _jumpForce = jumpForce;
        _animator = animator;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        _baseFSM.RevertToPreviousState(); // 이전 상태로 돌려줌
    }

    public override void OnStateExit()
    {
        _animator.SetBool("OnAir", false);
    }

    public override void OnStateEnter()
    {
        _animator.SetBool("OnAir", true);
        _moveComponent.Jump(_jumpForce);
    }
}