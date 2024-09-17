using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileViewer : BaseViewer
{
    [SerializeField] Image _profile;
    [SerializeField] TMP_Text _name;

    [SerializeField] Image _equipedWeaponImg;
    [SerializeField] HpViewer _hpViewer;

    public void Initialize(Sprite profileSprite, string name)
    {
        _profile.sprite = profileSprite;
        _name.text = name.ToString();
    }

    public void OnHpChangeRequested(float ratio, Color startColor, Color endColor)
    {
        _hpViewer.UpdateViewer(ratio, startColor, endColor);
    }

    public void OnWeaponChangeRequested(Sprite weaponSprite)
    {
        _equipedWeaponImg.sprite = weaponSprite;
    }

    public void OnDisableProfileRequested(float dieAlphaValue, float dieBackgroundAlpha)
    {
        _profile.color = new Color(_profile.color.r, _profile.color.g, _profile.color.b, dieAlphaValue);
        _name.color = new Color(_name.color.r, _name.color.g, _name.color.b, dieAlphaValue);
        _equipedWeaponImg.color = new Color(_equipedWeaponImg.color.r, _equipedWeaponImg.color.g, _equipedWeaponImg.color.b, dieAlphaValue);

        //_hpViewer.OnDieRequested(dieAlphaValue, dieBackgroundAlpha);
    }
}
