using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIController : MonoBehaviour
{
    [SerializeField] WeaponViewer _previewPrefab;
    [SerializeField] Transform _content;

    Dictionary<BaseItem.Name, Sprite> _itemSpriteDictionary;
    Dictionary<BaseWeapon.Type, WeaponViewer> _previewContainer;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _previewContainer = new Dictionary<BaseWeapon.Type, WeaponViewer>();

        int size = Enum.GetValues(typeof(BaseWeapon.Type)).Length;
        for (int i = 0; i < size; i++)
        {
            WeaponViewer preview = Instantiate(_previewPrefab, _content);
            preview.ActiveViewer(false);

            _previewContainer.Add((BaseWeapon.Type)i, preview);
        }
    }

    public void AddPreview(BaseWeapon.Type weaponType, BaseItem.Name iconName)
    {
        Sprite itemIcon = _itemSpriteDictionary[iconName];
        _previewContainer[weaponType].UpdateViewer((int)weaponType, itemIcon);
        _previewContainer[weaponType].ActiveViewer(true);
    }

    public void RemovePreview(BaseWeapon.Type weaponType)
    {
        _previewContainer[weaponType].ActiveViewer(false);
    }
}
