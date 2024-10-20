using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] CameraController _cameraController;
    [SerializeField] PlayerUIController _playerUIController;
    [SerializeField] WeaponInfoViewer _weaponInfoViewer;
    [SerializeField] Transform _effectParent;

    StageController _stageController;
    EffectEmitter _effectEmitter;

    private void OnApplicationFocus(bool focus)
    {
        if (focus) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        _stageController = GetComponent<StageController>();
        EventBusManager.Instance.Initialize(new MainEventBus(), new ObserverEventBus());

        EventBusManager.Instance.MainEventBus.Register(
            MainEventBus.Type.GameClear, 
            new ResultCommand(() => {
                Cursor.lockState = CursorLockMode.None;
                ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.GameClearScene); 
            })
        );

        EventBusManager.Instance.MainEventBus.Register(
            MainEventBus.Type.GameOver, 
            new ResultCommand(() => {
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

        HelperMediator mediator = new HelperMediator();
        FactoryCollection factoryCollection = new FactoryCollection(system.AddressableHandler, mediator, _effectParent);
        _cameraController.Initialize();

        _effectEmitter = new EffectEmitter(factoryCollection.Factories[FactoryCollection.Type.Effect]);
        _stageController.Initialize(15, factoryCollection, _cameraController, _playerUIController);
    }
}
