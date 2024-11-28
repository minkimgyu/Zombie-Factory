using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour, ITarget
{
    IIdentifiable.Type _targetType = IIdentifiable.Type.Sound;

    public bool IsOpponent(List<IIdentifiable.Type> types)
    {
        for (int i = 0; i < types.Count; i++)
        {
            if(types[i] == _targetType) return true;
        }

        return false;
    }

    public Transform ReturnTargetPoint()
    {
        return transform;
    }

    public Transform ReturnSightPoint()
    {
        return transform;
    }
}
