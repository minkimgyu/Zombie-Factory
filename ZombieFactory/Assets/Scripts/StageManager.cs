using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] List<Wall> walls;
    [SerializeField] InputHandler _inputHandler;

    [SerializeField] CameraController _cameraController;
    [SerializeField] GameUIController _gameUIController;

    AddressableHandler _addressableHandler;
    FactoryCollection _factoryCollection;

    private void Awake()
    {
        ServiceLocater.Provide(_inputHandler);
    }

    private void Start()
    {
        EventBusManager.Instance.Initialize(new ObserverEventBus());
        _addressableHandler = new AddressableHandler();
        _addressableHandler.Load(Initialize);
    }

    public void Initialize()
    {
        _factoryCollection = new FactoryCollection(_addressableHandler);
        _cameraController.Initialize();

        BaseLife player = _factoryCollection.Factories[FactoryCollection.Type.Life].Create(BaseLife.Name.Player);
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

        BaseItem knife = _factoryCollection.Factories[FactoryCollection.Type.Weapon].Create(BaseItem.Name.Knife);
        BaseItem classic = _factoryCollection.Factories[FactoryCollection.Type.Weapon].Create(BaseItem.Name.Stinger);
        BaseItem phantom = _factoryCollection.Factories[FactoryCollection.Type.Weapon].Create(BaseItem.Name.Guardian);

        player.AddWeapon(knife as BaseWeapon);
        player.AddWeapon(classic as BaseWeapon);
        player.AddWeapon(phantom as BaseWeapon);


        for (int i = 0; i < walls.Count; i++)
        {
            walls[i].Initialize(_factoryCollection.Factories[FactoryCollection.Type.Effect]);
        }
    }
}
