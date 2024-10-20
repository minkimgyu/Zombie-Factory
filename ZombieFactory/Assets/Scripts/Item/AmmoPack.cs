using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : BaseItem, IInteractable
{
    [SerializeField] protected Name _itemName;

    public void Interact(IInteracter interacter)
    {
        interacter.GetAmmoPack(_ammoCount);
        Destroy(gameObject);
    }

    int _ammoCount = 0;

    public override void ResetData(AmmoPackData data)
    {
        _ammoCount = data.ammoCount;
    }

    public bool IsInteractable()
    {
        return true;
    }

    public void OnSightEnter()
    {
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ActiveInteractableInfo, true, _itemName.ToString(), transform.position);
    }

    public void OnSightExit()
    {
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ActiveInteractableInfo, false);
    }
}
