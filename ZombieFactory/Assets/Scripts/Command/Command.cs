using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseCommand
{
    public virtual void Execute() { }
    public virtual void Execute(Vector2 dir) { }
    public virtual void Execute(Vector3 dir) { }
    public virtual void Execute(Vector3 cameraHolderPosition, Vector3 viewRotation) { }
    public virtual void Execute(float fieldOfView, float ratio) { }
    public virtual void Execute(int inMagazine, int inPossession) { }

    public virtual void Execute(BaseItem.Name name, BaseWeapon.Type type) { }
    public virtual void Execute(BaseWeapon.Type type) { }

    public virtual void Execute(bool active) { }
    public virtual void Execute(BaseWeapon.EventType type) { }

    public virtual void Execute(bool nowActivate, string name, Vector3 position) { }


    public virtual void Execute(BaseEffect.Name name, Vector3 hitPosition, Vector3 hitNormal) { }
    public virtual void Execute(IPoint point) { }
}

public class InputEventCommand : BaseCommand
{
    Action<BaseWeapon.EventType> _inputEvent;

    public InputEventCommand(Action<BaseWeapon.EventType> inputEvent)
    {
        _inputEvent = inputEvent;
    }

    public override void Execute(BaseWeapon.EventType eventType)
    {
        _inputEvent?.Invoke(eventType);
    }
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

public class EquipCommand : BaseCommand
{
    Action<BaseWeapon.Type> EquipEvent;

    public EquipCommand(Action<BaseWeapon.Type> EquipEvent)
    {
        this.EquipEvent = EquipEvent;
    }

    public override void Execute(BaseWeapon.Type type)
    {
        EquipEvent?.Invoke(type);
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

public class ViewCommand : BaseCommand
{
    Action<Vector2> _inputEvent;

    public ViewCommand(Action<Vector2> inputEvent)
    {
        _inputEvent = inputEvent;
    }

    public override void Execute(Vector2 dir)
    {
        _inputEvent?.Invoke(dir);
    }
}