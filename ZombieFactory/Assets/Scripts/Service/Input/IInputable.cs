using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputable
{
    public enum Type
    {
        View,

        Escape,
        Jump,
        Move,

        CrouchStart,
        CrouchEnd,

        RunStart,
        RunEnd,

        Equip,
        Reload,
        Drop,
        Interact,

        EventEnd,
        EventStart,

        RightMouseButtonUp,
        RightMouseButtonDown,
    }

    void AddEvent(Type type, BaseCommand command);
    void RemoveEvent(Type type);
}