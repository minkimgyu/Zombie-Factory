using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHelper
{
    void Initialize(ITarget commander, Action<IHelper> RemoveHelper);

    void ApplyOffset(Vector3 offset); // mediator¿¡¼­ ¹Þ¾Æ¿È
}
