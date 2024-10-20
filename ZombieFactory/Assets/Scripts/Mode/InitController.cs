using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ServiceLocater.Initialize();

        AddressableHandler addressableHandler = new AddressableHandler();

        InputHandler inputHandler = new InputHandler();
        ServiceLocater.Provide(inputHandler);

        SoundController soundController = FindObjectOfType<SoundController>();
        ServiceLocater.Provide(soundController);

        GameObject obj = new GameObject();
        DontDestroyObjects dontDestroyObjects = obj.AddComponent<DontDestroyObjects>();
        dontDestroyObjects.Initialize(addressableHandler, inputHandler, soundController);

        addressableHandler.Load(() => { Initialize(dontDestroyObjects); });
    }

    void Initialize(DontDestroyObjects dontDestroyObjects)
    {
        Transform playerParent = dontDestroyObjects.SoundController.transform;
        BaseFactory soundFactory = new SoundPlayerFactory(dontDestroyObjects.AddressableHandler, playerParent);
        dontDestroyObjects.SoundController.Initialize(dontDestroyObjects.AddressableHandler.AudioAssets, soundFactory);

        SceneController sceneController = new SceneController();
        ServiceLocater.Provide(sceneController);
        sceneController.ChangeScene(ISceneControllable.SceneName.StartScene);
    }
}
