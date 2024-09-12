using System;
using UnityEngine;

abstract public class EventStrategy : BaseStrategy
{
    /// <summary>
    /// 이벤트 시작 시 호출됨
    /// </summary>
    public Action<BaseWeapon.EventType> OnEventStart;

    /// <summary>
    /// 이벤트 진행 시 호출됨
    /// </summary>
    public Action<BaseWeapon.EventType> OnEventUpdate;

    /// <summary>
    /// 이벤트 종료 시 호출됨
    /// </summary>
    public Action<BaseWeapon.EventType> OnEventEnd;

    /// <summary>
    /// Action 이벤트가 필요한 타이밍에 호출됨
    /// </summary>
    public Action<BaseWeapon.EventType> OnAction;

    public EventStrategy(BaseWeapon.EventType type, Action<BaseWeapon.EventType> OnEventStart, Action<BaseWeapon.EventType> OnEventUpdate,
        Action<BaseWeapon.EventType> OnEventEnd, Action<BaseWeapon.EventType> OnAction) 
    {
        _callType = type;
        this.OnEventStart = OnEventStart;
        this.OnEventUpdate = OnEventUpdate;
        this.OnEventEnd = OnEventEnd;
        this.OnAction = OnAction;
    }

    protected BaseWeapon.EventType _callType;

    public virtual void OnMouseClickStart() => OnEventStart?.Invoke(_callType);

    public virtual void OnMouseClickProcess() => OnEventUpdate?.Invoke(_callType);

    public virtual void OnMouseClickEnd() => OnEventEnd?.Invoke(_callType);
}

public class NoEvent : EventStrategy
{
    public NoEvent() : base(default, null, null, null, null)
    {
    }
}

/// <summary>
/// 연사 액션
/// </summary>
public class AutoEvent : EventStrategy
{
    float _actionDelay;

    /// <summary>
    /// OnMouseClickProcess에서 호출되며 다음 공격 시까지 딜레이 시켜주는 타이머
    /// </summary>
    Timer _actionDelayTimer;

    /// <summary>
    /// OnMouseClickProgress에서 호출되며 광클을 방지해주는 타이머
    /// </summary>
    Timer _clickDelayTimer;


    public AutoEvent(BaseWeapon.EventType type, float actionDelay, Action<BaseWeapon.EventType> OnEventStart, Action<BaseWeapon.EventType> OnEventUpdate,
        Action<BaseWeapon.EventType> OnEventEnd, Action<BaseWeapon.EventType> OnAction) : base(type, OnEventStart, OnEventUpdate, OnEventEnd, OnAction)
    {
        _actionDelay = actionDelay;

        _actionDelayTimer = new Timer();
        _clickDelayTimer = new Timer();
    }

    public override void OnMouseClickStart() 
    {
        base.OnMouseClickStart();
    }

    public override void OnMouseClickEnd()
    {
        base.OnMouseClickEnd();

        if (_clickDelayTimer.CurrentState == Timer.State.Finish) _clickDelayTimer.Reset();
        _clickDelayTimer.Start(_actionDelay);
    }

    public override void OnMouseClickProcess()
    {
        base.OnMouseClickProcess();

        if (_clickDelayTimer.CurrentState == Timer.State.Running) return;
        if (_actionDelayTimer.CurrentState == Timer.State.Running) return;

        OnAction?.Invoke(_callType);

        if(_actionDelayTimer.CurrentState == Timer.State.Finish) _actionDelayTimer.Reset();
        _actionDelayTimer.Start(_actionDelay);
    }
}

/// <summary>
/// 단발 액션
/// </summary>
public class ManualEvent : EventStrategy
{
    float _actionDelay;

    /// <summary>
    /// OnMouseClickStart에서 호출되며 다음 공격 시까지 딜레이 시켜주는 타이머
    /// </summary>
    Timer _actionDelayTimer;

    public ManualEvent(BaseWeapon.EventType type, float actionDelay, Action<BaseWeapon.EventType> OnEventStart, Action<BaseWeapon.EventType> OnEventUpdate,
        Action<BaseWeapon.EventType> OnEventEnd, Action<BaseWeapon.EventType> OnAction) : base(type, OnEventStart, OnEventUpdate, OnEventEnd, OnAction)
    {
        _actionDelayTimer = new Timer();
        _actionDelay = actionDelay;
    }

    public override void OnMouseClickStart()
    {
        base.OnMouseClickStart();

        if (_actionDelayTimer.CurrentState == Timer.State.Running) return;

        OnAction?.Invoke(_callType);
        if (_actionDelayTimer.CurrentState == Timer.State.Finish) _actionDelayTimer.Reset();
        _actionDelayTimer.Start(_actionDelay);
    }
}

/// <summary>
/// 점사 액션
/// </summary>
public class BurstEvent: EventStrategy
{
    float _actionDelay;

    /// <summary>
    /// 점사 횟수
    /// </summary>
    int _fireCountInOneAction;

    Timer _tickDelayTimer;

    /// <summary>
    /// OnMouseClickStart에서 호출되며 다음 공격 시까지 딜레이 시켜주는 타이머
    /// </summary>
    Timer _actionDelayTimer;

    public BurstEvent(BaseWeapon.EventType callType, float attackDelay, int fireCountInOneAction, Action<BaseWeapon.EventType> OnEventStart, Action<BaseWeapon.EventType> OnEventUpdate,
        Action<BaseWeapon.EventType> OnEventEnd, Action<BaseWeapon.EventType> OnAction) : base(callType, OnEventStart, OnEventUpdate, OnEventEnd, OnAction)
    {
        _actionDelayTimer = new Timer();
        _tickDelayTimer = new Timer();

        _actionDelay = attackDelay;
        _fireCountInOneAction = fireCountInOneAction;
    }

    public override void OnMouseClickStart()
    {
        base.OnMouseClickStart();

        //if (_actionDelayTimer.CurrentState == Timer.State.Running) return;

        _tickDelayTimer.Reset();
        _actionDelayTimer.Reset();
        _actionDelayTimer.Start(_actionDelay * _fireCountInOneAction);
    }

    public override void OnUpdate()
    {
        if (_actionDelayTimer.CurrentState != Timer.State.Running) return;
        if (_tickDelayTimer.CurrentState == Timer.State.Running) return;

        OnAction?.Invoke(_callType);

        _tickDelayTimer.Reset();
        _tickDelayTimer.Start(_actionDelay);
    }
}