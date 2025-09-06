using UnityEngine;
using System;
using System.Threading.Tasks;

abstract public class BaseStage : MonoBehaviour
{
    // ���� ��ġ
    [SerializeField] Transform _entryPoint;

    public Vector3 EntryPoint { get { return _entryPoint.position; } }

    // �ⱸ ��Ż
    Portal _portal;

    // �������� Ŭ���� �̺�Ʈ
    protected Action OnStageClearRequested;

    // �������� �ʱ�ȭ �Լ�
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

    // �������� �ʱ�ȭ �Լ�
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

    // ��� �ʱ�ȭ �Լ�
    // Task�� �̿��� �񵿱� ó�� ���� -> ���� ���������� ����� �ʱ�ȭ �ϴ� �ͺ��� ȿ����
    // �Ϸ�� OnComplete �ݹ� ȣ��
    public async void InitializeNodes(Action OnComplete)
    {
        GridComponent gridComponent = GetComponentInChildren<GridComponent>();
        await Task.Run(() => {
            gridComponent.InitializeNodes();
        });

        OnComplete?.Invoke();
    }

    // �ѱ�, �� ������ ���� �Լ�
    public abstract void Spawn();

    // �������� Ȱ��ȭ �Լ�
    public virtual void Activate(Vector3 movePos) 
    {
        _portal.Active(movePos);
    }

    // �������� ��Ȱ��ȭ �Լ�
    public virtual void Disable()
    {
        _portal.Disable();

        // ������ ����
        BaseItem[] items = FindObjectsByType<BaseItem>(FindObjectsSortMode.None);
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].NowDrop() == false) continue;
            Destroy(items[i].gameObject);
        }
    }
}
