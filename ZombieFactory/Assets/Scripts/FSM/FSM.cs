using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseFSM<T>
{
    Dictionary<T, BaseState<T>> _states;

    protected BaseState<T> _currentState;
    protected BaseState<T> _previousState;

    public void Initialize(Dictionary<T, BaseState<T>> states)
    {
        _currentState = null;
        _previousState = null;

        _states = states;
    }

    public bool SetState(T stateName)
    {
        return ChangeState(_states[stateName]);
    }

    public bool SetState(T stateName, BaseWeapon newWeapon, string message)
    {
        return ChangeState(_states[stateName], newWeapon, message);
    }

    public bool SetState(T stateName, BaseWeapon.Type weaponType, string message)
    {
        return ChangeState(_states[stateName], weaponType, message);
    }

    public bool RevertToPreviousState()
    {
        return ChangeState(_previousState);
    }

    bool ChangeState(BaseState<T> state)
    {
        if (_states.ContainsValue(state) == false) return false;

        if (_currentState == state)
        {
            return false;
        }

        if (_currentState != null)
            _currentState.OnStateExit();

        _previousState = _currentState;

        _currentState = state;


        if (_currentState != null)
        {
            _currentState.OnStateEnter();
        }

        return true;
    }

    bool ChangeState(BaseState<T> state, BaseWeapon newWeapon, string message)
    {
        if (_states.ContainsValue(state) == false) return false;

        if (_currentState == state)
        {
            return false;
        }

        if (_currentState != null)
            _currentState.OnStateExit();

        _previousState = _currentState;

        _currentState = state;


        if (_currentState != null)
        {
            _currentState.OnStateEnter(newWeapon, message);
        }

        return true;
    }

    bool ChangeState(BaseState<T> state, BaseWeapon.Type weaponType, string message)
    {
        if (_states.ContainsValue(state) == false) return false;

        if (_currentState == state)
        {
            return false;
        }

        if (_currentState != null)
            _currentState.OnStateExit();

        _previousState = _currentState;

        _currentState = state;


        if (_currentState != null)
        {
            _currentState.OnStateEnter(weaponType, message);
        }

        return true;
    }
}

public class FSM<T> : BaseFSM<T>
{
    public void OnUpdate() => _currentState.OnStateUpdate();
    public void OnFixedUpdate() => _currentState.OnStateFixedUpdate();
}