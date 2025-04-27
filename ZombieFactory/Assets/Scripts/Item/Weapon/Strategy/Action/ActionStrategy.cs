using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Unity.Burst.CompilerServices;

abstract public class ActionStrategy : WeaponStrategy
{
    /// <summary>
    /// Action�� ȣ���� �� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    public virtual bool CanExecute() { return true; }

    /// <summary>
    /// Action�� ȣ���� �� ����ϴ� �Լ�
    /// </summary>
    public virtual void Execute() { }

    /// <summary>
    /// Aim�� ������ �� ȣ��Ǵ� �Լ�
    /// </summary>
    public virtual void TurnOffZoomDirectly() { }
}

public class NoAction : ActionStrategy
{
}