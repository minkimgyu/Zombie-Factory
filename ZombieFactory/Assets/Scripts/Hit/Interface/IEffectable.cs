using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
    public enum ConditionType
    {
        Penetration,
        NonPenetration,
        Stabbing
    }

    void effectFactory(ConditionType effectType);
    void effectFactory(ConditionType effectType, Vector3 hitPosition, Vector3 shootPosition, Quaternion holeRotation);

    void effectFactory(ConditionType effectType, Vector3 hitPosition, Vector3 hitNormal);
    void effectFactory(ConditionType effectType, float damage, Vector3 hitPosition, Vector3 hitNormal);
}