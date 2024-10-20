using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM.Movement;

[RequireComponent(typeof(FPSMoveComponent))]
[RequireComponent(typeof(FPSViewComponent))]
public class ActionController : MonoBehaviour
{
    public enum PostureState
    {
        Sit,
        Stand
    }

    public enum MovementState
    {
        Stop,
        Walk,
        Run,
        Jump
    }

    PostureState _postureState;

    MovementFSM _movementFSM;

    FPSMoveComponent _moveComponent;
    FPSViewComponent _viewComponent;
    ZoomComponent _zoomComponent;

    CapsuleCollider _capsuleCollider;

    //[SerializeField] Transform _direction;
    float _walkSpeed;
    float _runSpeed;
    float _onAirSpeed;
    float _jumpSpeed;

    float _postureSwitchDuration;

    float _currentCapsuleCenter;
    float _currentCapsuleHeight;

    float _capsuleStandCenter = 1f;
    float _capsuleCrouchHeight = 1.7f;

    float _capsuleStandHeight = 2f;
    float _capsuleCrouchCenter = 1.15f;

    public virtual void AddObserverEvent
    (
        Action<Vector3, Vector3> MoveCamera,
        Action<float, float> OnFieldOfViewChange,
        Action<bool> SwitchCrosshair
    )
    {
        _zoomComponent.AddObserverEvent(SwitchCrosshair, OnFieldOfViewChange);
        _viewComponent.AddObserverEvent(MoveCamera);
    }

    public void Initialize(float walkSpeed, float runSpeed, float onAirSpeed, float jumpSpeed, float postureSwitchDuration, float capsuleStandCenter,
        float capsuleStandHeight, float capsuleCrouchCenter, float capsuleCrouchHeight, float viewYRange, Vector2 viewSensitivity, Rigidbody rigidbody)
    {
        _walkSpeed = walkSpeed;
        _runSpeed = runSpeed;
        _onAirSpeed = onAirSpeed;
        _jumpSpeed = jumpSpeed;

        _postureSwitchDuration = postureSwitchDuration;
        _capsuleStandCenter = capsuleStandCenter;
        _capsuleCrouchHeight = capsuleCrouchHeight;

        _capsuleCrouchCenter = capsuleCrouchCenter;
        _capsuleStandHeight = capsuleStandHeight;

        _currentCapsuleCenter = _capsuleStandCenter;
        _currentCapsuleHeight = _capsuleStandHeight;

        _capsuleCollider = GetComponent<CapsuleCollider>();

        _moveComponent = GetComponent<FPSMoveComponent>();
        _moveComponent.Initialize(rigidbody);

        _viewComponent = GetComponent<FPSViewComponent>();
        _viewComponent.Initialize(viewYRange, viewSensitivity, rigidbody); // viewYRange, viewSensitivity

        _zoomComponent = GetComponent<ZoomComponent>();
        _zoomComponent.Initialize();

        _postureState = PostureState.Stand;
        _postureTimer = new Timer();

        _movementFSM = new MovementFSM();
        Dictionary<MovementState, BaseState<MovementState>> movementStates = new Dictionary<MovementState, BaseState<MovementState>>
        {
            { MovementState.Stop, new StopState(_movementFSM, _moveComponent) },
            { MovementState.Walk, new WalkState(_movementFSM, _moveComponent, _walkSpeed) },
            { MovementState.Run, new RunState(_movementFSM, _moveComponent, _runSpeed) },
            { MovementState.Jump, new JumpState(_movementFSM, _moveComponent, _jumpSpeed) }
        };

        _movementFSM.Initialize(movementStates);
        _movementFSM.SetState(MovementState.Stop);
    }

    Timer _postureTimer;

    void ChangeCapsuleHeight(float ratio, float capsuleHeight, float capsuleCenter)
    {
        Vector3 center = new Vector3(_capsuleCollider.center.x, capsuleCenter, _capsuleCollider.center.z);
        _capsuleCollider.height = Mathf.Lerp(_capsuleCollider.height, capsuleHeight, ratio);
        _capsuleCollider.center = Vector3.Lerp(_capsuleCollider.center, center, ratio);
    }

    void ChangePosture(PostureState state)
    {
        if (_postureState == state) return;

        _postureState = state;
        switch (_postureState)
        {
            case PostureState.Sit:
                _currentCapsuleCenter = _capsuleCrouchCenter;
                _currentCapsuleHeight = _capsuleCrouchHeight;
                break;
            case PostureState.Stand:
                _currentCapsuleCenter = _capsuleStandCenter;
                _currentCapsuleHeight = _capsuleStandHeight;
                break;
            default:
                break;
        }

        _postureTimer.Reset();
        _postureTimer.Start(_postureSwitchDuration);
    }

    public void OnHandleRunStart()
    {
        _movementFSM.OnHandleRunStart();
    }

    public void OnHandleRunEnd()
    {
        _movementFSM.OnHandleRunEnd();
    }

    public void OnHandleSit()
    {
        ChangePosture(PostureState.Sit);
    }

    public void OnHandleStand()
    {
        ChangePosture(PostureState.Stand);
    }

    public void OnHandleJump()
    {
        _movementFSM.OnHandleJump();
    }

    Vector2 _viewInput;

    public void OnHandleMove(Vector3 dir)
    {
        _movementFSM.OnHandleMove(dir);
    }

    public void OnHandleView(Vector2 dir)
    {
        _viewInput = dir;
    }

    public void OnUpdate()
    {
        _zoomComponent.OnUpdate();
        _movementFSM.OnUpdate();

        if (_postureTimer.CurrentState == Timer.State.Running)
        {
            ChangeCapsuleHeight(_postureTimer.Ratio, _currentCapsuleHeight, _currentCapsuleCenter);
        }

        _moveComponent.CheckIsOnSlope();
        _viewComponent.View(_viewInput);
    }

    public void OnFixedUpdate()
    {
        _movementFSM.OnStateFixedUpdate();
        _viewComponent.RotateRigidbody();
    }

    public void OnLateUpdate()
    {
        _viewComponent.ResetCamera();
    }

    public void OnCollisionEnterRequested(Collision collision)
    {
        _movementFSM.OnCollisionEnter(collision);
    }
}