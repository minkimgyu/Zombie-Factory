using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Unity.Burst.CompilerServices;

abstract public class ActionStrategy : WeaponStrategy
{
    /// <summary>
    /// Action을 호출할 수 있는지 확인하는 함수
    /// </summary>
    public virtual bool CanExecute() { return true; }

    /// <summary>
    /// Action이 호출할 때 사용하는 함수
    /// </summary>
    public virtual void Execute() { }

    /// <summary>
    /// Aim을 해제할 때 호출되는 함수
    /// </summary>
    public virtual void TurnOffZoomDirectly() { }
}

public class NoAction : ActionStrategy
{
}