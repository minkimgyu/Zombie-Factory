using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class ActionComponent : MonoBehaviour
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
        Jump
    }

    PostureState _postureState;

    MovementFSM _movementFSM;

    ViewComponent _viewComponent;
    //ZoomComponent _zoomComponent;

    CapsuleCollider _capsuleCollider;
    Rigidbody _rigidbody;

    [SerializeField] Transform _direction;
    float _walkSpeed;
    float _walkSpeedOnAir;
    float _jumpSpeed;

    float _postureSwitchDuration;

    float _currentCapsuleCenter;
    float _currentCapsuleHeight;

    float _capsuleStandCenter = 1f;
    float _capsuleCrouchHeight = 1.7f;

    float _capsuleStandHeight = 2f;
    float _capsuleCrouchCenter = 1.15f;

    public void Initialize(float walkSpeed, float walkSpeedOnAir, float jumpSpeed, float postureSwitchDuration, float capsuleStandCenter,
        float capsuleStandHeight, float capsuleCrouchCenter, float capsuleCrouchHeight, float viewYRange, Vector2 viewSensitivity)
    {
        _walkSpeed = walkSpeed;
        _walkSpeedOnAir = walkSpeedOnAir;
        _jumpSpeed = jumpSpeed;

        _postureSwitchDuration = postureSwitchDuration;
        _capsuleStandCenter = capsuleStandCenter;
        _capsuleCrouchHeight = capsuleCrouchHeight;

        _capsuleCrouchCenter = capsuleCrouchCenter;
        _capsuleStandHeight = capsuleStandHeight;

        _currentCapsuleCenter = _capsuleStandCenter;
        _currentCapsuleHeight = _capsuleStandHeight;

        //_inputState = InputState.Enable;

        _capsuleCollider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();

        _viewComponent = GetComponent<ViewComponent>();
        _viewComponent.Initialize(viewYRange, viewSensitivity);

        //_zoomComponent = GetComponent<ZoomComponent>();
        //_zoomComponent.Initialize();

        _postureState = PostureState.Stand;
        _postureTimer = new Timer();

        _movementFSM = new MovementFSM();
        Dictionary<MovementState, BaseState<MovementState>> movementStates = new Dictionary<MovementState, BaseState<MovementState>>
        {
            { MovementState.Stop, new StopState(_movementFSM) },
            { MovementState.Walk, new WalkState(_movementFSM, _direction, _walkSpeed, _rigidbody) },
            { MovementState.Jump, new JumpState(_movementFSM, _direction, _walkSpeedOnAir, _jumpSpeed, _rigidbody) }
        };

        _movementFSM.Initialize(movementStates, MovementState.Stop);
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

    public void OnHandleStop()
    {
        _movementFSM.OnHandleStop();
    }

    public void OnHandleMove(Vector3 dir)
    {
        _movementFSM.OnHandleMove(dir);
    }

    public void OnUpdate()
    {
        //_zoomComponent.OnUpdate();
        _movementFSM.OnUpdate();

        if (_postureTimer.CurrentState == Timer.State.Running)
        {
            ChangeCapsuleHeight(_postureTimer.Ratio, _currentCapsuleHeight, _currentCapsuleCenter);
        }

        //if (_inputState == InputState.Disable) return;
        _viewComponent.ResetView();
    }

    public void OnFixedUpdate()
    {
        _movementFSM.OnStateFixedUpdate();
    }

    public void OnLateUpdate()
    {
        _viewComponent.ApplyRecoilToCamera();
    }

    public void OnCollisionEnterRequested(Collision collision)
    {
        _movementFSM.OnCollisionEnter(collision);
    }
}
