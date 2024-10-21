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

    void SpawnEffect(ConditionType effectType) { }
    void SpawnEffect(ConditionType effectType, Vector3 hitPosition, Vector3 hitNormal) { }
    void SpawnEffect(float damage, Vector3 hitPosition, Vector3 hitNormal) { }
    void SpawnEffect(ConditionType effectType, Vector3 hitPosition, Vector3 shootPosition, Quaternion holeRotation) { }
}