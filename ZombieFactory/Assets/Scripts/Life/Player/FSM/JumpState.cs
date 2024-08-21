using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : MoveState
{
    float _jumpForce;

    public JumpState(FSM<ActionComponent.MovementState> fsm, Transform direction, float moveForce, float jumpForce, Rigidbody rigidbody)
        : base(fsm, direction, moveForce, rigidbody)
    {
        _jumpForce = jumpForce;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        _baseFSM.RevertToPreviousState(); // 이전 상태로 돌려줌
    }

    public override void OnStateEnter()
    {
        Jump(Vector3.up, _jumpForce);
    }
}