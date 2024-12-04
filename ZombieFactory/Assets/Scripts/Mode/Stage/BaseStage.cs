using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

abstract public class BaseStage : MonoBehaviour
{
    [SerializeField] Transform _entryPoint;
    Portal _portal;

    protected Action OnStageClearRequested;
    Action OnMoveToNextStageRequested;

    public virtual void Initialize(
        BaseFactory lifeFactory,
        BaseFactory weaponFactory,

        Action OnStageClearRequested,
        Action OnMoveToNextStageRequested)
    {
        this.OnStageClearRequested = OnStageClearRequested;

        GridComponent gridComponent = GetComponentInChildren<GridComponent>();
        gridComponent.Initialize();
        gridComponent.CreateGrid();

        _portal = GetComponentInChildren<Portal>();
        _portal.Initialize(OnMoveToNextStageRequested);
    }

    public async void InitializeNodes(Action OnComplete)
    {
        GridComponent gridComponent = GetComponentInChildren<GridComponent>();
        await Task.Run(() => { gridComponent.InitializeNodes(); });

        OnComplete?.Invoke();
    }

    public virtual void Initialize(
        BaseFactory lifeFactory,
        BaseFactory weaponFactory,
        BaseFactory viewerFactory, 

        CameraController cameraController,
        PlayerUIController plaUIController,
        
        Action OnStageClearRequested,
        Action OnMoveToNextStageRequested)
    {
        Initialize(lifeFactory, weaponFactory, OnStageClearRequested, OnMoveToNextStageRequested);
    }

    public abstract void Spawn();

    public virtual void Activate(Vector3 movePos) 
    {
        _portal.Active(movePos);
    }

    public virtual void Disable()
    {
        _portal.Disable();

        // 아이템 삭제
        BaseItem[] items = FindObjectsByType<BaseItem>(FindObjectsSortMode.None);
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].NowDrop() == false) continue;
            Destroy(items[i].gameObject);
        }
    }

    public Vector3 ReturnEntryPosition()
    {
        return _entryPoint.position;
    }
}
