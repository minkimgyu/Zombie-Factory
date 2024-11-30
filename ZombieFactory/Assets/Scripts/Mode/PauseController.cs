using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] Button _resumeBtn;
    [SerializeField] Button _returnToMenuBtn;
    [SerializeField] GameObject _content;

    bool _nowPause = false;
    public bool NowPause { get { return _nowPause; } }

    public void Activate()
    {
        _nowPause = !_nowPause;
        if (_nowPause)
        {
            Cursor.lockState = CursorLockMode.None;
            ServiceLocater.ReturnTimeController().ControllTime(true);
            _content.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            ServiceLocater.ReturnTimeController().ControllTime(false);
            _content.SetActive(false);
        }
    }

    public void Initialize()
    {
        IInputable inputable = ServiceLocater.ReturnInputHandler();
        inputable.AddEvent(IInputable.Type.Escape, new KeyCommand(Activate));

        _content.SetActive(false);
        _resumeBtn.onClick.AddListener(() => { Activate(); });
        _returnToMenuBtn.onClick.AddListener(() => 
        {
            IInputable inputable = ServiceLocater.ReturnInputHandler();
            inputable.Clear();

            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.StartScene); 
        });
    }
}
