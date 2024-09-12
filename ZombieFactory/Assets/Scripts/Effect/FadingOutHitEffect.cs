using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class FadingOutHitEffect : HitEffect
{
    Color _startColor;
    Color _endColor;
    SpriteRenderer _effectImg;

    public override void Initialize()
    {
        base.Initialize();
        _effectImg = GetComponentInChildren<SpriteRenderer>();
        _startColor = _effectImg.color;
        _endColor = new Color(_effectImg.color.r, _effectImg.color.g, _effectImg.color.b, 0);
    }

    public override void Play()
    {
        base.Play();
        _effectImg.DOColor(_endColor, _duration);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _effectImg.color = _startColor;
        _effectImg.DOKill();
    }
}
