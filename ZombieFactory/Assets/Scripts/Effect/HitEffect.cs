using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitEffect : BaseEffect
{
    float spaceBetweenWall = 0.001f;
    ParticleSystem[] _objectBurstEffects;

    public override void Initialize()
    {
        _objectBurstEffects = GetComponentsInChildren<ParticleSystem>();
    }

    public override void ResetData(Vector3 hitPosition)
    {
        transform.position = hitPosition;
    }

    public override void ResetData(Vector3 hitPosition, Vector3 hitNormal, Quaternion holeRotation)
    {
        transform.position = hitPosition + (hitNormal * spaceBetweenWall);
        transform.rotation = holeRotation * transform.rotation;
    }

    public override void Play()
    {
        base.Play();
        for (int i = 0; i < _objectBurstEffects.Length; i++)
        {
            _objectBurstEffects[i].Play();
        }
    }
}
