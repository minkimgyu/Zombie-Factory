using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHelper
{
    void InitializeHelper(ITarget commander, Action<IHelper> RemoveHelper);
    void RestOffset(Vector3 offset); // mediator¿¡¼­ ¹Þ¾Æ¿È
}
