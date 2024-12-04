using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMode : MonoBehaviour
{
    [SerializeField] CameraController _cameraController;
    [SerializeField] PlayerUIController _playerUIController;
    [SerializeField] WeaponInfoViewer _weaponInfoViewer;
    [SerializeField] Transform _effectParent;
    int _totalStageCount = 10;

    [SerializeField] PauseController _pauseController;

    StageController _stageController;
    EffectEmitter _effectEmitter;

    private void OnApplicationFocus(bool focus)
    {
        if (_pauseController.NowPause == true) return;

        if (focus) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundControllable.SoundName.InGame, 0.6f);

        _stageController = GetComponent<StageController>();
        EventBusManager.Instance.Initialize(new MainEventBus(), new ObserverEventBus());

        EventBusManager.Instance.MainEventBus.Register(
            MainEventBus.Type.GameClear, 
            new ResultCommand(() => {
                ServiceLocater.ReturnSoundPlayer().StopBGM();

                IInputable inputable = ServiceLocater.ReturnInputHandler();
                inputable.Clear();

                Cursor.lockState = CursorLockMode.None;
                ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.GameClearScene); 
            })
        );

        EventBusManager.Instance.MainEventBus.Register(
            MainEventBus.Type.GameOver, 
            new ResultCommand(() => {
                ServiceLocater.ReturnSoundPlayer().StopBGM();

                IInputable inputable = ServiceLocater.ReturnInputHandler();
                inputable.Clear();

                Cursor.lockState = CursorLockMode.None;
                ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.GameOverScene); 
            })
        );

        Initialize();
    }

    public void Initialize()
    {
        DontDestroyObjects system = FindObjectOfType<DontDestroyObjects>();

        _playerUIController.Initialize(system.AddressableHandler.ItemSpriteAssets);
        _weaponInfoViewer.Initialize();
        _pauseController.Initialize();

        HelperMediator mediator = new HelperMediator(_playerUIController.OnFreeRoleRequested, _playerUIController.OnBuildFormationRequested);
        FactoryCollection factoryCollection = new FactoryCollection(system.AddressableHandler, mediator, _effectParent);
        _cameraController.Initialize();

        _effectEmitter = new EffectEmitter(factoryCollection.Factories[FactoryCollection.Type.Effect]);
        _stageController.Initialize(_totalStageCount, factoryCollection, _cameraController, _playerUIController);

        Cursor.lockState = CursorLockMode.Locked;
    }
}
