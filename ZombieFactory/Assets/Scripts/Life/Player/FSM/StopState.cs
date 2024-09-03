using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class StopState : BaseMovementState
{
    MoveComponent _moveComponent;

    public StopState(FSM<ActionController.MovementState> fsm, MoveComponent moveComponent) : base(fsm)
    {
        _moveComponent = moveComponent;
    }

    public override void OnStateFixedUpdate()
    {
        _moveComponent.Stop();
    }

    public override void OnHandleMove(Vector3 input)
    {
        //Debug.Log("StopState: " + input.magnitude);

        if (input.magnitude > 0)
        {
            _baseFSM.SetState(ActionController.MovementState.Walk);
            return;
        }
    }

    public override void OnHandleJump()
    {
        _baseFSM.SetState(ActionController.MovementState.Jump);
    }
}