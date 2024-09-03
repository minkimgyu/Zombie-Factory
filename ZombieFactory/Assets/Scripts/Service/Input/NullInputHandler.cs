using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NullInputHandler : IInputable
{
    public void AddEvent(IInputable.Type type, BaseCommand command) { }
    public void RemoveEvent(IInputable.Type type) { }
}
