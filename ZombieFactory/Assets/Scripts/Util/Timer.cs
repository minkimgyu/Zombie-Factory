using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Timer
{
    public enum State
    {
        Ready,
        Running,
        Finish
    }

    State _state;
    float _startTime, _duration;

    public State CurrentState
    {
        get
        {
            if (_state == State.Running && _duration <= Time.time - _startTime) _state = State.Finish;
            return _state;
        }
    }

    public float Ratio
    {
        get
        {
            if (_state == State.Ready) return 0;
            else
            {
                return Mathf.Clamp((Time.time - _startTime) / _duration, 0, 1);
            }
        }
    }

    public Timer()
    {
        _state = State.Ready;
        _startTime = 0;
        _duration = 0;
    }

    public void Start(float duration)
    {
        if (_state != State.Ready) return;
        _state = State.Running;

        _startTime = Time.time;
        _duration = duration;
    }

    // Ÿ�̸Ӹ� ó������ �ʱ�ȭ���ش�.
    public void Reset()
    {
        if (_state == State.Ready) return;
        _state = State.Ready;
    }
}