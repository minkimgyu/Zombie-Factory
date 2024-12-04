using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] Button _startBtn;
    [SerializeField] Button _exitBtn;

    private void Start()
    {
        ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundControllable.SoundName.Lobby, 0.6f);

        _startBtn.onClick.AddListener(() => 
        { 
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundControllable.SoundName.Click, 0.3f); 
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.PlayScene); 
        });


        _exitBtn.onClick.AddListener(() => 
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundControllable.SoundName.Click, 0.3f);
            OnExit();
        });
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
