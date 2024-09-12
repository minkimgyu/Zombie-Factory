using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[System.Serializable]
public class DamageTxtEffect : BaseEffect
{
    Vector3 finalPoint;
    TMP_Text _text;

    public override void Initialize()
    {
        _text = GetComponentInChildren<TMP_Text>();
    }

    public override void ResetData(Vector3 hitPosition, Vector3 hitNormal, Quaternion holeRotation, float damage)
    {
        _text.text = damage.ToString();
        transform.position = hitPosition;
        transform.LookAt(Camera.main.transform);
    }

    public override void Play()
    {
        base.Play();
        _timer.Start(_duration);
    }
}
