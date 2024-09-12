using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveCameraCommand : BaseCommand
{
    Action<Vector3, Vector3> MoveEvent;

    public MoveCameraCommand(Action<Vector3, Vector3> MoveEvent)
    {
        this.MoveEvent = MoveEvent;
    }

    public override void Execute(Vector3 cameraHolderPosition, Vector3 viewRotation)
    {
        MoveEvent?.Invoke(cameraHolderPosition, viewRotation);
    }
}

public class ChangeFieldOfViewCommand : BaseCommand
{
    Action<float, float> ChangeEvent;

    public ChangeFieldOfViewCommand(Action<float, float> ChangeEvent)
    {
        this.ChangeEvent = ChangeEvent;
    }

    public override void Execute(float fieldOfView, float ratio)
    {
        ChangeEvent?.Invoke(fieldOfView, ratio);
    }
}

public class ActiveCommand : BaseCommand
{
    Action<bool> ActivateEvent;

    public ActiveCommand(Action<bool> ActivateEvent)
    {
        this.ActivateEvent = ActivateEvent;
    }

    public override void Execute(bool active)
    {
        ActivateEvent?.Invoke(active);
    }
}

public class ObserverEventBus : BaseBus<ObserverEventBus.Type>
{
    public enum Type
    {
        ActiveZoom,
        MoveCamera,
        ChangeFieldOfView,

        ActiveCrosshair
    }

    public override void Publish(Type state, bool active)
    {
        if (_commands.ContainsKey(state) == false) return;
        _commands[state].Execute(active);
    }

    public override void Publish(Type state, float fieldOfView, float ratio)
    {
        if (_commands.ContainsKey(state) == false) return;
        _commands[state].Execute(fieldOfView, ratio);
    }

    public override void Publish(Type state, Vector3 cameraHolderPosition, Vector3 viewRotation)
    {
        if (_commands.ContainsKey(state) == false) return;
        _commands[state].Execute(cameraHolderPosition, viewRotation);
    }
}
