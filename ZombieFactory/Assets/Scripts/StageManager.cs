using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] InputHandler _inputHandler;

    [SerializeField] CameraController _cameraController;
    [SerializeField] GameUIController _gameUIController;
    [SerializeField] WeaponInfoViewer _weaponInfoViewer;

    [SerializeField] GridComponent _gridComponent;

    [SerializeField] WeaponSpawner _weaponSpawner;
    [SerializeField] ZombieSpawner _zombieSpawner;
    EffectEmitter _effectEmitter;

    private void OnApplicationFocus(bool focus)
    {
        if (focus) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        EventBusManager.Instance.Initialize(new ObserverEventBus());
        AddressableHandler addressableHandler = new AddressableHandler();
        addressableHandler.Load(() => { Initialize(addressableHandler); });
    }

    public void Initialize(AddressableHandler addressableHandler)
    {
        _gridComponent.Initialize();
        ServiceLocater.Provide(_inputHandler);

        _gameUIController.Initialize(addressableHandler.ItemSpriteAssets);
        _weaponInfoViewer.Initialize();

        FactoryCollection factoryCollection = new FactoryCollection(addressableHandler);
        _cameraController.Initialize();

        SoundController soundController = FindObjectOfType<SoundController>();
        soundController.Initialize(addressableHandler.AudioAssets, factoryCollection.Factories[FactoryCollection.Type.SoundPlayer]);
        ServiceLocater.Provide(soundController);

        _effectEmitter = new EffectEmitter(factoryCollection.Factories[FactoryCollection.Type.Effect]);

        BaseLife player = factoryCollection.Factories[FactoryCollection.Type.Life].Create(BaseLife.Name.Player);
        player.AddObserverEvent
        (
            _cameraController.MoveCamera,
            _cameraController.OnFieldOfViewChange,

            _gameUIController.ChangeHpRatio,
            _gameUIController.ActiveCrosshair,

            _gameUIController.ActiveAmmoViewer,
            _gameUIController.ChangeAmmo,
            _gameUIController.AddWeaponViewer,
            _gameUIController.RemoveWeaponViewer
        );

        BaseItem knife = factoryCollection.Factories[FactoryCollection.Type.Item].Create(BaseItem.Name.Knife);
        BaseItem classic = factoryCollection.Factories[FactoryCollection.Type.Item].Create(BaseItem.Name.Stinger);
        BaseItem phantom = factoryCollection.Factories[FactoryCollection.Type.Item].Create(BaseItem.Name.Odin);

        player.AddWeapon(knife as BaseWeapon);
        player.AddWeapon(classic as BaseWeapon);
        player.AddWeapon(phantom as BaseWeapon);
        player.transform.position = new Vector3(15, 3, 15);

        //_weaponSpawner.Initialize(factoryCollection.Factories[FactoryCollection.Type.Item]);
        //_zombieSpawner.Initialize(factoryCollection.Factories[FactoryCollection.Type.Life]);
    }
}
