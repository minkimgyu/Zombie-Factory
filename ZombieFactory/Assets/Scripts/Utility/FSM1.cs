using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseFSM1<T>
{
    Dictionary<T, BaseState1<T>> _states;

    protected BaseState1<T> _currentState;
    protected BaseState1<T> _previousState;

    public void Initialize(Dictionary<T, BaseState1<T>> states, T startState)
    {
        _currentState = null;
        _previousState = null;

        _states = states;
        SetState(startState);
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

    bool ChangeState(BaseState1<T> state)
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
    bool ChangeState(BaseState1<T> state, BaseWeapon newWeapon, string message)
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
    bool ChangeState(BaseState1<T> state, BaseWeapon.Type weaponType, string message)
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

public class FSM1<T> : BaseFSM1<T>
{
    public void OnUpdate() => _currentState.OnStateUpdate();
}