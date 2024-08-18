using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseState<T>
{
    protected BaseFSM<T> _baseFSM;
    public BaseState(FSM<T> fsm) { _baseFSM = fsm; }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnFixedUpdate();

    public abstract void OnStateExit();
}

public class State<T> : BaseState<T>
{
    public State(FSM<T> baseFSM) : base(baseFSM) { }

    public override void OnStateEnter() { }
    public override void OnStateExit() { }
    public override void OnStateUpdate() { }
    public override void OnFixedUpdate() { }
}