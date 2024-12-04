using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultMenuController : MonoBehaviour
{
    [SerializeField] Button _returnToMenuBtn;
    [SerializeField] Button _exitBtn;

    private void Start()
    {
        _returnToMenuBtn.onClick.AddListener(() => 
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundControllable.SoundName.Click, 0.3f);
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.StartScene); 
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
