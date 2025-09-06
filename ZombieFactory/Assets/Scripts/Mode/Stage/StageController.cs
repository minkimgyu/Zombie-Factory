using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] BaseStage _startStage;
    [SerializeField] BaseStage[] _battleStages;
    Queue<BaseStage> _stageQueue;

    int _maxStageCount;
    int _stageCount;

    BaseStage _currentStage;
    BaseStage _nextStage;

    [SerializeField] StageViewer _stageViewer;
    FactoryCollection _factoryCollection;
    CameraController _cameraController;
    PlayerUIController _playerUIController;

    // �������� ��Ʈ�ѷ� �ʱ�ȭ
    public void Initialize(
        int maxStageCount, 
        FactoryCollection factoryCollection, 
        CameraController cameraController, 
        PlayerUIController playerUIController)
    {
        _stageQueue = new Queue<BaseStage>();

        _stageCount = 1;
        _maxStageCount = maxStageCount;
        _factoryCollection = factoryCollection;
        _cameraController = cameraController;
        _playerUIController = playerUIController;

        _stageViewer.Initialize();
        _stageViewer.OnStageChange(_stageCount);

        InitializeStages();
    }

    // �������� Ŭ���� �� ȣ��Ǵ� �Լ�
    public void OnStageClearRequested()
    {
        if (_stageQueue.Count == 0) return;

        _stageViewer.OnStageClear();
        _nextStage = _stageQueue.Dequeue();
        Vector3 entryPos = _nextStage.EntryPoint;
        _currentStage.Activate(entryPos);
    }

    // ���� ���������� �̵� ��û �� ȣ��Ǵ� �Լ�
    public void OnMoveToNextStageRequested()
    {
        if (_stageQueue.Count == 0)
        {
            EventBusManager.Instance.MainEventBus.Publish(MainEventBus.Type.GameClear);
            return;
        }

        _stageCount++;
        _stageViewer.OnStageChange(_stageCount);
        _currentStage.Disable();

        _currentStage = _nextStage;
        _nextStage = null;
        _currentStage.Spawn();
    }

    BaseStage ReturnRandomStage()
    {
        int startStageCount = _battleStages.Length;
        return _battleStages[UnityEngine.Random.Range(0, startStageCount)];
    }

    int _initializedStageCount = 0;
    int _totalStageCount = 0;

    public void InitializeStages()
    {
        _totalStageCount = 1 + _battleStages.Length;

        _startStage.Initialize(
            _factoryCollection.Factories[FactoryCollection.Type.Life],
            _factoryCollection.Factories[FactoryCollection.Type.Item],
            _factoryCollection.Factories[FactoryCollection.Type.Viewer],

            _cameraController,
            _playerUIController,
                
            OnStageClearRequested,
            OnMoveToNextStageRequested
        );

        for (int i = 0; i < _battleStages.Length; i++)
        {
            _battleStages[i].Initialize(
                _factoryCollection.Factories[FactoryCollection.Type.Life],
                _factoryCollection.Factories[FactoryCollection.Type.Item],

                OnStageClearRequested,
                OnMoveToNextStageRequested
            );
        }

        // ���� �ð� ������ ���� Stopwatch ����
        //stopwatch = new Stopwatch();
        //stopwatch.Start();

        _startStage.InitializeNodes(OnInitializeComplete);
        for (int i = 0; i < _battleStages.Length; i++)
        {
            _battleStages[i].InitializeNodes(OnInitializeComplete);
        }
    }

    // ���� �ð� ������ ���� ����
    //Stopwatch stopwatch;

    // ��� �������� �ʱ�ȭ�� �Ϸ�Ǿ��� �� ȣ��Ǵ� �ݹ� �Լ�
    void OnInitializeComplete()
    {
        _initializedStageCount++;
        if (_totalStageCount == _initializedStageCount)
        {
            // �ð� ���� ����
            // stopwatch.Stop();
            // �ɸ� �ð� ����� ���� ���� Ȯ��
            // UnityEngine.Debug.Log($"���� �ڵ� ���� �ð�: {stopwatch.ElapsedMilliseconds} ms");

            CreateStageQueue();
            StartPlay();
        }
    }

    // �������� ť ����
    void CreateStageQueue()
    {
        BaseStage storedBattleStage = null;
        _stageQueue.Enqueue(_startStage);

        for (int i = 0; i < _maxStageCount; i++)
        {
            BaseStage battleStage = ReturnRandomStage();

            if (storedBattleStage == null)
            {
                _stageQueue.Enqueue(battleStage);
                storedBattleStage = battleStage;
            }
            else
            {
                while (storedBattleStage == battleStage) battleStage = ReturnRandomStage();

                _stageQueue.Enqueue(battleStage);
                storedBattleStage = battleStage;
            }
        }
    }

    // �ش� �������� �÷��� ����
    void StartPlay()
    {
        _currentStage = _stageQueue.Dequeue();
        _currentStage.Spawn();
    }
}
