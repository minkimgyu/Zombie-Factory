using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TrajectoryLineEffect : BaseEffect
{
    Vector3 _hitPosition;

    public override void ResetData(Vector3 hitPosition, Vector3 shootPosition)
    {
        transform.position = shootPosition;
        _hitPosition = hitPosition;
    }

    protected override void Update()
    {
        base.Update();
        if(_timer.CurrentState == Timer.State.Running)
        {
            transform.position = Vector3.Lerp(transform.position, _hitPosition, _timer.Ratio);
            return;
        }
    }
}
