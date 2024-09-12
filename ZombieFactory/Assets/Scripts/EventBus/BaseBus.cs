using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseBus<T>
{
    protected Dictionary<T, BaseCommand> _commands = new Dictionary<T, BaseCommand>();

    public virtual void Register(T state, BaseCommand command)
    {
        if (_commands.ContainsKey(state)) return;
        _commands.Add(state, command);
    }

    //�̺�Ʈ ����
    public virtual void Unregister(T state, BaseCommand command)
    {
        if (!_commands.ContainsKey(state)) return;
        _commands.Remove(state);
    }

    //�̺�Ʈ ����
    public virtual void Publish(T state) { }
    public virtual void Publish(T state, Vector3 cameraHolderPosition, Vector3 viewRotation) { }
    public virtual void Publish(T state, float fieldOfView, float ratio) { }
    public virtual void Publish(T state, bool active) { }

    public virtual void Publish(T state, IPoint point) { }


    // �̺�Ʈ �����
    public virtual void Clear()
    {
        _commands.Clear();
    }
}