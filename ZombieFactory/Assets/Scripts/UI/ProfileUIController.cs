using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileUIController : MonoBehaviour
{
    // ���� ���ο� color ������ �ֱ�
    // alpha ���� �ֱ�

    Dictionary<BaseItem.Name, Sprite> _itemSpriteDictionary;
    Dictionary<BaseLife.Name, ProfileViewer> _helperProfileDictionary = new Dictionary<BaseLife.Name, ProfileViewer>();
    [SerializeField] Transform _content;
    [SerializeField] Color startColor = Color.white;
    [SerializeField] Color endColor = Color.red;

    [SerializeField] float _dieContentAlpha = 0.2f;
    [SerializeField] float _dieBackgroundAlpha = 0.3f;

    public void AddProfile(BaseLife.Name name)
    {
        //ProfileViewer viewer = Instantiate(_viewer, _content); // factory �ڵ� �ֱ�
        //viewer.Initialize(name);
        //_helperProfileDictionary.Add(name, viewer);
    }

    public void OnChangeHpViewer(BaseLife.Name name, float ratio)
    {
        _helperProfileDictionary[name].OnHpChangeRequested(ratio, startColor, endColor);
    }

    public void OnWeaponChange(BaseLife.Name name, BaseItem.Name iconName)
    {
        _helperProfileDictionary[name].OnWeaponChangeRequested(_itemSpriteDictionary[iconName]);
    }

    public void OnDisableProfileRequested(BaseLife.Name name)
    {
        _helperProfileDictionary[name].OnDisableProfileRequested(_dieContentAlpha, _dieBackgroundAlpha);
    }
}
