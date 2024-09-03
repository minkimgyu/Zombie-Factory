using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseMovementState
{
    Vector3 _input;
    float _moveSpeed;
    protected MoveComponent _moveComponent;

    Vector3 _storedDirection;

    public MoveState(FSM<ActionController.MovementState> fsm, MoveComponent moveComponent, float moveSpeed) : base(fsm)
    {
        _moveComponent = moveComponent;
        _moveSpeed = moveSpeed;
    }

    public override void OnHandleMove(Vector3 input)
    {
        _input = input;
    }

    public override void OnStateFixedUpdate()
    {
        _moveComponent.Move(_input, _moveSpeed);
    }
}