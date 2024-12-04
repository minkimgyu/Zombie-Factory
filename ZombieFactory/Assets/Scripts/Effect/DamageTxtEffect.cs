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

    protected override void Update()
    {
        base.Update();
        _text.color = Color.Lerp(new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a), new Color(_text.color.r, _text.color.g, _text.color.b, 0), _timer.Ratio);
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
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
    }
}
