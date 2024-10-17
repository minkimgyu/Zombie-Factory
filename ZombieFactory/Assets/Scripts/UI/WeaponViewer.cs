using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponViewer : BaseViewer
{
    [SerializeField] Image[] images;

    Dictionary<BaseItem.Name, Sprite> _weaponIconSprite;

    public void Initialize(Dictionary<BaseItem.Name, Sprite> weaponIconSprite)
    {
        _weaponIconSprite = weaponIconSprite;
        images = GetComponentsInChildren<Image>();
    }

    public void AddPreview(BaseItem.Name name, BaseWeapon.Type type)
    {
        images[(int)type].sprite = _weaponIconSprite[name];
        images[(int)type].gameObject.SetActive(true);
    }

    public void RemovePreview(BaseWeapon.Type type)
    {
        images[(int)type].gameObject.SetActive(false);
    }
}
