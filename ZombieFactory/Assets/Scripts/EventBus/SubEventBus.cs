using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPointCommand : BaseCommand
{
    Action<IPoint> GetPointEvent;

    public GetPointCommand(Action<IPoint> GetPointEvent)
    {
        this.GetPointEvent = GetPointEvent;
    }

    public override void Execute(IPoint point)
    {
        GetPointEvent?.Invoke(point);
    }
}

public class SubEventBus : BaseBus<SubEventBus.Type>
{
    public enum Type
    {
        GetCameraPoint,
    }

    public override void Publish(Type state, IPoint point)
    {
        if (_commands.ContainsKey(state) == false) return;
        _commands[state].Execute(point);
    }
}
