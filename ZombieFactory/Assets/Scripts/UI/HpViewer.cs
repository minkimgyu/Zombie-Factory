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

    Color startColor = Color.white;
    Color endColor = Color.red;

    float dieContentAlphaValue;
    float dieBackgroundAlphaValue;

    public void UpdateHp(float ratio)
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
