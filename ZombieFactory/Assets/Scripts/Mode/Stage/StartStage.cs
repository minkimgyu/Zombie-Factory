using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartStage : BaseStage
{
    PlayerSpawner _playerSpawner;
    SwatSpawner _swatSpawner;
    ItemSpawner _weaponSpawner;

    public override void Initialize(
        BaseFactory lifeFactory, 
        BaseFactory weaponFactory, 
        BaseFactory viewerFactory, 

        CameraController cameraController,
        PlayerUIController gameUIController,

        Action OnStageClearRequested,
        Action OnMoveToNextStageRequested)
    {
        base.Initialize(lifeFactory, weaponFactory, viewerFactory, cameraController, gameUIController, OnStageClearRequested, OnMoveToNextStageRequested);

        _playerSpawner = GetComponentInChildren<PlayerSpawner>();
        _swatSpawner = GetComponentInChildren<SwatSpawner>();
        _weaponSpawner = GetComponentInChildren<ItemSpawner>();

        _playerSpawner.Initialize(lifeFactory, weaponFactory, cameraController, gameUIController);
        _swatSpawner.Initialize(lifeFactory, viewerFactory);
        _weaponSpawner.Initialize(weaponFactory);
    }

    public override void Spawn()
    {
        OnStageClearRequested?.Invoke();
        _playerSpawner.Spawn();
        _swatSpawner.Spawn();
        _weaponSpawner.Spawn();
    }
}
