using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEventBus : BaseBus<SubEventBus.Type>
{
    public enum Type
    {
        GetCameraPoint,
    }
}
