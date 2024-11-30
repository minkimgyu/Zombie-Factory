using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM.Movement;

public class JumpState : MoveState
{
    float _jumpForce;

    public JumpState(
        FSM<ActionController.MovementState> fsm,
        BaseMoveComponent moveComponent,
        float jumpForce,

        float moveSpeed) : base(fsm, moveComponent, moveSpeed, true)
    {
        _jumpForce = jumpForce;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        _baseFSM.RevertToPreviousState(); // ���� ���·� ������
    }

    public override void OnStateEnter()
    {
        _moveComponent.Jump(_jumpForce);
    }
}