using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseBus<T>
{
    protected Dictionary<T, List<BaseCommand>> _commands;

    public BaseBus()
    {
        _commands = new Dictionary<T, List<BaseCommand>>();
    }


    public virtual void Register(T state, BaseCommand command)
    {
        if (_commands.ContainsKey(state) == false)
        {
            _commands[state] = new List<BaseCommand>();
        }

        List<BaseCommand> commands = _commands[state];
        commands.Add(command);
    }

    //이벤트 해제
    public virtual void Unregister(T state, BaseCommand command)
    {
        if (_commands.ContainsKey(state) == false)
        {
            _commands[state] = new List<BaseCommand>();
        }

        List<BaseCommand> commands = _commands[state];
        commands.Remove(command);
    }

    //이벤트 실행
    public virtual void Publish(T state) { }
    public virtual void Publish(T state, Vector3 cameraHolderPosition, Vector3 viewRotation) { }
    public virtual void Publish(T state, float fieldOfView, float ratio) { }
    public virtual void Publish(T state, bool active) { }

    public virtual void Publish(T state, bool nowActivate, string name, Vector3 position) { }

    public virtual void Publish(T state, IPoint point) { }
    public virtual void Publish(T state, BaseEffect.Name name, Vector3 hitPosition, Vector3 hitNormal) { }

    // 이벤트 지우기
    public virtual void Clear()
    {
        _commands.Clear();
    }
}