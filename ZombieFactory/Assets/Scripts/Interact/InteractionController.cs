using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] InteractableCaptureComponent _interactableCaptureComponent;
    IInteractable _interactableTarget;
    //Action<BaseWeapon> SendWeaponToController;
    //Action<int> RefillAmmo;
    //Action<float> GetHeal;

    IInteracter _interacter;

    public void GetWeapon(BaseWeapon weapon)
    {
        _interacter.AddWeapon(weapon);
    }

    public void GetAmmoPack(int ammoCount)
    {
        _interacter.GetAmmoPack(ammoCount);
    }

    public void GetAidPack(float healPoint)
    {
        _interacter.GetAidPack(healPoint);
    }

    public void OnHandleInteract()
    {
        if (_interactableTarget as UnityEngine.Object == null) return;

        _interactableTarget.Interact(_interacter);
        _interactableTarget = null; // null로 초기화
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ActiveInteractableInfo, false);
    }

    public void Initialize()
    {
        _interacter = GetComponent<IInteracter>();

        _interactableCaptureComponent.Initialize(OnEnter, OnExit);
        WeaponController weaponController = GetComponentInParent<WeaponController>();
        BaseLife life = GetComponentInParent<BaseLife>();
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