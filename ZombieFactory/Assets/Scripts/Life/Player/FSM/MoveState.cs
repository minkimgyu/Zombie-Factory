using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseMovementState
{
    Vector3 _input;
    float _moveSpeed;
    protected BaseMoveComponent _moveComponent;

    Vector3 _storedDirection;

    public MoveState(FSM<MovementState> fsm, BaseMoveComponent moveComponent, float moveSpeed) : base(fsm)
    {
        _moveComponent = moveComponent;
        _moveSpeed = moveSpeed;
    }

    public override void OnHandleMove(Vector3 input)
    {
        _input = input;
    }

    public override void OnStateUpdate()
    {
        _moveComponent.Move(_input, _moveSpeed);
    }

    public override void OnStateFixedUpdate()
    {
        _moveComponent.MoveRigidbody();
    }
}