using UnityEngine;
using System;
using System.Threading.Tasks;

abstract public class BaseStage : MonoBehaviour
{
    // 입장 위치
    [SerializeField] Transform _entryPoint;

    public Vector3 EntryPoint { get { return _entryPoint.position; } }

    // 출구 포탈
    Portal _portal;

    // 스테이지 클리어 이벤트
    protected Action OnStageClearRequested;

    // 스테이지 초기화 함수
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

    // 스테이지 초기화 함수
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

    // 노드 초기화 함수
    // Task를 이용해 비동기 처리 진행 -> 여러 스테이지를 동기로 초기화 하는 것보다 효율적
    // 완료시 OnComplete 콜백 호출
    public async void InitializeNodes(Action OnComplete)
    {
        GridComponent gridComponent = GetComponentInChildren<GridComponent>();
        await Task.Run(() => {
            gridComponent.InitializeNodes();
        });

        OnComplete?.Invoke();
    }

    // 총기, 적 스폰을 위한 함수
    public abstract void Spawn();

    // 스테이지 활성화 함수
    public virtual void Activate(Vector3 movePos) 
    {
        _portal.Active(movePos);
    }

    // 스테이지 비활성화 함수
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
}
