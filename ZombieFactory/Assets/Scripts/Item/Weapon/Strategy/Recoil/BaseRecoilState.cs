using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseRecoilState : WeaponState
{
    public BaseRecoilState() { }

    /// <summary>
    /// Action �̺�Ʈ�� ȣ��Ǵ� Ÿ�ֿ̹� ����
    /// </summary>
    public virtual void Execute() { }
}

abstract public class RecoilGenerator : BaseRecoilState
{
    protected Vector2 _viewRotationMultiplier;
    protected Vector2 _goalMultiplier;

    public Action<Vector2> OnRecoil;

    protected float _shootIntervalDuration;
    protected float _recoveryDuration;
    protected Timer _timer;

    /// <summary>
    /// �ݵ� �̺�Ʈ�� ȣ��� �� ����
    /// </summary>
    protected Action OnExecute;

    protected State _state;

    public enum State
    {
        Idle,
        Generate,
        Recover
    }

    public RecoilGenerator(float shootInterval, float recoveryDuration)
    {
        _shootIntervalDuration = shootInterval + 0.3f; // ���� Interval���� �� �� ũ�� ���ֱ�
        _recoveryDuration = recoveryDuration;
        _timer = new Timer();

        _state = State.Idle;
    }

    public override void Execute() => GenerateRecoil();

    protected void StartMultiplying(Vector2 goalMultiplier, float duration)
    {
        _timer.Reset();
        _timer.Start(duration);
        _goalMultiplier = goalMultiplier;
    }

    protected void GenerateRecoil()
    {
        _state = State.Generate;
        Vector2 point = ReturnNextRecoilPoint();
        StartMultiplying(point, _shootIntervalDuration);
    }

    /// <summary>
    /// Recover�� ������ �� ȣ��
    /// </summary>
    protected virtual void OnStartRecovering() { }

    public override void OnUpdate()
    {
        switch (_state)
        {
            case State.Idle:
                return;
            case State.Generate:
                if (_timer.CurrentState != Timer.State.Finish) break;

                _state = State.Recover;
                OnStartRecovering();
                StartMultiplying(Vector2.zero, _recoveryDuration);

                break;
            case State.Recover:
                if (_timer.CurrentState != Timer.State.Finish) break;

                _state = State.Idle;
                _timer.Reset();
                break;
            default:
                break;
        }

        _viewRotationMultiplier = Vector2.Lerp(_viewRotationMultiplier, _goalMultiplier, _timer.Ratio); // ����Ǵ� ���� �Ѱ��ֱ�
        OnRecoil?.Invoke(_viewRotationMultiplier);
    }

    /// <summary>
    /// ���� �ݵ� ��ġ�� ��ȯ�Ѵ�.
    /// </summary>
    protected abstract Vector2 ReturnNextRecoilPoint();

    public override void LinkEvent(WeaponBlackboard blackboard)
    {
        OnRecoil += blackboard.OnRecoilRequested;
    }

    public override void UnlinkEvent(WeaponBlackboard blackboard)
    {
        OnRecoil -= blackboard.OnRecoilRequested;
    }
}
