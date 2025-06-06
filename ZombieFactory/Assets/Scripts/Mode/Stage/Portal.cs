using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Portal : MonoBehaviour, IInteractable
{
    Vector3 _movePosition;
    bool _isActive;
    Action OnMoveToNextStageRequested;

    [SerializeField] Transform _infoPoint;

    public void Initialize(Action OnMoveToNextStageRequested)
    {
        this.OnMoveToNextStageRequested = OnMoveToNextStageRequested;
    }

    public void Active(Vector3 movePos)
    {
        _movePosition = movePos;
        _isActive = true;
    }

    public void Disable()
    {
        _isActive = false;
    }

    public void Interact(IInteracter interacter)
    {
        if (_isActive == false) return;

        OnMoveToNextStageRequested?.Invoke();
        interacter.TeleportTo(_movePosition);
    }

    public bool IsInteractable()
    {
        return true;
    }

    public void OnSightEnter()
    {
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ActiveInteractableInfo, true, "Move to next stage", _infoPoint.position);
    }

    public void OnSightExit()
    {
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ActiveInteractableInfo, false);
    }
}
