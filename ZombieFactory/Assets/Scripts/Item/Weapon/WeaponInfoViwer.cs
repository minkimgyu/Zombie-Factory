using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeaponInfoViewer : MonoBehaviour
{
    [SerializeField] ContentSizeFitter _contentSizeFitter; 
    [SerializeField] GameObject _content;
    [SerializeField] TMP_Text _objectName;
    [SerializeField] float _offsetYPos = 0.5f;

    bool _nowActivate = false;

    public void Initialize()
    {
        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.Type.ActiveInteractableInfo, new ActiveWeaponViewerCommand(OnViewEventReceived));
        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.Type.ActiveInteractableInfo, new ActivateCommand(Activate));
    }

    void Activate(bool active)
    {
        _content.SetActive(active);
    }

    public void OnViewEventReceived(bool nowActivate, string name, Vector3 position)
    {
        _nowActivate = nowActivate;
        Activate(nowActivate);
        _objectName.text = $"Get {name}";
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_contentSizeFitter.transform);
        transform.position = position + Vector3.up * _offsetYPos;
    }

    private void Update()
    {
        if (_nowActivate == false) return;
        transform.LookAt(_content.transform.position + Camera.main.transform.forward);
    }
}
