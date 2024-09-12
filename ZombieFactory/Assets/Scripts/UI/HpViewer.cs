using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class HpViewer : BaseViewer
{
    [SerializeField] Image _background;
    [SerializeField] Image _content;

    public void OnDieRequested(float dieContentAlphaValue, float dieBackgroundAlphaValue)
    {
        _content.color = new Color(_content.color.r, _content.color.g, _content.color.b, dieContentAlphaValue);
        _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, dieBackgroundAlphaValue);
    }

    public override void UpdateViewer(float ratio, Color startColor, Color endColor)
    {
        Color mixColor = Color.Lerp(endColor, startColor, ratio);
        _content.DOFillAmount(ratio, 0.5f);
        _content.DOBlendableColor(mixColor, 0.5f);
    }

    private void OnDisable()
    {
        DOTween.Kill(_content);
    }
}
