using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseZoom : ActionStrategy
{
    protected float _zoomDuration;

    protected float _normalFieldOfView;
    protected float _zoomFieldOfView;

    protected Vector3 _zoomCameraPosition;

    /// <summary>
    /// bool nowTurnOn, float zoomDuration, Vector3 zoomPos, float fieldOfView
    /// </summary>
    protected Action<bool, float, Vector3, float> Zoom;

    /// <summary>
    /// 줌이 되면 Action을 바꿔줌 --> 연사 속도 등 세부 수치 변경해줘야 하기 때문에
    /// </summary>
    protected Action<bool> OnZoomRequested;

    public BaseZoom(Vector3 zoomCameraPosition, float zoomDuration, float normalFieldOfView, float zoomFieldOfView,
        Action<bool> OnZoomRequested)
    {
        _zoomDuration = zoomDuration;
        _zoomCameraPosition = zoomCameraPosition;

        _normalFieldOfView = normalFieldOfView;
        _zoomFieldOfView = zoomFieldOfView;

        this.OnZoomRequested = OnZoomRequested;
    }

    public override void LinkEvent(WeaponBlackboard blackboard)
    {
        Zoom = blackboard.OnZoomRequested;
    }

    public override void UnlinkEvent(WeaponBlackboard blackboard)
    {
        TurnOffZoomDirectly();
        Zoom = null;
    }
}

public class Zoom : BaseZoom
{
    public enum State
    {
        Idle,
        Zoom
    }

    State _state;

    public Zoom(Vector3 zoomCameraPosition, float zoomDuration, float normalFieldOfView, float zoomFieldOfView,
        Action<bool> OnZoomRequested) : base(zoomCameraPosition, zoomDuration, normalFieldOfView, zoomFieldOfView, OnZoomRequested)
    {
        _state = State.Idle;
    }

    public override void Execute()
    {
        if (_state == State.Idle) _state = State.Zoom;
        else _state = State.Idle;

        switch (_state)
        {
            case State.Idle:
                OnZoomRequested?.Invoke(false);
                Zoom?.Invoke(true, _zoomDuration, Vector3.zero, _normalFieldOfView);
                break;

            case State.Zoom:
                OnZoomRequested?.Invoke(true);
                Zoom?.Invoke(false, _zoomDuration, _zoomCameraPosition, _zoomFieldOfView);
                break;
        }
    }

    public override void TurnOffZoomDirectly()
    {
        if (_state == State.Idle) return;

        _state = State.Idle;
        OnZoomRequested?.Invoke(false);
        Zoom?.Invoke(true, 0, Vector3.zero, _normalFieldOfView);
    }
}

public class DoubleZoomStrategy : BaseZoom
{
    public enum State
    {
        Idle,
        Zoom,
        DoubleZoom
    }

    float _doubleZoomFieldOfView;
    State _state;

    public DoubleZoomStrategy(Vector3 zoomCameraPosition, float zoomDuration, float normalFieldOfView, float zoomFieldOfView,
        float doubleZoomFieldOfView, Action<bool> OnZoomRequested)
        : base(zoomCameraPosition, zoomDuration, normalFieldOfView, zoomFieldOfView, OnZoomRequested)
    {
        _state = State.Idle;
        _doubleZoomFieldOfView = doubleZoomFieldOfView;
    }


    public override void Execute()
    {
        if (_state == State.Idle) _state = State.Zoom;
        else if (_state == State.Zoom) _state = State.DoubleZoom;
        else _state = State.Idle;

        switch (_state)
        {
            case State.Idle:
                OnZoomRequested?.Invoke(false);
                Zoom?.Invoke(true, _zoomDuration, Vector3.zero, _normalFieldOfView);
                break;

            case State.Zoom:
                OnZoomRequested?.Invoke(true);
                Zoom?.Invoke(false, _zoomDuration, _zoomCameraPosition, _zoomFieldOfView);
                break;

            case State.DoubleZoom:
                // 여기에는 넣지 않음
                Zoom?.Invoke(false, _zoomDuration, _zoomCameraPosition, _doubleZoomFieldOfView);
                break;
        }
    }

    public override void TurnOffZoomDirectly()
    {
        if (_state == State.Idle) return;

        _state = State.Idle;
        OnZoomRequested?.Invoke(false);
        Zoom?.Invoke(true, 0, Vector3.zero, _normalFieldOfView);
    }
}