using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : BaseSpawner
{
    BaseFactory _lifeFactory;
    BaseFactory _itemFactory;

    CameraController _cameraController;
    PlayerUIController _gameUIController;

    [SerializeField] List<BaseItem.Name> _weaponNames;

    public override void Initialize(
        BaseFactory lifeFactory,
        BaseFactory itemFactory,

        CameraController cameraController,
        PlayerUIController gameUIController)
    {
        _lifeFactory = lifeFactory;
        _itemFactory = itemFactory;

        _cameraController = cameraController;
        _gameUIController = gameUIController;
    }

    public override void Spawn()
    {
        BaseLife player = _lifeFactory.Create(BaseLife.Name.Player, _weaponNames);
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

        IWeaponEquipable equipable = player.GetComponent<IWeaponEquipable>();
        for (int i = 0; i < _weaponNames.Count; i++)
        {
            BaseItem item = _itemFactory.Create(_weaponNames[i]); // 랜덤한 무기 생성 후 추가
            equipable.AddWeapon(item as BaseWeapon);
        }

        player.ResetPosition(transform.position);
    }
}