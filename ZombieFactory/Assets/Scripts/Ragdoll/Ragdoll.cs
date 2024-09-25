using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : PoolObject
{
    [SerializeField] Transform _rag;
    Action ReturnToPool;

    public void Activate(Transform origin)
    {
        StartTimer();
        CopyAnimCharacterTransformToRagdoll(origin, _rag);
    }

    void CopyAnimCharacterTransformToRagdoll(Transform origin, Transform rag)
    {
        rag.position = origin.position;
        rag.rotation = origin.rotation;

        for (int i = 0; i < origin.transform.childCount; i++)
        {
            if (origin.childCount != rag.childCount) continue;
            if (origin.transform.childCount != 0)
            {
                CopyAnimCharacterTransformToRagdoll(origin.transform.GetChild(i), rag.transform.GetChild(i));
            }

            rag.transform.GetChild(i).localPosition = origin.transform.GetChild(i).localPosition;
            rag.transform.GetChild(i).localRotation = origin.transform.GetChild(i).localRotation;
        }
    }
}
