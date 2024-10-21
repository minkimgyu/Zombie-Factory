using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : PoolObject
{
    [SerializeField] float _duration;
    [SerializeField] Transform _rig;
    Action ReturnToPool;

    public void Activate(Transform origin)
    {
        StartTimer(_duration);
        CopyAnimCharacterTransformToRagdoll(origin, _rig);
    }

    void CopyAnimCharacterTransformToRagdoll(Transform origin, Transform rig)
    {
        rig.position = origin.position;
        rig.rotation = origin.rotation;

        for (int i = 0; i < origin.transform.childCount; i++)
        {
            if (origin.childCount != rig.childCount) continue;
            if (origin.transform.childCount != 0)
            {
                CopyAnimCharacterTransformToRagdoll(origin.transform.GetChild(i), rig.transform.GetChild(i));
            }

            rig.transform.GetChild(i).localPosition = origin.transform.GetChild(i).localPosition;
            rig.transform.GetChild(i).localRotation = origin.transform.GetChild(i).localRotation;
        }
    }
}
