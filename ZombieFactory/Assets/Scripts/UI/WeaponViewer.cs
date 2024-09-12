using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponViewer : BaseViewer
{
    [SerializeField] TMP_Text _numTxt;
    [SerializeField] Image _weaponImg;

    public override void ActiveViewer(bool active)
    {
        gameObject.SetActive(active);
    }

    public override void UpdateViewer(int index, Sprite sprite)
    {
        _numTxt.text = index.ToString();
        _weaponImg.sprite = sprite;
    }
}
