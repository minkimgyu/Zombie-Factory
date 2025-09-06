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

    // 스테이지 컨트롤러 초기화
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

    // 스테이지 클리어 시 호출되는 함수
    public void OnStageClearRequested()
    {
        if (_stageQueue.Count == 0) return;

        _stageViewer.OnStageClear();
        _nextStage = _stageQueue.Dequeue();
        Vector3 entryPos = _nextStage.EntryPoint;
        _currentStage.Activate(entryPos);
    }

    // 다음 스테이지로 이동 요청 시 호출되는 함수
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

        // 수행 시간 측정을 위한 Stopwatch 시작
        //stopwatch = new Stopwatch();
        //stopwatch.Start();

        _startStage.InitializeNodes(OnInitializeComplete);
        for (int i = 0; i < _battleStages.Length; i++)
        {
            _battleStages[i].InitializeNodes(OnInitializeComplete);
        }
    }

    // 수행 시간 측정을 위한 변수
    //Stopwatch stopwatch;

    // 모든 스테이지 초기화가 완료되었을 때 호출되는 콜백 함수
    void OnInitializeComplete()
    {
        _initializedStageCount++;
        if (_totalStageCount == _initializedStageCount)
        {
            // 시간 측정 종료
            // stopwatch.Stop();
            // 걸린 시간 출력을 통한 성능 확인
            // UnityEngine.Debug.Log($"최종 코드 수행 시간: {stopwatch.ElapsedMilliseconds} ms");

            CreateStageQueue();
            StartPlay();
        }
    }

    // 스테이지 큐 생성
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

    // 해당 스테이지 플레이 시작
    void StartPlay()
    {
        _currentStage = _stageQueue.Dequeue();
        _currentStage.Spawn();
    }
}
