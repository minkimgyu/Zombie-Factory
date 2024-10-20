using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidPack : BaseItem, IInteractable
{
    [SerializeField] protected Name _itemName;

    public void Interact(IInteracter interacter)
    {
        interacter.GetAidPack(_healPoint);
        Destroy(gameObject);
    }

    float _healPoint = 0;

    public override void ResetData(AidPackData data)
    {
        _healPoint = data.healPoint;
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
