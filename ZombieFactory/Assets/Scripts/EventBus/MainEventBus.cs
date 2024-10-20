using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCommand : BaseCommand
{
    Action _resultEvent;

    public ResultCommand(Action resultEvent)
    {
        _resultEvent = resultEvent;
    }

    public override void Execute()
    {
        _resultEvent?.Invoke();
    }
}


public class MainEventBus : BaseBus<MainEventBus.Type>
{
    public enum Type
    {
        GameClear,
        GameOver,
    }

    public override void Publish(Type type)
    {
        if (_commands.ContainsKey(type) == false) return;
        for (int i = 0; i < _commands[type].Count; i++)
        {
            _commands[type][i].Execute();
        }
    }
}
