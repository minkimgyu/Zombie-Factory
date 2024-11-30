using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM.Movement;

public class MoveState : BaseMovementState
{
    Vector3 _input;
    float _moveSpeed;
    protected BaseMoveComponent _moveComponent;

    Vector3 _storedDirection;

    bool _onAir;

    public MoveState(
        FSM<ActionController.MovementState> fsm,
        BaseMoveComponent moveComponent,
        float moveSpeed,
        bool onAir) : base(fsm)
    {
        _moveComponent = moveComponent;
        _moveSpeed = moveSpeed;

        _onAir = onAir;
    }

    public override void OnHandleMove(Vector3 input)
    {
        _input = input;
    }

    public override void OnStateUpdate()
    {
        _moveComponent.Move(_input, _moveSpeed, _onAir);
    }

    public override void OnStateFixedUpdate()
    {
        _moveComponent.MoveRigidbody();
    }
}