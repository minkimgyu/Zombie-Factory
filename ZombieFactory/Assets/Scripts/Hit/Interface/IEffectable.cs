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

    void CreateEffect(ConditionType effectType);
    void CreateEffect(ConditionType effectType, Vector3 hitPosition, Vector3 shootPosition, Quaternion holeRotation);

    void CreateEffect(ConditionType effectType, Vector3 hitPosition, Vector3 hitNormal);
    void CreateEffect(ConditionType effectType, float damage, Vector3 hitPosition, Vector3 hitNormal);
}