using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseMovementState
{
    float _jumpForce;
    BaseMoveComponent _moveComponent;

    public JumpState(FSM<MovementState> fsm, BaseMoveComponent moveComponent, float jumpForce) : base(fsm)
    {
        _moveComponent = moveComponent;
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