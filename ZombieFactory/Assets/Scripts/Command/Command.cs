using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseCommand
{
    public virtual void Execute() { }
    public virtual void Execute(Vector3 dir) { }
}

public class KeyCommand : BaseCommand
{
    Action _inputEvent;

    public KeyCommand(Action inputEvent)
    {
        _inputEvent = inputEvent;
    }

    public override void Execute()
    {
        _inputEvent?.Invoke();
    }
}

public class MoveCommand : BaseCommand
{
    Action<Vector3> _inputEvent;

    public MoveCommand(Action<Vector3> inputEvent)
    {
        _inputEvent = inputEvent;
    }

    public override void Execute(Vector3 dir)
    {
        _inputEvent?.Invoke(dir);
    }
}