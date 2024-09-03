using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputable
{
    public enum Type
    {
        Escape,
        Jump,
        Move,

        CrouchStart,
        CrouchEnd,

        RunStart,
        RunEnd,
    }

    void AddEvent(Type type, BaseCommand command);
    void RemoveEvent(Type type);
}