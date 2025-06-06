using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InitController : MonoBehaviour
{
    [SerializeField] Image _loadindImg;
    [SerializeField] TMP_Text _loadingTxt;

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

        addressableHandler.Load
        (
            () => { Initialize(dontDestroyObjects); }, 
            (ratio) => { _loadindImg.fillAmount = ratio; _loadingTxt.text = $"{Mathf.RoundToInt(ratio * 100)}%"; }
        );
    }

    void Initialize(DontDestroyObjects dontDestroyObjects)
    {
        Transform playerParent = dontDestroyObjects.SoundController.transform;
        BaseFactory soundFactory = new SoundPlayerFactory(dontDestroyObjects.AddressableHandler, playerParent);
        dontDestroyObjects.SoundController.Initialize(dontDestroyObjects.AddressableHandler.AudioAssets, soundFactory);

        SceneController sceneController = new SceneController();
        TimeController timeController = new TimeController();
        ServiceLocater.Provide(sceneController);
        ServiceLocater.Provide(timeController);

        sceneController.ChangeScene(ISceneControllable.SceneName.StartScene);
    }
}
