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
        _startBtn.onClick.AddListener(() => { ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.PlayScene); });
        _exitBtn.onClick.AddListener(OnExit);
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
