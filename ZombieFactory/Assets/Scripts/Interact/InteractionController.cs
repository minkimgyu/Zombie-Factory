using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] InteractableCaptureComponent _interactableCaptureComponent;
    IInteractable _interactableTarget;
    Action<BaseWeapon> SendWeaponToController;

    public void OnHandleInteract()
    {
        if(_interactableTarget == null) return;

        BaseWeapon weapon = _interactableTarget.ReturnComponent<BaseWeapon>();
        if (weapon == null) return;

        SendWeaponToController?.Invoke(weapon);
    }

    public void Initialize()
    {
        _interactableCaptureComponent.Initialize(OnEnter, OnExit);
        WeaponController weaponController = GetComponentInParent<WeaponController>();
        SendWeaponToController = weaponController.OnWeaponReceived;
    }

    void OnEnter(IInteractable interactable)
    {
        if (CanInteract(interactable) == false) return;

        if (_interactableTarget != null) _interactableTarget.OnSightExit();

        _interactableTarget = interactable;
        _interactableTarget.OnSightEnter();
    }

    void OnExit(IInteractable interactable)
    {
        if (_interactableTarget != null) _interactableTarget.OnSightExit();
    }

    bool CanInteract(IInteractable target)
    {
        return target != null && target.IsInteractable() == true;
    }
}