using AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Pathfinding;

public class WanderingFSM
{
    public enum State
    {
        Stop,
        Rotate,
        Move
    }

    State _state;

    float _stateChangeDuration;
    float _moveSpeed;
    float _moveRange;

    Timer _stateTimer;
    BaseViewComponent _viewComponent;
    BaseMoveComponent _moveComponent;

    Transform _myTransform;
    SightComponent _sightComponent;

    PathSeeker _pathSeeker;

    Vector3 _movePoint;
    Vector3 _viewDirection;

    public WanderingFSM(
        float speed,
        float stateChangeDuration,
        float moveRange,

        BaseViewComponent viewComponent,
        BaseMoveComponent moveComponent,
        PathSeeker pathSeeker,

        Transform myTransform,
        SightComponent sightComponent)
    {
        _viewComponent = viewComponent;
        _moveComponent = moveComponent;

        _pathSeeker = pathSeeker;

        _myTransform = myTransform;
        _sightComponent = sightComponent;

        _state = State.Stop;
        _stateTimer = new Timer();

        _moveSpeed = speed;
        _moveRange = moveRange;
        _stateChangeDuration = stateChangeDuration;
    }

    public void GoToStopState()
    {
        if( _state != State.Stop )
        {
            _state = State.Stop;
            _stateTimer.Reset();
            _stateTimer.Start(_stateChangeDuration);
        }
    }

    public void OnUpdate()
    {
        switch (_state)
        {
            case State.Stop:
                _moveComponent.Stop();
                break;
            case State.Rotate:
                _viewComponent.View(_viewDirection);
                _moveComponent.Stop();
                break;
            case State.Move:

                Vector3 dir = _pathSeeker.ReturnDirection(_movePoint);

                if (_pathSeeker.NowFinish() == true)
                {
                    _moveComponent.Stop();
                }
                else
                {
                    _viewComponent.View(dir);
                    _moveComponent.Move(dir, _moveSpeed);
                }
                break;
        }

        if(_stateTimer.CurrentState == Timer.State.Ready)
        {
            _state = State.Stop;
            _stateTimer.Start(_stateChangeDuration);
        }
        else if (_stateTimer.CurrentState == Timer.State.Finish)
        {
            int size = Enum.GetValues(typeof(State)).Length;
            _state = (State)Random.Range(0, size);

            Vector3 dir;
            switch (_state)
            {
                case State.Rotate:
                    dir = Random.insideUnitCircle;
                    _viewDirection = new Vector3(dir.x, 0, dir.y).normalized;
                    break;
                case State.Move:
                    dir = Random.insideUnitCircle;
                    _movePoint = _myTransform.position + new Vector3(dir.x, 0, dir.y) * _moveRange;
                    break;
                case State.Stop:
                    break;
            }

            _stateTimer.Reset();
            _stateTimer.Start(_stateChangeDuration);
            return;
        }
    }
}