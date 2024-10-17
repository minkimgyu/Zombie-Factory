using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class InteractionController : MonoBehaviour, IInteracter
{
    [SerializeField] InteractableCaptureComponent _interactableCaptureComponent;
    IInteractable _interactableTarget;
    Action<BaseWeapon> SendWeaponToController;
    Action<int> RefillAmmo;
    Action<float> GetHeal;

    public void GetWeapon(BaseWeapon weapon)
    {
        SendWeaponToController?.Invoke(weapon);
    }

    public void GetAmmoPack(int ammoCount)
    {
        RefillAmmo?.Invoke(ammoCount);
    }

    public void GetAidPack(float healPoint)
    {
        GetHeal?.Invoke(healPoint);
    }

    public void OnHandleInteract()
    {
        if (_interactableTarget as UnityEngine.Object == null) return;

        _interactableTarget.Interact(this);
        _interactableTarget = null; // null로 초기화
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ActiveItemInfo, false);
    }

    public void Initialize()
    {
        _interactableCaptureComponent.Initialize(OnEnter, OnExit);
        WeaponController weaponController = GetComponentInParent<WeaponController>();
        BaseLife life = GetComponentInParent<BaseLife>();

        SendWeaponToController = weaponController.OnWeaponReceived;
        RefillAmmo = weaponController.RefillAmmo;
        GetHeal = life.GetHeal;
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
        if (_interactableTarget != null)
        {
            _interactableTarget.OnSightExit();
            _interactableTarget = null;
        }
    }

    bool CanInteract(IInteractable target)
    {
        return target != null && target.IsInteractable() == true;
    }
}