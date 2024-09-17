using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseStrategy
{
    public virtual void OnUpdate() { }

    public virtual void LinkEvent(WeaponBlackboard blackboard) { }

    public virtual void UnlinkEvent(WeaponBlackboard blackboard) { }
}