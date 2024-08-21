using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseMovementState
{
    Transform _direction;
    Vector3 _input;
    float _moveForce;
    protected Rigidbody _rigidbody;

    public MoveState(FSM<ActionComponent.MovementState> fsm, Transform direction, float moveForce, Rigidbody rigidbody) : base(fsm)
    {
        _direction = direction;
        _moveForce = moveForce;
        _rigidbody = rigidbody;
    }

    public override void OnHandleStop()
    {
        _baseFSM.SetState(ActionComponent.MovementState.Stop);
    }

    public override void OnHandleMove(Vector3 input)
    {
        _input = input;
    }

    protected void Move(Vector3 dir, float force)
    {
        Vector3 direction = dir * force * Time.fixedDeltaTime;
        _rigidbody.velocity = new Vector3(direction.x, _rigidbody.velocity.y, direction.z);
    }

    protected void Jump(Vector3 dir, float force)
    {
        _rigidbody.AddForce(dir * force, ForceMode.Impulse);
    }

    public override void OnStateFixedUpdate()
    {
        Vector3 moveDir = _direction.TransformVector(_input);
        Move(moveDir, _moveForce);
    }
}