using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StageManager : MonoBehaviour
{
    [SerializeField] List<Wall> walls;
    [SerializeField] InputHandler _inputHandler;
    [SerializeField] CameraController _cameraController;
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

        BaseLife life = _factoryCollection.Factories[FactoryCollection.Type.ArmedCharacter].Create(BaseLife.Name.Player, new List<BaseItem.Name> { BaseItem.Name.Bat, BaseItem.Name.AR });

        for (int i = 0; i < walls.Count; i++)
        {
            walls[i].Initialize(_factoryCollection.Factories[FactoryCollection.Type.Effect]);
        }
    }
}
