using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseStage : MonoBehaviour
{
    [SerializeField] Transform _entryPoint;
    Portal _portal;

    protected Action OnStageClearRequested;
    Action OnMoveToNextStageRequested;

    public virtual void Initialize(
        BaseFactory lifeFactory,
        BaseFactory weaponFactory,
        BaseFactory viewerFactory, 

        CameraController cameraController,
        PlayerUIController plaUIController,
        
        Action OnStageClearRequested,
        Action OnMoveToNextStageRequested)
    {
        this.OnStageClearRequested = OnStageClearRequested;

        GridComponent gridComponent = GetComponentInChildren<GridComponent>();
        gridComponent.Initialize();

        _portal = GetComponentInChildren<Portal>();
        _portal.Initialize(OnMoveToNextStageRequested);
    }

    public virtual void Initialize(
        BaseFactory lifeFactory,
        BaseFactory weaponFactory,

        Action OnStageClearRequested,
        Action OnMoveToNextStageRequested)
    {
        this.OnStageClearRequested = OnStageClearRequested;

        GridComponent gridComponent = GetComponentInChildren<GridComponent>();
        gridComponent.Initialize();

        _portal = GetComponentInChildren<Portal>();
        _portal.Initialize(OnMoveToNextStageRequested);
    }

    public abstract void Spawn();

    public virtual void Activate(Vector3 movePos) 
    {
        _portal.Active(movePos);
    }

    public virtual void Disable()
    {
        _portal.Disable();
    }

    public Vector3 ReturnEntryPosition()
    {
        return _entryPoint.position;
    }
}
